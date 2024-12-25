// -----------------------------------------------------------------------
// <copyright file="PluginConfig.cs" company="Carl Frellett">
// Copyright (c) Carl Frellett. All rights reserved.
// Licensed under the GPL-3.0 license license.
// </copyright>
// -----------------------------------------------------------------------

namespace FuckYouNorthWood
{
    using System.ComponentModel;

    /// <summary>
    /// 配置文件类.
    /// </summary>
    public sealed class PluginConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether 是否允许离线玩家加入.
        /// </summary>
        [Description("是否允许离线玩家加入？[离线玩家指未开启Steam的玩家 (关闭此选项会导致更多的作弊问题!)]")]
        public bool IsEnabledUnOnlinePlayerJoin { get; set; } = true;

        /// <summary>
        /// Gets or sets 多久更新列表.
        /// </summary>
        [Description("多久更新一次IP列表")]
        public int Time { get; set; } = 100;

        /// <summary>
        /// Gets or sets 首选的Url地址.
        /// </summary>
        [Description("首选Url")]
        public string Url { get; set; } = "https://oldserver.scpslgame.cn/api/whlistip.php?key=sdhSHJ231";

        /// <summary>
        /// Gets or sets 备选的Url地址.
        /// </summary>
        [Description("备选Url")]
        public string Url_2 { get; set; } = "http://103.119.1.71:45684/api/whlistip.php?key=sdhSHJ231";
    }
}
