<?php
require 'db.php';

require 'PHPMailer/src/PHPMailer.php';
require 'PHPMailer/src/SMTP.php';
require 'PHPMailer/src/Exception.php';

use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\Exception;

$email = $_POST['email'] ?? '';
$user_ip = $_POST['user_ip'] ?? $_SERVER['HTTP_X_FORWARDED_FOR'] ?? $_SERVER['REMOTE_ADDR']; 
$change_ip = isset($_POST['change_ip']) && $_POST['change_ip'] == '1';  
$turnstile_response = $_POST['cf-turnstile-response'] ?? ''; 

$turnstile_secret_key = 'Turnstile 秘钥';  // 设置成你的 Turnstile 秘钥
$turnstile_verify_url = 'https://challenges.cloudflare.com/turnstile/v0/siteverify';

$ch = curl_init();
curl_setopt($ch, CURLOPT_URL, $turnstile_verify_url);
curl_setopt($ch, CURLOPT_POST, true);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_POSTFIELDS, [
    'secret' => $turnstile_secret_key,
    'response' => $turnstile_response,
    'remoteip' => $user_ip,
]);

$response = curl_exec($ch);
curl_close($ch);

$response_data = json_decode($response, true);

if (!$response_data['success']) {
    die('Turnstile人机验证失败，请重试');
}

if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
    die('邮箱格式不正确');
}
if (!preg_match('/@qq\.com$/', $email)) {
    die('邮箱后缀必须是 @qq.com');
}

$stmt_blacklist = $pdo->prepare("SELECT * FROM blacklist WHERE email = :email OR ip_address = :ip_address LIMIT 1");
$stmt_blacklist->execute([':email' => $email, ':ip_address' => $user_ip]);
$blacklist_record = $stmt_blacklist->fetch(PDO::FETCH_ASSOC);

if ($blacklist_record) {
    die('该邮箱或 IP 地址被列入黑名单，无法进行验证');
}

$stmt_check_verified = $pdo->prepare("SELECT * FROM email_verification WHERE email = :email LIMIT 1");
$stmt_check_verified->execute([':email' => $email]);
$verified_record = $stmt_check_verified->fetch(PDO::FETCH_ASSOC);

$stmt_check = $pdo->prepare("SELECT * FROM email_verifications_temp WHERE email = :email AND expires_at > NOW() LIMIT 1");
$stmt_check->execute([':email' => $email]);
$existing_record = $stmt_check->fetch(PDO::FETCH_ASSOC);

if ($existing_record && !$change_ip) {
    die('该邮箱已请求过验证码，请稍后再试');
}

if ($change_ip) {
    $stmt_ip_changes = $pdo->prepare("SELECT COUNT(*) FROM ip_changes WHERE email = :email AND WEEK(created_at) = WEEK(NOW())");
    $stmt_ip_changes->execute([':email' => $email]);
    $ip_changes = $stmt_ip_changes->fetchColumn();

    if ($ip_changes >= 2) {
        die('本周您已经使用过两次更改 IP 的机会');
    }

    $stmt_check_email = $pdo->prepare("SELECT 1 FROM email_verification WHERE email = :email LIMIT 1");
    $stmt_check_email->execute([':email' => $email]);

    if (!$stmt_check_email->fetchColumn()) {
        die('无法更改 IP，因为该邮箱尚未完成验证');
    }

    $stmt_get_verified_ip = $pdo->prepare("SELECT ip_address FROM email_verification WHERE email = :email LIMIT 1");
    $stmt_get_verified_ip->execute([':email' => $email]);
    $verified_ip_record = $stmt_get_verified_ip->fetch(PDO::FETCH_ASSOC);
    $old_ip = $verified_ip_record['ip_address'] ?? '';  

    $stmt_insert_ip = $pdo->prepare("INSERT INTO ip_changes (email, old_ip, new_ip) VALUES (:email, :old_ip, :new_ip)");
    $stmt_insert_ip->execute([ 
        ':email' => $email,
        ':old_ip' => $old_ip,
        ':new_ip' => $user_ip
    ]);
}

if ($change_ip) {
    $code = rand(100000, 999999); 
} else {
    $code = rand(10000, 99999);
}

$expires_at = date('Y-m-d H:i:s', time() + 300);

$stmt = $pdo->prepare("INSERT INTO email_verifications_temp (email, code, expires_at, ip_address) 
                        VALUES (:email, :code, :expires_at, :ip_address)");
$stmt->execute([
    ':email' => $email,
    ':code' => $code,
    ':expires_at' => $expires_at,
    ':ip_address' => $user_ip
]);

//这边替换成你站点的域名
$verification_link = "https://我是域名/verify.php?email=" . urlencode($email) . "&code=" . urlencode($code) . "&user_ip=" . urlencode($user_ip);

$mail = new PHPMailer();
$mail->CharSet = 'UTF-8';  
$mail->isSMTP();  // 使用 SMTP
$mail->Host = 'smtp.qiye.aliyun.com';  // SMTP服务器地址
$mail->SMTPAuth = true;  // 启用SMTP认证
$mail->Username = '111@qq.com';  // 发件人邮箱地址
$mail->Password = '111@qq.com.';  // 发件人邮箱授权码
$mail->SMTPSecure = PHPMailer::ENCRYPTION_SMTPS;  // 使用SSL加密
$mail->Port = 465;  // SSL 端口

$mail->setFrom('111@qq.com', '验证系统'); // 发件人邮箱和名称
$mail->addAddress($email); 

$mail->isHTML(true);  
$mail->Subject = 'SCP秘密实验室怀旧服白名单IP验证';
$mail->Body    = "请点击以下链接完成验证：<a href='{$verification_link}'>点击这里完成验证</a>。请在5分钟内完成验证。";

if(!$mail->send()) {
    die('邮件发送失败: ' . $mail->ErrorInfo);
}

echo '验证链接已发送，请查收您的邮箱。';
?>
