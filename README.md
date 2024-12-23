# SCP 秘密实验室白名单验证插件自述文件

## 插件简介

这是一个用于 SCP 秘密实验室怀旧服白名单验证的插件。该插件提供了一个简单的 IP 验证系统。管理员可以通过此插件验证用户的邮箱和IP地址，以确保只有符合条件的玩家可以访问服务器。

## 部署教程

### 插件部署

1. **插件文件放置**
   - 将插件文件放置在 **EXILED** 的插件目录下。

2. **重启服务器**
   - 重启 SCP 秘密实验室服务器，插件将在首次启动时生成必要的配置文件。

3. **修改请求 URL**
   - 找到插件生成的配置文件，修改其中的请求 URL，以指向您的站点。

4. **加入怀旧服联盟**
   - 如果您希望加入怀旧服联盟并获取白名单 URL，请通过 QQ 联系 **1022140881**。

### 网站部署

1. **环境要求**
   - 您的服务器需要具备以下环境：
     - **MySQL**：用于存储白名单数据。
     - **Nginx**：作为 Web 服务器。
     - **PHP 7.4+**：用于运行网站后台。

2. **导入数据库**
   - 在您的 MySQL 数据库中导入插件目录中的 **数据表.sql** 文件。该文件包含了插件所需的数据库结构。

3. **配置数据库连接**
   - 修改 **db.php** 文件中的数据库连接信息：
     - 设置您的 MySQL 主机、用户名、密码以及数据库名称。

4. **配置发件人信息**
   - 修改 **send_email.php** 文件中从第 111 行开始的站点地址和发件人信息：
     - 设置站点的基本 URL。
     - 配置邮件发送者的信息，包括发件邮箱和SMTP服务配置。

5. **修改 API 请求秘钥**
   - 修改 **api/whlistip.php** 文件中的请求秘钥：
     - 默认为 `key`，您需要将其替换为您自己的秘钥，以确保安全性。

### 使用说明

- **邮箱格式要求**：用户必须输入一个 QQ 邮箱，且邮箱用户名必须是纯数字（例如：123456789@qq.com）。
- **IP 地址验证**：用户可以选择自动获取其 IP 地址，或者手动输入一个 IP 地址。如果输入了 IP 地址，系统将验证其有效性。
- **修改 IP 地址**：每个用户每周最多修改两次 IP 地址。如果用户需要修改其 IP 地址，可以勾选相应的选项。
- **验证码**：系统使用 Cloudflare Turnstile 验证来防止恶意请求。

### 联系方式

如果您遇到任何问题或需要帮助，欢迎添加 **QQ1022140881** 咨询。
