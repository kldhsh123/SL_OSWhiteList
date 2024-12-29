// -----------------------------------------------------------------------
// <copyright file="Configs.cs" company="Carl Frellett">
// Copyright (c) Carl Frellett.
// </copyright>
// -----------------------------------------------------------------------

namespace ConfigurationManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 抽象的配置.
    /// </summary>
    public abstract class Configs
    {
        /// <summary>
        /// Gets 配置文件的路径.
        /// </summary>
        public abstract string ConfigFilePath { get; }

        /// <summary>
        /// 添加一个配置.
        /// </summary>
        /// <param name="configName">配置名.</param>
        /// <param name="defaultValue">默认值.</param>
        /// <param name="configType">类型.</param>
        /// <param name="description">描述.</param>
        public void AddConfig(string configName, object defaultValue, Type configType, string description)
        {
            if (!File.Exists(this.ConfigFilePath))
            {
                File.WriteAllText(this.ConfigFilePath, string.Empty);
            }

            string[] lines = File.ReadAllLines(this.ConfigFilePath);
            bool configExists = false;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith($"{configName}:"))
                {
                    configExists = true;
                    break;
                }
            }

            if (configExists)
            {
                Console.WriteLine($"Config '{configName}' already exists in the file.");
                return;
            }

            using (StreamWriter writer = File.AppendText(this.ConfigFilePath))
            {
                writer.WriteLine($"# {description}");
                writer.WriteLine($"{configName}: {defaultValue}");
            }
        }

        /// <summary>
        /// 获取Bool类型的配置.
        /// </summary>
        /// <param name="configName">配置名.</param>
        /// <returns>返回false 或 true.</returns>
        public bool GetBoolConfig(string configName)
        {
            string value = this.GetConfigValue(configName);
            if (bool.TryParse(value, out bool result))
            {
                return result;
            }

            throw new InvalidCastException($"The configuration '{configName}' cannot be converted to a boolean.");
        }

        /// <summary>
        /// 获取数字配置.
        /// </summary>
        /// <param name="configName">配置名称.</param>
        /// <returns>返回个数字.</returns>
        public int GetIntConfig(string configName)
        {
            string value = this.GetConfigValue(configName);
            if (int.TryParse(value, out int result))
            {
                return result;
            }

            throw new InvalidCastException($"The configuration '{configName}' cannot be converted to an integer.");
        }

        /// <summary>
        /// 获取文本的配置.
        /// </summary>
        /// <param name="configName">配置名称.</param>
        /// <returns>返回串文本.</returns>
        public string GetStringConfig(string configName)
        {
            return this.GetConfigValue(configName);
        }

        /// <summary>
        /// 内部用于获取": "后一行的内容的方法.
        /// </summary>
        /// <param name="configName">配置名称.</param>
        /// <returns>返回配置值.</returns>
        private string GetConfigValue(string configName)
        {
            if (!File.Exists(this.ConfigFilePath))
            {
                throw new FileNotFoundException($"Configuration file '{this.ConfigFilePath}' does not exist.");
            }

            string[] lines = File.ReadAllLines(this.ConfigFilePath);
            foreach (string line in lines)
            {
                if (line.StartsWith("#"))
                {
                    continue;
                }

                if (line.StartsWith($"{configName}:"))
                {
                    int colonIndex = line.IndexOf(":");
                    if (colonIndex != -1)
                    {
                        return line.Substring(colonIndex + 2).Trim();
                    }
                }
            }

            throw new KeyNotFoundException($"The configuration '{configName}' was not found in the file.");
        }
    }
}
