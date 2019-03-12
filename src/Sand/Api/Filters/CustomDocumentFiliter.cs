using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Sand.Api.Filters
{
    /// <summary>
    /// 添加控制器解释
    /// </summary>
    public class CustomDocumentFiliter : IDocumentFilter
    {
        /// <summary>
        /// 添加控制器解释
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            IList<Tag> tags = new List<Tag>();
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            string _xmlPath = Path.Combine(Path.Combine(basePath, "Sand.Api.xml"));
            ConcurrentDictionary<string, string> _controllerDescDict = new ConcurrentDictionary<string, string>();
            if (File.Exists(_xmlPath))
            {
                XmlDocument _xmlDoc = new XmlDocument();
                _xmlDoc.Load(_xmlPath);
                string _type = string.Empty, _path = string.Empty, _controllerName = string.Empty;
                XmlNode _summaryNode = null;

                foreach (XmlNode _node in _xmlDoc.SelectNodes("//member"))
                {
                    _type = _node.Attributes["name"].Value;

                    if (_type.StartsWith("T:"))
                    {
                        _summaryNode = _node.SelectSingleNode("summary");
                        string[] _names = _type.Split('.');
                        string _key = _names[_names.Length - 1];
                        if (_key.IndexOf("Controller") > -1&&_key!= "BaseApiController")
                        {
                            _key = _key.Substring(0, _key.Length - "Controller".Length);
                            tags.Add(new Tag() { Name = _key, Description = _summaryNode.InnerText });
                        }
                    }
                }
                swaggerDoc.Tags = tags;
            }
        }
    }
}
