// -----------------------------------------------------------------------
// <copyright file="PlayerJoin.cs" company="Carl Frellett">
// Copyright (c) Carl Frellett. All rights reserved.
// Licensed under the GPL-3.0 license license.
// </copyright>
// -----------------------------------------------------------------------

namespace FuckYouNorthWood
{
    using System;
    using System.IO;
    using System.Linq;
    using Smod2.EventHandlers;
    using Smod2.Events;
    using Smod2.Logging;

    /// <summary>
    /// 玩家加入事件类.
    /// </summary>
#pragma warning disable SA1306 // Field names should begin with lower-case letter
    internal class PlayerJoin : IEventHandlerPlayerJoin
    {
        private static string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static string iPList = Path.Combine(desktop, "IPList.txt");
        private static Logger Log;
        private Core core;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerJoin"/> class.
        /// 我也不知道为啥有这个.
        /// </summary>
        /// <param name="cORE">参数.</param>
        public PlayerJoin(Core cORE)
        {
            this.core = cORE;
        }

        /// <summary>
        /// 玩家加入.
        /// </summary>
        /// <param name="ev">参数.</param>
        public void OnPlayerJoin(PlayerJoinEvent ev)
        {
            if (!File.Exists(iPList))
            {
                Log.Error(string.Empty, $"无法找到'IPList.txt'IP列表本地文件，故插件将无法检查玩家的IP地址！");
                return;
            }

            ev.Player.SendConsoleMessage("执行玩家IP验证操作！", "yellow");
            Log.Info(string.Empty, $"玩家 {ev.Player.Name} ({ev.Player.IpAddress}) 尝试加入服务器, 开始验证!");

            string[] iPListPlayer = File.ReadAllLines(iPList);

            string iP = ev.Player.IpAddress;

            if (iPListPlayer.Contains(iP))
            {
                ev.Player.SendConsoleMessage("验证完毕", "green");
                Log.Info(string.Empty, $"玩家 {ev.Player.Name} ({ev.Player.IpAddress}) 验证完毕！");

                if (ev.Player.Name.Contains("Player ") == true || this.core.GetConfigBool("IsEnabledUnOnlinePlayerJoin") == true)
                {
                    ev.Player.SendConsoleMessage("离线玩家检测验证完毕", "green");
                    Log.Info(string.Empty, $"玩家 {ev.Player.Name} ({ev.Player.IpAddress}) 离线玩家检测验证完毕！");
                }
                else if (this.core.GetConfigBool("IsEnabledUnOnlinePlayerJoin") == true)
                {
                    ev.Player.Disconnect($"\n您疑似为离线玩家！请开启Steam后再加入!");
                    Log.Info(string.Empty, $"玩家 {ev.Player.Name} ({ev.Player.IpAddress}) 为离线玩家！");
                }
            }
            else
            {
                Log.Info(string.Empty, $"玩家 {ev.Player.Name} ({ev.Player.IpAddress}) 无法验证其身份！");
                ev.Player.SendConsoleMessage("无法验证您的身份！请前往指定网站验证您的身份！", "red");

                ev.Player.Disconnect("\n无法验证您的身份！请前往指定网站验证您的身份！");
            }
        }
    }
#pragma warning restore SA1306 // Field names should begin with lower-case letter
}
