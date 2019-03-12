using Sand.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sand.Utils.Tree
{
    /// <summary>
    /// 树形数据
    /// </summary>
    [DataContract(Name = "source")]
    public class TreeView : ITreeView<TreeView>
    {
        /// <summary>
        /// 节点编号
        /// </summary>
        [DataMember]
        public virtual string Value { get; set; }
        /// <summary>
        /// 节点编号
        /// </summary>
        [DataMember]
        public virtual string Id { get; set; }
        /// <summary>
        /// 节点标签
        /// </summary>
        [DataMember]
        public virtual string Label { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>
        [DataMember]
        public virtual string ParentId { get; set; }
        /// <summary>
        /// 关系编号
        /// </summary>
        [DataMember]
        public virtual string TreeCode { get; set; }
        /// <summary>
        /// 节点
        /// </summary>
        protected bool _isLeaf;
        /// <summary>
        /// 是否为叶子节点
        /// </summary>
        [DataMember]
        public virtual bool IsLeaf
        {
            get { return _isLeaf = !(Children == null || Children.Count() == 0); }
            set { _isLeaf = value; }
        }
        /// <summary>
        /// 子节点
        /// </summary>
        [DataMember]
        public virtual List<TreeView> Children { get; set; }
        /// <summary>
        /// 层数
        /// </summary>
        [DataMember]
        public virtual int Level { get { if (TreeCode.IsEmpty() || TreeCode.Length == 0) return 1; return TreeCode.Length / 36; } }
        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        public virtual string Status { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public virtual string Code { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public virtual string Remark { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        [DataMember]
        public virtual bool Disabled { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        [DataMember]
        public virtual string Version { get; set; }
        /// <summary>
        /// 打印名称
        /// </summary>
        [DataMember]
        public virtual string PrintName { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        [DataMember]
        public bool Selected { get; set; }
        /// <summary>
        /// 全部选中的节点,只有第一个几点才存在此值
        /// </summary>
        [DataMember]
        public string [] Selecteds { get; set; }
    }

    /// <summary>
    /// vueTableTree 专用
    /// </summary>
    public class VueTableTreeView : TreeView, ITreeView<VueTableTreeView>
    {
        /// <summary>
        /// 节点编号
        /// </summary>
        public override string Value { get; set; }
        /// <summary>
        /// 节点编号
        /// </summary>
        [DataMember(Name = "id")]
        public override string Id { get; set; }
        /// <summary>
        /// 节点标签
        /// </summary>
        [DataMember(Name = "label")]
        public override string Label { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>
        [DataMember(Name = "parent_id")]
        public override string ParentId { get; set; }
        /// <summary>
        /// 关系编号
        /// </summary>
        [DataMember]
        public override string TreeCode { get; set; }
        /// <summary>
        /// 是否为叶子节点
        /// </summary>
        [DataMember]
        public override bool IsLeaf
        {
            get { return _isLeaf = !(Children == null || Children.Count() == 0); }
            set { _isLeaf = value; }
        }
        /// <summary>
        /// 子节点
        /// </summary>
        [DataMember]
        public new List<VueTableTreeView> Children { get; set; }
        /// <summary>
        /// 子节点条数
        /// </summary>
        [DataMember(Name = "child_num")]
        public int ChildrenCount { get { return Children == null ? 0 : Children.Count(); } }
        /// <summary>
        /// 层数
        /// </summary>
        [DataMember(Name = "depth")]
        public override int Level { get { if (TreeCode.IsEmpty() || TreeCode.Length == 0) return 0; return TreeCode.Length / 36; } }
        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        public override string Status { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public override string Code { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember(Name = "description")]
        public override string Remark { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        [DataMember]
        public override bool Disabled { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        [DataMember]
        public bool IsEnable { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        [DataMember]
        public override string Version { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        [DataMember]
        public int Sort { get; set; }
        /// <summary>
        /// 打印名称
        /// </summary>
        [DataMember]
        public override string PrintName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        [DataMember]
        public string Para { get; set; }
    }
}
