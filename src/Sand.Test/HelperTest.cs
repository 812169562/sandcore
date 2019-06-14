using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Sand.Api;
using Sand.Extensions;
using Xunit;
using Sand.Utils.Enums;
using System.Runtime.Serialization;
using Sand.Utils.Tree;
namespace Sand.Test
{
    public class HelperTest
    {
        [Fact]
        public void EunmDescription()
        {
            var value = SystemStatus.Pause.Description();
            Assert.Equal(value, "停用");
        }
        [Fact]
        public void EunmDisplay()
        {
            var value = SystemStatus.Pause.DisplayName();
            Assert.Equal(value, "否");
        }

        [Fact]
        public void EunmList()
        {
            var values = SystemStatus.Pause.GetEnumList();
            foreach (var item in values)
            {
                if (item.Value == SystemStatus.Pause.Value())
                {
                    Assert.Equal(item.Value, 1);
                    Assert.Equal(item.DisplayName, "否");
                    Assert.Equal(item.Description, "停用");
                }
                if (item.Value == SystemStatus.Using.Value())
                {
                    Assert.Equal(item.Value, 0);
                    Assert.Equal(item.DisplayName, "是");
                    Assert.Equal(item.Description, "正常");
                }
            }
        }
        [Fact]
        public void IsEmpty()
        {
            var str = "1";
            Assert.Equal(str.IsEmpty(), false);
        }

        [Fact]
        public void IsNotEmpty()
        {
            var str = "1";
            Assert.Equal(str.IsNotEmpty(), true);
        }

        [Fact]
        public void IsWhiteSpaceEmpty()
        {
            var str = "  ";
            Assert.Equal(str.IsWhiteSpaceEmpty(), true);

            var str1 = " ";
            Assert.Equal(str1.IsWhiteSpaceEmpty(), true);

            var str2 = "1";
            Assert.Equal(str2.IsWhiteSpaceEmpty(), false);
        }

        [Fact]
        public void IsNotWhiteSpaceEmpty()
        {
            var str = "  ";
            Assert.Equal(str.IsNotWhiteSpaceEmpty(), false);

            var str1 = " ";
            Assert.Equal(str1.IsNotWhiteSpaceEmpty(), false);

            var str2 = "1";
            Assert.Equal(str2.IsNotWhiteSpaceEmpty(), true);
        }

        [Fact]
        public void Guid()
        {
            //var list = new List<Guid>();
            //Guid first;
            //Guid end;
            //for (int i = 0; i < 100000; i++)
            //{
            //    Thread.Sleep(1);
            //    var guid = Helpers.Uuid.Next();
            //    if (i == 0)
            //    {
            //        first = guid;
            //    }
            //    if (i == 99999)
            //    {
            //        end = guid;
            //    }
            //    list.Add(guid);
            //}
            //Assert.Equal(first, list.Min());
            //Assert.Equal(end, list.Max());
        }

        [Fact]
        public void TestWu()
        {
            string str1 = "中国";
            string str2 = "中华人民共和国";
            string str3 = "松果科技";
            string str4 = "成都";
            var temp1 = Sand.Helpers.String.WuBi(str1);
            var temp2 = Sand.Helpers.String.WuBi(str2);
            var temp3 = Sand.Helpers.String.WuBi(str3);
            var temp4 = Sand.Helpers.String.WuBi(str4);
            Assert.Equal(temp1, "kl");
            Assert.Equal(temp2, "kwwnatl");
            Assert.Equal(temp3, "sjtr");
            Assert.NotEqual(temp4, "fd");
        }

