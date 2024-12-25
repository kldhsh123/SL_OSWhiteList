// -----------------------------------------------------------------------
// <copyright file="Core.cs" company="Carl Frellett">
// Copyright (c) Carl Frellett. All rights reserved.
// Licensed under the GPL-3.0 license license.
// </copyright>
// -----------------------------------------------------------------------

namespace FuckYouNorthWood
{
    using FuckYouNorthWood.Whitelist;
    using Smod2;
    using Smod2.Attributes;
    using Smod2.Config;
    using Smod2.EventHandlers;

#pragma warning disable SA1401 // Fields should be private
    /// <summary>
    /// 插件主类, 用于加载插件.
    /// </summary>
    [PluginDetails(author = "Carl Frellett", name = "FuckYouNorthWood", description = "怀旧服白名单验证插件", id = "FuckYouNorthWood", version = "1.0", SmodMajor = 3, SmodMinor = 7, SmodRevision = 0)]
    public class Core : Plugin
    {
        /// <summary>
        /// 启动插件时.
        /// </summary>
        public override void OnEnable()
        {
            GetIPList.FetchIpList();
            this.Info("FuckYou140scpsl - 旧版白名单插件 [已加载]");
            this.Info("FuckYou140scpsl - 旧版白名单更新系统 [已加载]");

            bool isEnabledUnOnlinePlayerJoin = this.GetConfigBool("IsEnabledUnOnlinePlayerJoin");
            int time = this.GetConfigInt("Time");
            string url = this.GetConfigString("Url");
            string url_2 = this.GetConfigString("Url_2");
        }

        /// <summary>
        /// 注册事件.
        /// </summary>
        public override void Register()
        {
            this.AddConfig(new ConfigSetting("IsEnabledUnOnlinePlayerJoin", true, true, "是否允许离线玩家加入？[离线玩家指未开启Steam的玩家 (关闭此选项会导致更多的作弊问题!)]"));
            this.AddConfig(new ConfigSetting("Time", 100, true, "多久更新一次列表"));
            this.AddConfig(new ConfigSetting("Url", "https://oldserver.scpslgame.cn/api/whlistip.php?key=sdhSHJ231", true, "首选Url地址"));
            this.AddConfig(new ConfigSetting("Url_2", "http://103.119.1.71:45684/api/whlistip.php?key=sdhSHJ231", true, "备选Url地址"));

            this.AddEventHandler(typeof(IEventHandlerPlayerJoin), new PlayerJoin(this));
        }

        /// <summary>
        /// 当卸载插件时.
        /// </summary>
        public override void OnDisable()
        {
            this.Info("插件关闭啦");
        }
    }
#pragma warning restore SA1401 // Fields should be private
}
