// -----------------------------------------------------------------------
// <copyright file="GetIPList.cs" company="Carl Frellett">
// Copyright (c) Carl Frellett. All rights reserved.
// Licensed under the GPL-3.0 license license.
// </copyright>
// -----------------------------------------------------------------------

namespace FuckYouNorthWood.Whitelist
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using FuckYouNorthWood.Whitelist.API;
    using MEC;
    using Smod2.Logging;

    /// <summary>
    /// 用于通过接入Url来获取IP地址的类.
    /// </summary>
    internal static class GetIPList
    {
        private const string A = "https://oldserver.scpslgame.cn/api/whlistip.php?key=sdhSHJ231";
        private const string B = "http://103.119.1.71:45684/api/whlistip.php?key=sdhSHJ231";
        private static Logger logger;
        private static Core core;

        /// <summary>
        /// 开始定期更新IP列表.
        /// </summary>
        internal static void FetchIpList()
        {
            Timing.RunCoroutine(UpdateIpList());
        }

        /// <summary>
        /// 更新 IP 列表的方法.
        /// </summary>
        /// <returns>无参数.</returns>
        private static IEnumerator<float> UpdateIpList()
        {
            while (true)
            {
                try
                {
                    logger.Info(string.Empty, "正在尝试从首选 URL 获取 IP 列表...");

                    string a_Url = core.GetConfigString("Url");
                    if (string.IsNullOrEmpty(a_Url))
                    {
                        a_Url = A;
                        logger.Warn(string.Empty, "无法读取配置文件内的IP列表服务器地址！使用默认Url");
                    }

                    using (var client = new WebClient())
                    {
                        string clientiPList = client.DownloadString(a_Url);
                        string[] iPList = Split.IPList(clientiPList);

                        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string file = Path.Combine(desktopPath, "IPList.txt");
                        File.WriteAllLines(file, iPList);

                        logger.Info(string.Empty, $"IP 列表已成功获取并保存至：{file}");
                    }
                }
                catch (WebException ex)
                {
                    logger.Error(string.Empty, $"从首选 URL 获取 IP 列表时出现错误：{ex.Message}，现在尝试使用备用 URL...");
                    try
                    {
                        string b_Url = core.GetConfigString("Url_2");
                        if (string.IsNullOrEmpty(b_Url))
                        {
                            b_Url = B;
                            logger.Warn(string.Empty, "无法读取配置文件内的IP列表服务器地址且默认主Url无法连接！使用默认备选Url");
                        }

                        using (var client = new WebClient())
                        {
                            string clientiPList = client.DownloadString(b_Url);

                            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            string file = Path.Combine(desktopPath, "IPList.txt");
                            File.WriteAllText(file, clientiPList);

                            logger.Info(string.Empty, $"IP 列表已成功获取（备用 URL）并保存至：{file}");
                        }
                    }
                    catch (WebException log_2)
                    {
                        logger.Error(string.Empty, $"从备用 URL 获取 IP 列表时出现网络错误：{log_2.Message}");
                    }
                    catch (Exception log_2)
                    {
                        logger.Error(string.Empty, $"从备用 URL 获取 IP 列表时出现错误：{log_2.Message}");
                    }
                }
                catch (Exception log)
                {
                    logger.Error(string.Empty, $"获取 IP 列表时出现错误：{log.Message}");
                }

                yield return Timing.WaitForSeconds(core.GetConfigInt("Time"));
            }
        }
    }
}
