// -----------------------------------------------------------------------
// <copyright file="Core.cs" company="Carl Frellett">
// Copyright (c) Carl Frellett. All rights reserved.
// Licensed under the GPL-3.0 license license.
// </copyright>
// -----------------------------------------------------------------------

namespace FuckYouNorthWood
{
    using EXILED;
    using FuckYouNorthWood.Whitelist;

#pragma warning disable SA1201
#pragma warning disable SA1401 // Fields should be private
    /// <summary>
    /// 插件主类, 用于加载插件.
    /// </summary>
    public class Core : Plugin
    {
        /// <inheritdoc/>
        public override string getName => "FuckYouNorthWood";

        /// <summary>
        /// 关闭.
        /// </summary>
        public override void OnDisable()
        {
            PlayerJoin joinEvent = null;
            EXILED.Events.PlayerJoinEvent -= joinEvent.Join;
        }

        /// <summary>
        /// 开启.
        /// </summary>
        public override void OnEnable()
        {
            GetIPList.FetchIpList();
            Log.Info("FuckYou140scpsl - 旧版白名单插件 [已加载]");
            Log.Info("FuckYou140scpsl - 旧版白名单更新系统 [已加载]");

            PlayerJoin joinEvent = new PlayerJoin();
            EXILED.Events.PlayerJoinEvent += joinEvent.Join;
        }

        /// <summary>
        /// 重载.
        /// </summary>
        public override void OnReload()
        {
        }
    }
#pragma warning restore SA1201
#pragma warning restore SA1401 // Fields should be private
}
