// -----------------------------------------------------------------------
// <copyright file="Core.cs" company="Carl Frellett">
// Copyright (c) Carl Frellett. All rights reserved.
// Licensed under the GPL-3.0 license license.
// </copyright>
// -----------------------------------------------------------------------

namespace FuckYouNorthWood
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using FuckYouNorthWood.Whitelist;

#pragma warning disable SA1201
#pragma warning disable SA1401
    /// <summary>
    /// 插件主类, 用于加载插件.
    /// </summary>
    public class Core : Plugin<PluginConfig>
    {
        /// <inheritdoc/>
        public override string Author => "Carl Frellett";

        /// <inheritdoc/>
        public override string Name => "FuckYouNorthWood";

        /// <inheritdoc/>
        public override PluginPriority Priority => PluginPriority.High;

        /// <summary>
        /// 静态接口Config.
        /// </summary>
        internal static PluginConfig PluginConfig = new PluginConfig();

        /// <summary>
        /// 当插件启动时.
        /// </summary>
        public override void OnEnabled()
        {
            base.OnEnabled();

            GetIPList.FetchIpList();
            Log.Info("FuckYou140scpsl - 旧版白名单插件 [已加载]");
            Log.Info("FuckYou140scpsl - 旧版白名单更新系统 [已加载]");

            PlayerJoin joinEvent = new PlayerJoin();
            Exiled.Events.Handlers.Player.Joined += joinEvent.Join;
        }

        /// <summary>
        /// 当插件关闭时.
        /// </summary>
        public override void OnDisabled()
        {
            base.OnDisabled();
            Log.Info("FuckYou140scpsl - 旧版白名单插件 [已卸载]");

            PlayerJoin joinEvent = null;
            Exiled.Events.Handlers.Player.Joined -= joinEvent.Join;
        }
    }
#pragma warning restore SA1201
#pragma warning restore SA1401
}
