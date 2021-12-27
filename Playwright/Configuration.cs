using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightDemo
{
    public class Configuration
    {
        static readonly string CONFIG_FILE = "appsettings.json";  // 配置文件地址

        static IConfiguration Configurationonfigs;

        /// <summary>
        /// 获取配置文件的配置节点
        /// </summary>
        /// <param name="configKey">多个节点可以用英文.号隔开</param>
        /// <returns></returns>
        public static string GetConfiguration(string configKey)
        {
            if (Configurationonfigs == null)
            {
                var build = new ConfigurationBuilder();
                build.SetBasePath(Directory.GetCurrentDirectory());  // 获取当前程序执行目录
                build.AddJsonFile(CONFIG_FILE, true, true);
                Configurationonfigs = build.Build();
            }

            if (configKey.Contains("."))
            {
                IConfigurationSection child = null;
                foreach (string key in configKey.Split('.'))
                {
                    if (child == null)
                        child = Configurationonfigs.GetSection(key);
                    else
                        child = child.GetSection(key);
                }

                return (child == null) ? "" : child.Value;
            }
            else
            {
                return Configurationonfigs.GetSection(configKey).Value;
            }
        }
    }
}
