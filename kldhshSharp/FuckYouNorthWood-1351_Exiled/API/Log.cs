// -----------------------------------------------------------------------
// <copyright file="Log.cs" company="Carl Frellett">
// Copyright (c) Carl Frellett. All rights reserved.
// Licensed under the GPL-3.0 license license.
// </copyright>
// -----------------------------------------------------------------------

namespace FuckYouNorthWood.API
{
    /// <summary>
    /// 用于FuckYouNorthWood的Log类.
    /// </summary>
    internal static class Log
    {
        /// <summary>
        /// 用于插件内部的日志.
        /// </summary>
        /// <param name="text">文本.</param>
        internal static void AddLog(string text)
        {
            if (Core.PluginConfig.IsEnableUpdataIPListLog == true)
            {
                Exiled.API.Features.Log.Info(text);
            }
        }
    }
}
