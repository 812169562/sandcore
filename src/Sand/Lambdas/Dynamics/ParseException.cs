using System;

namespace Sand.Lambdas.Dynamics {
    /// <summary>
    /// 
    /// </summary>
    public sealed class ParseException : Exception {
        int position;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="position"></param>
        public ParseException( string message, int position )
            : base( message ) {
            this.position = position;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Position {
            get { return position; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return string.Format( Res.ParseExceptionFormat, Message, position );
        }
    }
}
