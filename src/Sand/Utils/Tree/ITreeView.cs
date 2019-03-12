using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Utils.Tree
{
    /// <summary>
    /// 树形接口
    /// </summary>
    public interface ITreeView<TITreeView> where TITreeView : ITreeView<TITreeView>
    {
        /// <summary>
        /// 节点编号
        /// </summary>
        string Value { get; set; }
        /// <summary>
        /// 节点编号
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// 节点标签
        /// </summary>
        string Label { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>
        string ParentId { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        string Code { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        string Status { get; set; }
        /// <summary>
        /// 是否为子节点
        /// </summary>
        bool IsLeaf { get; set; }
        /// <summary>
        /// 层数
        /// </summary>
        int Level { get; }
        /// <summary>
        /// 是否可用
        /// </summary>
        bool Disabled { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        string Remark { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        bool Selected { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        List<TITreeView> Children { get; set; }
    }
}
