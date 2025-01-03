<?php
$email = $_POST['email'] ?? $_GET['email'] ?? '';
$code = $_POST['verification_code'] ?? $_GET['code'] ?? '';
$user_ip = $_POST['hidden_user_ip'] ?? $_GET['user_ip'] ?? $_SERVER['HTTP_X_FORWARDED_FOR'] ?? $_SERVER['REMOTE_ADDR'];

$user_ip = empty($user_ip) ? 'unknown_ip' : $user_ip;

if (!preg_match('/^[0-9]+@qq\.com$/', $email)) {
    die('邮箱格式错误，仅支持 qq.com 域名且用户名为纯数字的邮箱');
}

if (!filter_var($user_ip, FILTER_VALIDATE_IP)) {
    die('IP 地址格式错误');
}

if (empty($email) || empty($code)) {
    die('缺少必要参数：请确保邮箱和验证码已填写');
}

require 'db.php';

$delete_expired = $pdo->prepare("DELETE FROM email_verifications_temp WHERE expires_at < NOW()");
$delete_expired->execute();

$stmt = $pdo->prepare("SELECT * FROM email_verifications_temp WHERE email = :email ORDER BY created_at DESC LIMIT 1");
$stmt->execute([':email' => $email]);
$record = $stmt->fetch(PDO::FETCH_ASSOC);

if (!$record || $record['code'] !== $code) {
    die('验证码错误或不存在');
}

if (strtotime($record['expires_at']) < time()) {
    die('验证码已过期');
}

$delete_used_code = $pdo->prepare("DELETE FROM email_verifications_temp WHERE email = :email AND code = :code");
$delete_used_code->execute([':email' => $email, ':code' => $code]);

if (strlen($code) == 6) {
    $update_ip = $pdo->prepare("UPDATE email_verification SET ip_address = :ip_address WHERE email = :email");
    $update_ip->execute([':email' => $email, ':ip_address' => $user_ip]);

    if ($update_ip->rowCount() == 0) {
        $insert = $pdo->prepare("INSERT INTO email_verification (email, ip_address) VALUES (:email, :ip_address)");
        $insert->execute([':email' => $email, ':ip_address' => $user_ip]);
    }

    echo '验证成功，您的IP已更新或加入白名单！';
} else {
    $check_existing = $pdo->prepare("SELECT 1 FROM email_verification WHERE email = :email LIMIT 1");
    $check_existing->execute([':email' => $email]);

    if ($check_existing->fetchColumn()) {
        echo '该邮箱已经存在验证记录，无需再次验证。';
    } else {
        $insert = $pdo->prepare("INSERT INTO email_verification (email, ip_address) VALUES (:email, :ip_address)");
        $insert->execute([':email' => $email, ':ip_address' => $user_ip]);
        echo '验证成功，您的IP已加入白名单！';
    }
}
?>
