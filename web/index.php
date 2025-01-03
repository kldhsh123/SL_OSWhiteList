<?php
function getClientIP() {
    if (!empty($_SERVER['HTTP_X_FORWARDED_FOR'])) {
        $ip = explode(',', $_SERVER['HTTP_X_FORWARDED_FOR'])[0];
    } elseif (!empty($_SERVER['HTTP_CLIENT_IP'])) {
        $ip = $_SERVER['HTTP_CLIENT_IP'];
    } else {
        $ip = $_SERVER['REMOTE_ADDR'];
    }
    return $ip;
}

$user_ip = getClientIP();
?>
<!DOCTYPE html>
<html lang="zh">
<head>
    <!-- 
    版权所有 © by:开朗的火山河123
    二开程序可以添加内容，但是禁止去除本声明。
    未经明确许可，任何个人或实体不得以任何形式商业利用此程序。
    github开源地址:https://github.com/kldhsh123/SL_OSWhiteList
    -->
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>SCP秘密实验室怀旧服白名单IP验证</title>
    <script src="https://challenges.cloudflare.com/turnstile/v0/api.js" async defer></script>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;700&display=swap" rel="stylesheet">
    <style>
        :root {
            --primary-color: #007bff;
            --secondary-color: #6c757d;
            --success-color: #28a745;
            --danger-color: #dc3545;
            --warning-color: #ffc107;
            --info-color: #17a2b8;
            --light-color: #f8f9fa;
            --dark-color: #343a40;
        }

        body {
            font-family: 'Roboto', Arial, sans-serif;
            padding: 20px;
            background-color: #f0f2f5;
            color: var(--dark-color);
            line-height: 1.6;
        }

        .container {
            max-width: 600px;
            margin: 40px auto;
            background-color: #fff;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }

        h1 {
            text-align: center;
            color: var(--primary-color);
            margin-bottom: 30px;
        }

        .announcement {
            background-color: #fff3cd;
            padding: 15px;
            margin-bottom: 25px;
            border-radius: 8px;
            color: #856404;
            border: 1px solid #ffeeba;
            font-size: 0.95em;
        }

        .form-group {
            margin-bottom: 20px;
        }

        label {
            font-weight: bold;
            display: block;
            margin-bottom: 5px;
            color: var(--secondary-color);
        }

        input[type="email"],
        input[type="text"] {
            width: 100%;
            padding: 12px;
            margin-top: 5px;
            border: 1px solid #ced4da;
            border-radius: 6px;
            font-size: 16px;
            transition: border-color 0.3s ease;
        }

        input[type="email"]:focus,
        input[type="text"]:focus {
            outline: none;
            border-color: var(--primary-color);
            box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
        }

        .checkbox-group {
            display: flex;
            align-items: center;
            margin-bottom: 20px;
        }

        .checkbox-group input[type="checkbox"] {
            margin-right: 10px;
        }

        button {
            background-color: var(--primary-color);
            color: white;
            padding: 12px 24px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-size: 16px;
            font-weight: bold;
            transition: background-color 0.3s ease;
            width: 100%;
        }

        button:hover {
            background-color: #0056b3;
        }

        .cf-turnstile {
            margin-bottom: 20px;
        }
    </style>
</head>
<body>
    <div class="container">
        <h1>SCP秘密实验室怀旧服白名单IP验证</h1>

        <div class="announcement">
            <strong>注意：</strong> 请确保您输入的邮箱是 <strong>纯数字</strong> 并且是 <strong>@qq.com</strong> 的QQ邮箱。
            <br>每周仅有三次修改IP机会，如果遇到问题，请添加 <strong>QQ1022140881</strong> 咨询
            <br>如果您使用了代理，请手动输入ip来防止自动获取到您代理的ip地址
        </div>

        <form action="send_email.php" method="POST" id="send_form">
            <div class="form-group">
                <label for="email">邮箱:</label>
                <input type="email" name="email" id="email" required placeholder="请输入您的邮箱 (@qq.com)">
            </div>

            <div class="form-group">
                <label for="user_ip">IP 地址（为空则自动获取）：</label>
                <input type="text" name="user_ip" id="user_ip" placeholder="请输入您的 IP 地址 (为空则自动获取)">
            </div>
            
            <div class="checkbox-group">
                <input type="checkbox" name="change_ip" id="change_ip" value="1">
                <label for="change_ip">更改已记录的 IP 地址<span style="color: red;">(请确保ip地址已验证再勾选)</span></label>
            </div>

            <div class="cf-turnstile" data-sitekey="0x4AAAAAAA3zzVEtQg1g9vfV"></div>
            
            <button type="submit" name="action" value="send_code">发送验证码</button>
        </form>
    </div>

    <script>
        document.getElementById("send_form").onsubmit = function(event) {
            var email = document.getElementById("email").value;
            var user_ip = document.getElementById("user_ip").value;

            var emailRegex = /^[0-9]+@qq\.com$/;
            if (!emailRegex.test(email)) {
                alert('请输入正确的QQ邮箱，且用户名必须为纯数字');
                event.preventDefault(); 
                return;
            }

            // 验证IP地址格式
            if (user_ip && !/^(\d{1,3}\.){3}\d{1,3}$/.test(user_ip)) {
                alert('请输入有效的IP地址');
                event.preventDefault();  
                return;
            }

            if (!user_ip) {
                document.getElementById("user_ip").value = "<?php echo $user_ip; ?>";
            }

            var hiddenEmail = document.createElement("input");
            hiddenEmail.type = "hidden";
            hiddenEmail.name = "hidden_email";
            hiddenEmail.value = email;
            this.appendChild(hiddenEmail);

            var hiddenUserIp = document.createElement("input");
            hiddenUserIp.type = "hidden";
            hiddenUserIp.name = "hidden_user_ip";
            hiddenUserIp.value = document.getElementById("user_ip").value;
            this.appendChild(hiddenUserIp);
        };
    </script>
</body>
</html>