        [Fact]
        public void ToTreeTest()
        {
            try
            {
                var f = "[{\"Value\":\"119C0F21-0146-40FA-B0AD-F61A324CC986\",\"id\":\"119C0F21-0146-40FA-B0AD-F61A324CC986\",\"label\":\"中医眼科\",\"parent_id\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"TreeCode\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"IsLeaf\":false,\"Children\":null,\"child_num\":0,\"depth\":2,\"Status\":\"否\",\"Code\":\"00030052\",\"description\":null,\"Disabled\":true,\"IsEnable\":false,\"Version\":\"66966838-0E8E-4EA0-B399-46CBDC85\",\"Sort\":0,\"PrintName\":null,\"Url\":null,\"Path\":null,\"Para\":null,\"Selected\":false,\"Selecteds\":null},{\"Value\":\"28F965B9-CCE9-4FC6-97C0-06B82C87B00E\",\"id\":\"28F965B9-CCE9-4FC6-97C0-06B82C87B00E\",\"label\":\"中西医结合科\",\"parent_id\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"TreeCode\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"IsLeaf\":false,\"Children\":null,\"child_num\":0,\"depth\":2,\"Status\":\"否\",\"Code\":\"00030056\",\"description\":null,\"Disabled\":true,\"IsEnable\":false,\"Version\":\"66966838-0E8E-4EA0-B399-46CBDC85\",\"Sort\":0,\"PrintName\":null,\"Url\":null,\"Path\":null,\"Para\":null,\"Selected\":false,\"Selecteds\":null},{\"Value\":\"33DFD832-2FA7-4049-B37E-4B4518972613\",\"id\":\"33DFD832-2FA7-4049-B37E-4B4518972613\",\"label\":\"中医眼科（银海）\",\"parent_id\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"TreeCode\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"IsLeaf\":false,\"Children\":null,\"child_num\":0,\"depth\":2,\"Status\":\"否\",\"Code\":\"K0000030\",\"description\":null,\"Disabled\":true,\"IsEnable\":false,\"Version\":\"66966838-0E8E-4EA0-B399-46CBDC85\",\"Sort\":0,\"PrintName\":null,\"Url\":null,\"Path\":null,\"Para\":null,\"Selected\":false,\"Selecteds\":null},{\"Value\":\"4E641E79-4A24-4AE1-9356-7B46C6673E74\",\"id\":\"4E641E79-4A24-4AE1-9356-7B46C6673E74\",\"label\":\"针灸科\",\"parent_id\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"TreeCode\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"IsLeaf\":false,\"Children\":null,\"child_num\":0,\"depth\":2,\"Status\":\"否\",\"Code\":\"00030049\",\"description\":null,\"Disabled\":true,\"IsEnable\":false,\"Version\":\"66966838-0E8E-4EA0-B399-46CBDC85\",\"Sort\":0,\"PrintName\":null,\"Url\":null,\"Path\":null,\"Para\":null,\"Selected\":false,\"Selecteds\":null},{\"Value\":\"56DD4946-D5A3-4B3B-988F-237AD59A6445\",\"id\":\"56DD4946-D5A3-4B3B-988F-237AD59A6445\",\"label\":\"中医妇产科\",\"parent_id\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"TreeCode\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"IsLeaf\":false,\"Children\":null,\"child_num\":0,\"depth\":2,\"Status\":\"否\",\"Code\":\"K0000027\",\"description\":null,\"Disabled\":true,\"IsEnable\":false,\"Version\":\"66966838-0E8E-4EA0-B399-46CBDC85\",\"Sort\":0,\"PrintName\":null,\"Url\":null,\"Path\":null,\"Para\":null,\"Selected\":false,\"Selecteds\":null},{\"Value\":\"66722D00-8805-4E6F-B5E7-82DA00F090E7\",\"id\":\"66722D00-8805-4E6F-B5E7-82DA00F090E7\",\"label\":\"中医外科\",\"parent_id\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"TreeCode\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"IsLeaf\":false,\"Children\":null,\"child_num\":0,\"depth\":2,\"Status\":\"否\",\"Code\":\"K0000028\",\"description\":null,\"Disabled\":true,\"IsEnable\":false,\"Version\":\"66966838-0E8E-4EA0-B399-46CBDC85\",\"Sort\":0,\"PrintName\":null,\"Url\":null,\"Path\":null,\"Para\":null,\"Selected\":false,\"Selecteds\":null},{\"Value\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"id\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"label\":\"成都中医药大学国医馆\",\"parent_id\":null,\"TreeCode\":null,\"IsLeaf\":false,\"Children\":null,\"child_num\":0,\"depth\":1,\"Status\":\"否\",\"Code\":null,\"description\":null,\"Disabled\":true,\"IsEnable\":false,\"Version\":\"66966838-0E8E-4EA0-B399-46CBDC85\",\"Sort\":0,\"PrintName\":null,\"Url\":null,\"Path\":null,\"Para\":null,\"Selected\":false,\"Selecteds\":null},{\"Value\":\"744F2597-8348-4BE9-8F08-F7654EC4F814\",\"id\":\"744F2597-8348-4BE9-8F08-F7654EC4F814\",\"label\":\"中医内科\",\"parent_id\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"TreeCode\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"IsLeaf\":false,\"Children\":null,\"child_num\":0,\"depth\":2,\"Status\":\"否\",\"Code\":\"K0000026\",\"description\":null,\"Disabled\":true,\"IsEnable\":false,\"Version\":\"66966838-0E8E-4EA0-B399-46CBDC85\",\"Sort\":0,\"PrintName\":null,\"Url\":null,\"Path\":null,\"Para\":null,\"Selected\":false,\"Selecteds\":null},{\"Value\":\"9AFA2C02-9BCF-40B6-8C23-49F405A44AE2\",\"id\":\"9AFA2C02-9BCF-40B6-8C23-49F405A44AE2\",\"label\":\"中医骨伤科\",\"parent_id\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"TreeCode\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"IsLeaf\":false,\"Children\":null,\"child_num\":0,\"depth\":2,\"Status\":\"否\",\"Code\":\"00030051\",\"description\":null,\"Disabled\":true,\"IsEnable\":false,\"Version\":\"66966838-0E8E-4EA0-B399-46CBDC85\",\"Sort\":0,\"PrintName\":null,\"Url\":null,\"Path\":null,\"Para\":null,\"Selected\":false,\"Selecteds\":null},{\"Value\":\"B20A7847-1AF6-4CF9-A5E5-DBF7412BB813\",\"id\":\"B20A7847-1AF6-4CF9-A5E5-DBF7412BB813\",\"label\":\"中医肛肠科\",\"parent_id\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"TreeCode\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"IsLeaf\":false,\"Children\":null,\"child_num\":0,\"depth\":2,\"Status\":\"否\",\"Code\":\"00030053\",\"description\":null,\"Disabled\":true,\"IsEnable\":false,\"Version\":\"66966838-0E8E-4EA0-B399-46CBDC85\",\"Sort\":0,\"PrintName\":null,\"Url\":null,\"Path\":null,\"Para\":null,\"Selected\":false,\"Selecteds\":null},{\"Value\":\"C828EA19-C73A-40B1-B069-104215A64542\",\"id\":\"C828EA19-C73A-40B1-B069-104215A64542\",\"label\":\"美容中医科\",\"parent_id\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"TreeCode\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"IsLeaf\":false,\"Children\":null,\"child_num\":0,\"depth\":2,\"Status\":\"否\",\"Code\":\"00030225\",\"description\":null,\"Disabled\":true,\"IsEnable\":false,\"Version\":\"66966838-0E8E-4EA0-B399-46CBDC85\",\"Sort\":0,\"PrintName\":null,\"Url\":null,\"Path\":null,\"Para\":null,\"Selected\":false,\"Selecteds\":null},{\"Value\":\"CD122D69-56C9-4732-BD10-BF255D4844EA\",\"id\":\"CD122D69-56C9-4732-BD10-BF255D4844EA\",\"label\":\"推拿科\",\"parent_id\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"TreeCode\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"IsLeaf\":false,\"Children\":null,\"child_num\":0,\"depth\":2,\"Status\":\"否\",\"Code\":\"00030050\",\"description\":null,\"Disabled\":true,\"IsEnable\":false,\"Version\":\"66966838-0E8E-4EA0-B399-46CBDC85\",\"Sort\":0,\"PrintName\":null,\"Url\":null,\"Path\":null,\"Para\":null,\"Selected\":false,\"Selecteds\":null},{\"Value\":\"D34BA23A-A8FE-41FE-975B-6152EC6597DF\",\"id\":\"D34BA23A-A8FE-41FE-975B-6152EC6597DF\",\"label\":\"中医儿科\",\"parent_id\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"TreeCode\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"IsLeaf\":false,\"Children\":null,\"child_num\":0,\"depth\":2,\"Status\":\"否\",\"Code\":\"K0000029\",\"description\":null,\"Disabled\":true,\"IsEnable\":false,\"Version\":\"66966838-0E8E-4EA0-B399-46CBDC85\",\"Sort\":0,\"PrintName\":null,\"Url\":null,\"Path\":null,\"Para\":null,\"Selected\":false,\"Selecteds\":null},{\"Value\":\"D9F45491-6BEA-407E-A14D-CE9DB0B37D63\",\"id\":\"D9F45491-6BEA-407E-A14D-CE9DB0B37D63\",\"label\":\"中医耳鼻咽喉科\",\"parent_id\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"TreeCode\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"IsLeaf\":false,\"Children\":null,\"child_num\":0,\"depth\":2,\"Status\":\"否\",\"Code\":\"00030054\",\"description\":null,\"Disabled\":true,\"IsEnable\":false,\"Version\":\"66966838-0E8E-4EA0-B399-46CBDC85\",\"Sort\":0,\"PrintName\":null,\"Url\":null,\"Path\":null,\"Para\":null,\"Selected\":false,\"Selecteds\":null},{\"Value\":\"DD0959E1-041F-42AB-A12E-407C26784AFD\",\"id\":\"DD0959E1-041F-42AB-A12E-407C26784AFD\",\"label\":\"中医肿瘤科\",\"parent_id\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"TreeCode\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"IsLeaf\":false,\"Children\":null,\"child_num\":0,\"depth\":2,\"Status\":\"否\",\"Code\":\"00030058\",\"description\":null,\"Disabled\":true,\"IsEnable\":false,\"Version\":\"66966838-0E8E-4EA0-B399-46CBDC85\",\"Sort\":0,\"PrintName\":null,\"Url\":null,\"Path\":null,\"Para\":null,\"Selected\":false,\"Selecteds\":null},{\"Value\":\"E2119DAE-8B15-46BC-ABA8-F903DDA60D1D\",\"id\":\"E2119DAE-8B15-46BC-ABA8-F903DDA60D1D\",\"label\":\"中医皮肤科\",\"parent_id\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"TreeCode\":\"66966838-0E8E-4EA0-B399-46CBDC857335\",\"IsLeaf\":false,\"Children\":null,\"child_num\":0,\"depth\":2,\"Status\":\"否\",\"Code\":\"00030055\",\"description\":null,\"Disabled\":true,\"IsEnable\":false,\"Version\":\"66966838-0E8E-4EA0-B399-46CBDC85\",\"Sort\":0,\"PrintName\":null,\"Url\":null,\"Path\":null,\"Para\":null,\"Selected\":false,\"Selecteds\":null}]";
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VueTableTreeView>>(f);
                var ff = data.ToTree();
                var m = ff;
            }
            catch (Exception ex)
            {
                throw;
            }
         
        }

