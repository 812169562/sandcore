namespace Sand.Cloud.Tencent.Ocr
{
    public class InputParm
    {

        public InputParm()
        {
            CardSide = "FRONT";
            Config = "{\"DetectPsWarn\":true}";
        }
        /// <summary>
        /// 图片的 Base64 值。
       /// 支持的图片格式：PNG、JPG、JPEG，暂不支持 GIF 格式。
        ///支持的图片大小：所下载图片经Base64编码后不超过 7M。图片下载时间不超过 3 秒。
       /// 图片的 ImageUrl、ImageBase64 必须提供一个，如果都提供，只使用 ImageUrl。
        /// </summary>
        public  string ImageBase64 { get; set; }
        /// <summary>
        ///图片的 Url 地址。
        /// 支持的图片格式：PNG、JPG、JPEG，暂不支持 GIF 格式。
        ///支持的图片大小：所下载图片经Base64编码后不超过 7M。图片下载时间不超过 3 秒。
        /// 图片的 ImageUrl、ImageBase64 必须提供一个，如果都提供，只使用 ImageUrl。
        /// </summary>
        public  string ImageUrl { get; set; }
        /// <summary>
        /// FRONT 为身份证有照片的一面（人像面）
        /// BACK 为身份证有国徽的一面（国徽面）
        /// </summary>
        public string CardSide  { get; set; }
        /// <summary>
        /// 可选字段，根据需要选择是否请求对应字段。
        /// 目前包含的字段为：
        /// CropIdCard，身份证照片裁剪，bool 类型，默认false，
        /// CropPortrait，人像照片裁剪，bool 类型，默认false，
        /// CopyWarn，复印件告警，bool 类型，默认false，
        /// BorderCheckWarn，边框和框内遮挡告警，bool 类型，默认false，
        /// ReshootWarn，翻拍告警，bool 类型，默认false，
        /// DetectPsWarn，PS检测告警，bool类型，默认false，
        /// TempIdWarn，临时身份证告警，bool类型，默认false，
        /// InvalidDateWarn，身份证有效日期不合法告警，bool类型，默认false。
        /// SDK 设置方式参考：
        /// Config = Json.stringify({"CropIdCard":true,"CropPortrait":true})
        /// API 3.0 Explorer 设置方式参考：
        /// Config = {"CropIdCard":true,"CropPortrait":true}
        /// </summary>
        public string  Config { get; set; }
    }
}