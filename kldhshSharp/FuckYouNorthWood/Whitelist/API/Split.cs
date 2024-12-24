// -----------------------------------------------------------------------
// <copyright file="Split.cs" company="Carl Frellett">
// Copyright (c) Carl Frellett. All rights reserved.
// Licensed under the GPL-3.0 license license.
// </copyright>
// -----------------------------------------------------------------------

namespace FuckYouNorthWood.Whitelist.API
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// 用于分割IP地址以确保规整性.
    /// </summary>
    internal static class Split
    {
        /// <summary>
        /// 用于分割IP地址的方法.
        /// </summary>
        /// <param name="iPList">IP列表.</param>
        /// <returns>一个分割IP的数组方法.</returns>
        public static string[] IPList(string iPList)
        {
            if (string.IsNullOrEmpty(iPList))
            {
                return Array.Empty<string>();
            }

            iPList = Regex.Replace(iPList, "<pre>", string.Empty, RegexOptions.IgnoreCase);
            iPList = Regex.Replace(iPList, "</pre>", string.Empty, RegexOptions.IgnoreCase);

            string[] iP = iPList.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < iP.Length; i++)
            {
                iP[i] = iP[i].Trim();
            }

            return iP;
        }
    }
}