        public class Tree : TreeView
        {
            //public  string Id { get; set; }
            //
            // 摘要:
            //     地址
            [DataMember]
            public string Url { get; set; }
            //
            // 摘要:
            //     打印名称
            [DataMember]
            public new string PrintName { get; set; }
            //
            // 摘要:
            //     排序号
            [DataMember]
            public int Sort { get; set; }
            //
            // 摘要:
            //     版本号
            [DataMember]
            public new string Version { get; set; }
            //
            // 摘要:
            //     是否可用
            [DataMember]
            public bool IsEnable { get; set; }
            //
            // 摘要:
            //     是否可用
            [DataMember]
            public new bool Disabled { get; set; }
            //
            // 摘要:
            //     备注
            [DataMember(Name = "description")]
            public new string Remark { get; set; }
            //
            // 摘要:
            //     编号
            [DataMember]
            public new string Code { get; set; }
            //
            // 摘要:
            //     路径
            [DataMember]
            public string Path { get; set; }
            //
            // 摘要:
            //     状态
            [DataMember]
            public new string Status { get; set; }
            //
            // 摘要:
            //     子节点条数
            [DataMember(Name = "child_num")]
            public int ChildrenCount { get; }
            //
            // 摘要:
            //     子节点
            [DataMember]
            public new List<VueTableTreeView> Children { get; set; }
            //
            // 摘要:
            //     是否为叶子节点
            [DataMember]
            public new bool IsLeaf { get; set; }
            //
            // 摘要:
            //     关系编号
            [DataMember]
            public string TreeCode { get; set; }
            //
            // 摘要:
            //     父节点
            [DataMember(Name = "parent_id")]
            public string ParentId { get; set; }
            //
            // 摘要:
            //     节点标签
            [DataMember(Name = "label")]
            public string Label { get; set; }
            //
            // 摘要:
            //     节点编号
            public string Value { get; set; }
            //
            // 摘要:
            //     层数
            [DataMember(Name = "depth")]
            public int Level { get; }
            //
            // 摘要:
            //     参数
            [DataMember]
            public string Para { get; set; }
        }

    }
}
