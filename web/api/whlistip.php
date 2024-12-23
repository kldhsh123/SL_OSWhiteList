<?php
$key = $_GET['key'] ?? '';

if ($key !== 'key') {
    die('无效的请求。');
}

require '../db.php';

$stmt = $pdo->query("SELECT ip_address, email FROM email_verification");
$email_verifications = $stmt->fetchAll(PDO::FETCH_ASSOC);

$stmt = $pdo->query("SELECT ip_address, email FROM blacklist");
$blacklist = $stmt->fetchAll(PDO::FETCH_ASSOC);

$blacklist_ips = [];
$blacklist_emails = [];

foreach ($blacklist as $blacklisted_record) {
    if ($blacklisted_record['ip_address']) {
        $blacklist_ips[] = $blacklisted_record['ip_address'];
    }
    if ($blacklisted_record['email']) {
        $blacklist_emails[] = $blacklisted_record['email'];
    }
}

$filtered_ips = [];
foreach ($email_verifications as $record) {
    if (in_array($record['ip_address'], $blacklist_ips) || in_array($record['email'], $blacklist_emails)) {
        continue;
    }

    $filtered_ips[] = $record['ip_address'];
}

if ($filtered_ips) {
    echo '<pre>';
    foreach ($filtered_ips as $ip) {
        echo $ip . "<br>";
    }
    echo '</pre>';
} else {
    echo '';
}
?>
