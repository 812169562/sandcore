using System.Text;

namespace Sand.Helpers  {
    /// <summary>
    /// 字符串操作 - 字符串生成器
    /// </summary>
    public partial class String {
        /// <summary>
        /// 初始化字符串操作
        /// </summary>
        public String() {
            Builder = new StringBuilder();
        }

        /// <summary>
        /// 字符串生成器
        /// </summary>
        private StringBuilder Builder { get; set; }

        /// <summary>
        /// 追加内容
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="value">值</param>
        public void Append<T>( T value ) {
            Builder.Append( value );
        }

        /// <summary>
        /// 追加内容
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="args">参数</param>
        public void Append( string value, params object[] args ) {
            if( args == null )
                args = new object[] { string.Empty };
            if( args.Length == 0 )
                Builder.Append( value );
            else
                Builder.AppendFormat( value, args );
        }

        /// <summary>
        /// 追加内容并换行
        /// </summary>
        public void AppendLine() {
            Builder.AppendLine();
        }

        /// <summary>
        /// 追加内容并换行
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="value">值</param>
        public void AppendLine<T>( T value ) {
            Append( value );
            Builder.AppendLine();
        }

        /// <summary>
        /// 追加内容并换行
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="args">参数</param>
        public void AppendLine( string value, params object[] args ) {
            Append( value, args );
            Builder.AppendLine();
        }

        /// <summary>
        /// 替换内容
        /// </summary>
        /// <param name="value">值</param>
        public void Replace( string value ) {
            Builder.Clear();
            Builder.Append( value );
        }

        /// <summary>
        /// 移除末尾字符串
        /// </summary>
        /// <param name="end">末尾字符串</param>
        public void RemoveEnd( string end ) {
            string result = Builder.ToString();
            if( !result.EndsWith( end ) )
                return;
            int index = result.LastIndexOf( end, System.StringComparison.Ordinal );
            Builder = Builder.Remove( index, end.Length );
        }

        /// <summary>
        /// 清空字符串
        /// </summary>
        public void Clear() {
            Builder = Builder.Clear();
        }

        /// <summary>
        /// 字符串长度
        /// </summary>
        public int Length {
            get {
                return Builder.Length;
            }
        }

        /// <summary>
        /// 空字符串
        /// </summary>
        public string Empty {
            get { return string.Empty; }
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        public override string ToString() {
            return Builder.ToString();
        }
    }
}
