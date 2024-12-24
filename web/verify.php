<?php
// 获取用户提交的数据
$email = $_POST['email'] ?? $_GET['email'] ?? '';
$code = $_POST['verification_code'] ?? $_GET['code'] ?? '';
$user_ip = $_POST['hidden_user_ip'] ?? $_GET['user_ip'] ?? $_SERVER['HTTP_X_FORWARDED_FOR'] ?? $_SERVER['REMOTE_ADDR'];

// 如果 IP 为空，设置默认值
$user_ip = empty($user_ip) ? 'unknown_ip' : $user_ip;

// 验证邮箱格式是否正确，限制域名为 qq.com 且用户名为纯数字
if (!preg_match('/^[0-9]+@qq\.com$/', $email)) {
    die('邮箱格式错误，仅支持 qq.com 域名且用户名为纯数字的邮箱');
}

// 验证 IP 地址格式是否正确
if (!filter_var($user_ip, FILTER_VALIDATE_IP)) {
    die('IP 地址格式错误');
}

// 检查输入的必要参数
if (empty($email) || empty($code)) {
    die('缺少必要参数：请确保邮箱和验证码已填写');
}

// 连接数据库
require 'db.php';

// 删除所有过期验证码记录
$delete_expired = $pdo->prepare("DELETE FROM email_verifications_temp WHERE expires_at < NOW()");
$delete_expired->execute();

// 查找最新的验证码记录并验证
$stmt = $pdo->prepare("SELECT * FROM email_verifications_temp WHERE email = :email ORDER BY created_at DESC LIMIT 1");
$stmt->execute([':email' => $email]);
$record = $stmt->fetch(PDO::FETCH_ASSOC);

// 验证验证码是否存在并匹配
if (!$record || $record['code'] !== $code) {
    die('验证码错误或不存在');
}

// 检查验证码是否过期，5分钟过期
if (strtotime($record['expires_at']) < time()) {
    die('验证码已过期');
}

// 验证成功后删除已使用的验证码记录
$delete_used_code = $pdo->prepare("DELETE FROM email_verifications_temp WHERE email = :email AND code = :code");
$delete_used_code->execute([':email' => $email, ':code' => $code]);

// 判断验证码是否是6位数
if (strlen($code) == 6) {
    // 如果验证码是6位数，更新 IP 地址，而不是删除旧的记录
    $update_ip = $pdo->prepare("UPDATE email_verification SET ip_address = :ip_address WHERE email = :email");
    $update_ip->execute([':email' => $email, ':ip_address' => $user_ip]);

    // 如果没有更新任何记录（邮箱不存在），则插入新记录
    if ($update_ip->rowCount() == 0) {
        $insert = $pdo->prepare("INSERT INTO email_verification (email, ip_address) VALUES (:email, :ip_address)");
        $insert->execute([':email' => $email, ':ip_address' => $user_ip]);
    }

    echo '验证成功，您的IP已更新或加入白名单！';
} else {
    // 如果验证码不是6位数，检查数据库是否已存在相同的 email
    $check_existing = $pdo->prepare("SELECT 1 FROM email_verification WHERE email = :email LIMIT 1");
    $check_existing->execute([':email' => $email]);

    if ($check_existing->fetchColumn()) {
        // 如果存在相同的 email，选择跳过插入
        echo '该邮箱已经存在验证记录，无需再次验证。';
    } else {
        // 插入新的验证记录
        $insert = $pdo->prepare("INSERT INTO email_verification (email, ip_address) VALUES (:email, :ip_address)");
        $insert->execute([':email' => $email, ':ip_address' => $user_ip]);
        echo '验证成功，您的IP已加入白名单！';
    }
}
?>
