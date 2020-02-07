using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Cloud.Tencent.Ocr
{
    /// <summary>
    /// 身份证识别
    /// </summary>
    public class IdCard
    {
        /// <summary>
        /// 姓名（人像面）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别（人像面）
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 民族（人像面）
        /// </summary>
        public string Nation { get; set; }

        /// <summary>
        /// 出生日期（人像面）
        /// </summary>
        public string Birth { get; set; }

        /// <summary>
        /// 地址（人像面）
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 身份证号（人像面）
        /// </summary>
        public string IdNum { get; set; }

        /// <summary>
        /// 发证机关（国徽面）
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// 证件有效期（国徽面）
        /// </summary>
        public string ValidDate { get; set; }

        /// <summary>
        /// 扩展信息，根据请求的可选字段返回对应内容，不请求则不返回，具体输入参考示例3和示例4。
        ///目前支持的扩展字段为：
        ///  IdCard，身份证照片，请求 CropIdCard 时返回；
        ///  Portrait，人像照片，请求 CropPortrait 时返回；
        ///  WarnInfos，告警信息（Code - 告警码），识别出以下告警内容时返回。
        ///  Code 告警码列表和释义：
        ///  -9100 身份证有效日期不合法告警，
        ///  -9101 身份证边框不完整告警，
        ///  -9102 身份证复印件告警，
        ///  -9103 身份证翻拍告警，
        ///  -9105 身份证框内遮挡告警，
        ///  -9104 临时身份证告警，
        ///  -9106 身份证 PS 告警。
        /// </summary>
        public string AdvancedInfo { get; set; }
        /// <summary>
        /// 唯一请求 ID，每次请求都会返回。定位问题时需要提供该次请求的 RequestId
        /// </summary>
        public string RequestId { get; set; }
    }
}