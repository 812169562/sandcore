using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using RestSharp.Extensions;
using Sand.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.Extensions.Configuration
{

    /// <summary>
    /// 加载加密配置文件
    /// </summary>
    public static class ConfigurationExtention
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="path"></param>
        /// <param name="optional"></param>
        /// <param name="reloadOnChange"></param>
        /// <param name="asckey"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IConfigurationBuilder AddRootJsonFile(this IConfigurationBuilder builder, string path, bool optional,
            bool reloadOnChange, string asckey)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("File path must be a non-empty string.");
            }

            var source = new JsonRootConfigurationSource
            {
                FileProvider = null,
                Path = path,
                Optional = optional,
                ReloadOnChange = reloadOnChange,
                Key = asckey
            };
            source.ResolveFileProvider();
            builder.Add(source);
            return builder;
        }
    }

    /// <summary>
    /// 加载加密的配置文件
    /// </summary>
    public class JsonRootConfigurationProvider : JsonConfigurationProvider
    {
        private readonly JsonRootConfigurationSource _source;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public JsonRootConfigurationProvider(JsonRootConfigurationSource source) : base(source)
        {
            _source = source;
        }

        public override void Load(Stream stream)
        {
            try
            {
                stream.Close();
                stream.Dispose();
                using (var newStream2 = new StreamReader(_source.FileProvider.GetFileInfo(_source.Path).PhysicalPath))
                {
                    var jsonConfig = Encoding.UTF8.GetString(newStream2.BaseStream.ReadAsBytes());
                    using (var fsstream = new MemoryStream(Encoding.UTF8.GetBytes(jsonConfig.ToAesDecrypt(_source.Key))))
                    {
                        base.Load(fsstream);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class JsonRootConfigurationSource : JsonConfigurationSource
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new JsonRootConfigurationProvider(this);
        }
    }
}
