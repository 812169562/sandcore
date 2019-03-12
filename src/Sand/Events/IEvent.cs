using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Events
{/// <summary>
/// 事件接口
/// </summary>
    public interface IEvent
    {/// <summary>
     /// 事件编号
     /// </summary>
        string Id { get; }
        /// <summary>
        /// 事件发生时间
        /// </summary>
        DateTime Time { get; }
    }
}
