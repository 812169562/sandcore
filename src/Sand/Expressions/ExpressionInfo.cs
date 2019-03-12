namespace Sand.Expressions {
    /// <summary>
    /// 表达式信息
    /// </summary>
    public class ExpressionInfo {
        /// <summary>
        /// 数据类型
        /// </summary>
        public DataType Type { get; set; }
        /// <summary>
        /// 属性名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 是否验证必填项
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// 必填项验证消息
        /// </summary>
        public string RequiredMessage { get; set; }
        /// <summary>
        /// 最大字符串长度
        /// </summary>
        public int? MaxLength { get; set; }
        /// <summary>
        /// 最小字符串长度
        /// </summary>
        public int? MinLength { get; set; }
        /// <summary>
        /// 字符串长度验证消息
        /// </summary>
        public string StringLengthMessage { get; set; }
        /// <summary>
        /// 是否验证电子邮件
        /// </summary>
        public bool Email { get; set; }
        /// <summary>
        /// 电子邮件验证消息
        /// </summary>
        public string EmailMessage { get; set; }
        /// <summary>
        /// 是否验证Url
        /// </summary>
        public bool Url { get; set; }
        /// <summary>
        /// Url验证消息
        /// </summary>
        public string UrlMessage { get; set; }
    }
}
