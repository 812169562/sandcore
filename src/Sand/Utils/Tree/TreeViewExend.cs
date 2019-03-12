using Sand.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sand.Utils.Tree
{
    /// <summary>
    /// 树形扩展
    /// </summary>
    public static class TreeViewExend
    {
        /// <summary>
        /// 生成树形数据
        /// </summary>
        /// <param name="tree">非树形的集合（获取数据组成该格式）</param>
        /// <param name="parentid">返回节点树数据</param>
        /// <param name="firstnode">第一个节点名称（空代表不填）</param>
        /// <returns></returns>
        public static List<TTreeView> ToTree<TTreeView>(this List<TTreeView> tree, string parentid = "", string firstnode = "请选择") where TTreeView : ITreeView<TTreeView>, new()
        {
            var dic = new Dictionary<string, TTreeView>(tree.Count);
            foreach (var chapter in tree)
            {
                dic.Add(chapter.Id, chapter);
            }
            foreach (var chapter in dic.Values)
            {
                if (dic.ContainsKey(chapter.ParentId))
                {
                    if (dic[chapter.ParentId].Children == null)
                        dic[chapter.ParentId].Children = new List<TTreeView>();
                    dic[chapter.ParentId].Children.Add(chapter);
                }
            }
            var result = new List<TTreeView>();
            if (parentid == "")
            {
                if (!string.IsNullOrEmpty(firstnode))
                {
                    result.Add(new TTreeView() { Id = "", Label = firstnode, Children = null, ParentId = "", Value = "" });
                }
                result.AddRange(dic.Values.Where(t => t.ParentId.IsEmpty()).ToList());
            }
            else
            {
                result.AddRange(dic.Values.Where(t => t.ParentId == parentid).ToList());
            }
            return result;
        }

        /// <summary>
        /// 生成树形数据
        /// </summary>
        /// <param name="tree">非树形的集合（获取数据组成该格式）</param>
        /// <param name="ignore">忽略默认第一节点</param>
        /// <param name="parentid">返回节点树数据</param>
        /// <param name="firstnode">第一个节点名称（空代表不填）</param>
        /// <returns></returns>
        public static List<TTreeView> ToTree<TTreeView>(this IEnumerable<TTreeView> tree, bool ignore = true, string parentid = "", string firstnode = "请选择") where TTreeView : ITreeView<TTreeView>, new()
        {
            var dic = new Dictionary<string, TTreeView>(tree.Count());
            foreach (var chapter in tree)
            {
                dic.Add(chapter.Id, chapter);
            }
            foreach (var chapter in dic.Values)
            {
                if (dic.ContainsKey(chapter.ParentId))
                {
                    if (dic[chapter.ParentId].Children == null)
                        dic[chapter.ParentId].Children = new List<TTreeView>();
                    dic[chapter.ParentId].Children.Add(chapter);
                }
            }
            var result = new List<TTreeView>();
            if (parentid == "" && ignore)
            {
                if (!string.IsNullOrEmpty(firstnode))
                {
                    result.Add(new TTreeView() { Id = "", Label = firstnode, Children = null, ParentId = "", Value = "" });
                }
                result.AddRange(dic.Values.Where(t => t.ParentId.IsEmpty()).ToList());
            }
            else
            {
                result.AddRange(dic.Values.Where(t => t.ParentId == parentid).ToList());
            }
            return result;
        }
    }
}
