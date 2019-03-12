using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Sand.Api;
using Sand.Extensions;
using Xunit;
using Sand.Utils.Enums;

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
    }
}
