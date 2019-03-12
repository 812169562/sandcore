using Sand.Extensions;
using System.Collections.Generic;

namespace Sand.Log.Abstractions
{
    /// <summary>
    /// 日志转换器
    /// </summary>
    public interface ILogConvert
    {
        /// <summary>
        /// 转换
        /// </summary>
        List<Item> To();
    }
}
