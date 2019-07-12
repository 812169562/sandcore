using Sand.Exceptions;
using System;

namespace Sand.Extensions
{
    /// <summary>
    /// 验证扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 检测空值,为null则抛出ArgumentNullException异常
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="parameterName">参数名</param>
        public static void CheckNull(this object obj, string parameterName)
        {
            if (obj == null)
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// 验证身份证正确性
        /// </summary>
        /// <param name="idcard">身份证</param>
        /// <param name="ischeck">是否验证</param>
        public static void CheckIdCard(this string idcard, bool ischeck = true)
        {
            if (!ischeck)
                return;
            if (string.IsNullOrEmpty(idcard))
                throw new Warning("身份证信息不正确");

            if (idcard.Length < 15)
            {
                throw new Warning("输入的身份证长度不正确");
            }

            if (idcard.Length == 15)
            {
                if (!CheckCid_15(idcard))
                {
                    throw new Warning("输入的身份证不合法");
                }
            }
            if (idcard.Length == 18)
            {
                if (!CheckCid_18(idcard))
                {
                    throw new Warning("输入的身份证不合法");
                }
            }
        }

        #region 18位的身份证格式与合法性
        /// <summary>
        /// 验证18位的身份证格式与合法性，并输出身份证信息(地区,出生年月日,性别)
        /// </summary>
        /// <param name="sCardId">身份证卡号</param>
        /// <returns></returns>
        private static bool CheckCid_18(string sCardId)
        {
            string[] cardMessage;
            //河南省开头是 41 且没有00 01 02 等开头的省份，北京市是11，故按照下面的方式构造数组
            string[] aCity = new string[]
            {
                null, null, null, null, null, null, null, null, null, null, null, //00 - 10
                "北京", "天津", "河北", "山西", "内蒙古", //11 - 15
                null, null, null, null, null, //16 - 20
                "辽宁", "吉林", "黑龙江", //21 - 23
                null, null, null, null, null, null, null, //24 - 30
                "上海", "江苏", "浙江", "安微", "福建", "江西", "山东",//31 - 37
                null, null, null, //38 - 40
                "河南", "湖北", "湖南", "广东", "广西", "海南", //41 - 47
                null, null, null, //48 - 50
                "重庆", "四川", "贵州", "云南", "西藏", //51 - 55
                null, null, null, null, null, null, //56 - 60
                "陕西", "甘肃", "青海", "宁夏", "新疆", //61 - 65
                null, null, null, null, null, //66 - 70
                "台湾",//71
                null, null, null, null, null, null, null, null, null,//72 - 80
                "香港", "澳门", //81 82
                null, null, null, null, null, null, null, null,//83 - 90
                "国外" //91
            };
            double iSum = 0;
            //构造正则表达式对象
            System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^\d{17}(\d|x|X)$");
            System.Text.RegularExpressions.Match mc = rg.Match(sCardId);
            if (!mc.Success) //匹配失败
            {
                cardMessage = new string[] { "身份证信息不合法" };
                return false;
            }
            sCardId = sCardId.ToLower();
            sCardId = sCardId.Replace("x", "a");
            if (aCity[int.Parse(sCardId.Substring(0, 2))] == null)
            {
                cardMessage = new string[] { "非法地区" };
                return false;
            }
            string borth = sCardId.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime borthTest;
            if (!DateTime.TryParse(borth, out borthTest))
            {
                cardMessage = new string[] { "非法生日" };
                return false;
            }
            for (int i = 17; i >= 0; i--)
            {
                iSum += (System.Math.Pow(2, i) % 11) * int.Parse(sCardId[17 - i].ToString(), System.Globalization.NumberStyles.HexNumber);
            }
            if (iSum % 11 != 1)
            {
                cardMessage = new string[] { "非法证号" };
                return false;
            }
            //地区、生日、性别
            cardMessage = new string[] { aCity[int.Parse(sCardId.Substring(0, 2))], borth, (int.Parse(sCardId.Substring(16, 1)) % 2 == 1 ? "男" : "女") };
            return true;
        }

        #region 15位的身份证格式与合法性
        /// <summary>
        /// 验证15位的身份证格式与合法性，并输出身份证信息(地区,出生年月日,性别)
        /// </summary>
        /// <param name="sCardId">身份证卡号</param>
        /// <returns></returns>
        private static bool CheckCid_15(string sCardId)
        {
            string[] cardMessage;
            //河南省开头是 41 且没有00 01 02 等开头的省份，北京市是11，故按照下面的方式构造数组
            string[] aCity = new string[]
            {
                null, null, null, null, null, null, null, null, null, null, null, //00 - 10
                "北京", "天津", "河北", "山西", "内蒙古", //11 - 15
                null, null, null, null, null, //16 - 20
                "辽宁", "吉林", "黑龙江", //21 - 23
                null, null, null, null, null, null, null, //24 - 30
                "上海", "江苏", "浙江", "安微", "福建", "江西", "山东",//31 - 37
                null, null, null, //38 - 40
                "河南", "湖北", "湖南", "广东", "广西", "海南", //41 - 47
                null, null, null, //48 - 50
                "重庆", "四川", "贵州", "云南", "西藏", //51 - 55
                null, null, null, null, null, null, //56 - 60
                "陕西", "甘肃", "青海", "宁夏", "新疆", //61 - 65
                null, null, null, null, null, //66 - 70
                "台湾",//71
                null, null, null, null, null, null, null, null, null,//72 - 80
                "香港", "澳门", //81 82
                null, null, null, null, null, null, null, null,//83 - 90
                "国外" //91
            };
            //构造正则表达式对象
            System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^\d{15}$");
            System.Text.RegularExpressions.Match mc = rg.Match(sCardId);
            if (!mc.Success) //匹配失败
            {
                cardMessage = new string[] { "身份证信息不合法" };
                return false;
            }
            sCardId = sCardId.ToLower();
            sCardId = sCardId.Replace("x", "a");
            if (aCity[int.Parse(sCardId.Substring(0, 2))] == null)
            {
                cardMessage = new string[] { "非法地区" };
                return false;
            }
            string borth = sCardId.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime borthTest;
            if (!DateTime.TryParse(borth, out borthTest))
            {
                cardMessage = new string[] { "非法生日" };
                return false;
            }
            //地区、生日、性别
            cardMessage = new string[] { aCity[int.Parse(sCardId.Substring(0, 2))], borth, (int.Parse(sCardId.Substring(13, 1)) % 2 == 1 ? "男" : "女") };
            return true;
        }
        #endregion
        #endregion
    }
}
