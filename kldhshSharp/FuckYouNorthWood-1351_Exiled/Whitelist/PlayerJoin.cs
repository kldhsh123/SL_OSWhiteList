﻿// -----------------------------------------------------------------------
// <copyright file="PlayerJoin.cs" company="Carl Frellett">
// Copyright (c) Carl Frellett. All rights reserved.
// Licensed under the GPL-3.0 license license.
// </copyright>
// -----------------------------------------------------------------------

namespace FuckYouNorthWood
{
    using System;
    using System.IO;

    using Exiled.API.Features;
    using Exiled.Events.EventArgs.Player;

    /// <summary>
    /// 玩家加入事件类.
    /// </summary>
    internal class PlayerJoin
    {
        private static string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static string iPList = Path.Combine(desktop, "IPList.txt");

        /// <summary>
        /// 玩家加入.
        /// </summary>
        /// <param name="ev">参数.</param>
        public void Join(JoinedEventArgs ev)
        {
            if (!File.Exists(iPList))
            {
                Log.Error($"无法找到'IPList.txt'IP列表本地文件，故插件将无法检查玩家的IP地址！");
                return;
            }

            ev.Player.SendConsoleMessage("执行玩家IP验证操作！", "yellow");
            Log.Info($"玩家 {ev.Player.Nickname} ({ev.Player.IPAddress}) 尝试加入服务器, 开始验证!");

            string[] iPListPlayer = File.ReadAllLines(iPList);

            string iP = ev.Player.IPAddress;

            if (iPListPlayer.Contains(iP))
            {
                ev.Player.SendConsoleMessage("验证完毕", "green");
                Log.Info($"玩家 {ev.Player.Nickname} ({ev.Player.IPAddress}) 验证完毕！");

                if (ev.Player.Nickname.Contains("Player ") == true || Core.PluginConfig.IsEnabledUnOnlinePlayerJoin == true)
                {
                    ev.Player.SendConsoleMessage("离线玩家检测验证完毕", "green");
                    Log.Info($"玩家 {ev.Player.Nickname} ({ev.Player.IPAddress}) 离线玩家检测验证完毕！");
                }
                else if (Core.PluginConfig.IsEnabledUnOnlinePlayerJoin == true)
                {
                    ev.Player.Disconnect($"\n您疑似为离线玩家！请开启Steam后再加入!");
                    Log.Info($"玩家 {ev.Player.Nickname} ({ev.Player.IPAddress}) 为离线玩家！");
                }
            }
            else
            {
                Log.Info($"玩家 {ev.Player.Nickname} ({ev.Player.IPAddress}) 无法验证其身份！");
                ev.Player.SendConsoleMessage(Core.PluginConfig.DisMessage, "red");

                ev.Player.Disconnect(Core.PluginConfig.DisMessage);
            }
        }
    }
}
