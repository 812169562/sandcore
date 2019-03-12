using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Sand.Helpers;

namespace Sand.Expressions {
    /// <summary>
    /// 表达式信息解析器
    /// </summary>
    public class ExpressionInfoResolver<TEntity, TProperty> {
        /// <summary>
        /// 初始化表达式信息解析器
        /// </summary>
        /// <param name="expression">表达式</param>
        public ExpressionInfoResolver( Expression<Func<TEntity, TProperty>> expression ) {
            _expression = expression;
            _expressionInfo = new ExpressionInfo();
        }

        /// <summary>
        /// 表达式
        /// </summary>
        private readonly Expression<Func<TEntity, TProperty>> _expression;
        /// <summary>
        /// 表达式信息
        /// </summary>
        private readonly ExpressionInfo _expressionInfo;

        /// <summary>
        /// 解析表达式
        /// </summary>
        public ExpressionInfo Resolve() {
            //ResolveDataType();
            //ResolveName();
            //ResolveDisplayName();
            //ResolveValidationAttributes();
            return _expressionInfo;
        }

        ///// <summary>
        ///// 解析数据类型
        ///// </summary>
        //private void ResolveDataType() {
        //    var memberInfo = Lambda.GetMember( _expression );
        //    if( Reflection.IsInt( memberInfo ) ) {
        //        _expressionInfo.Type = DataType.Int;
        //        return;
        //    }
        //    if ( Reflection.IsDate( memberInfo ) ) {
        //        _expressionInfo.Type = DataType.DateTime;
        //        return;
        //    }
        //    if ( Reflection.IsNumber( memberInfo ) ) {
        //        _expressionInfo.Type = DataType.Number;
        //        return;
        //    }
        //    if ( Reflection.IsBool( memberInfo ) ) {
        //        _expressionInfo.Type = DataType.Bool;
        //        return;
        //    }
        //    if ( Reflection.IsEnum( memberInfo ) ) {
        //        _expressionInfo.Type = DataType.Enum;
        //        return;
        //    }
        //}

        ///// <summary>
        ///// 解析并设置name属性
        ///// </summary>
        //private void ResolveName() {
        //    _expressionInfo.Name = Lambda.GetName( _expression );
        //}

        ///// <summary>
        ///// 解析显示名
        ///// </summary>
        //private void ResolveDisplayName() {
        //    ResolveDisplayAttribute();
        //    ResolveDisplayNameAttribute();
        //}

        ///// <summary>
        ///// 解析Display特性
        ///// </summary>
        //private void ResolveDisplayAttribute() {
        //    var displayAttribute = Lambda.GetAttribute<DisplayAttribute>( _expression );
        //    if ( displayAttribute == null )
        //        return;
        //    _expressionInfo.DisplayName = displayAttribute.Name;
        //}

        ///// <summary>
        ///// 解析DisplayNameAttribute特性
        ///// </summary>
        //private void ResolveDisplayNameAttribute() {
        //    var displayNameAttribute = Lambda.GetAttribute<DisplayNameAttribute>( _expression );
        //    if ( displayNameAttribute == null )
        //        return;
        //    _expressionInfo.DisplayName = displayNameAttribute.DisplayName;
        //}

        ///// <summary>
        ///// 解析验证器列表
        ///// </summary>
        //private void ResolveValidationAttributes() {
        //    var validationAttributes = Lambda.GetAttributes<TEntity, TProperty, ValidationAttribute>( _expression );
        //    foreach ( var attribute in validationAttributes )
        //        ResolveValidationAttribute( attribute );
        //}

        ///// <summary>
        ///// 解析验证器
        ///// </summary>
        //private void ResolveValidationAttribute( ValidationAttribute attribute ) {
        //    if ( ResolveRequired( attribute as RequiredAttribute ) )
        //        return;
        //    if ( ResolveLength( attribute ) )
        //        return;
        //    if ( ResolveEmail( attribute as EmailAddressAttribute ) )
        //        return;
        //    if ( ResolveUrl( attribute as UrlAttribute ) )
        //        return;
        //}

        ///// <summary>
        ///// 解析必填项验证器
        ///// </summary>
        //private bool ResolveRequired( RequiredAttribute attribute ) {
        //    if ( attribute == null )
        //        return false;
        //    _expressionInfo.Required = true;
        //    _expressionInfo.RequiredMessage = attribute.GetErrorMessage();
        //    return true;
        //}

        ///// <summary>
        ///// 解析长度验证器
        ///// </summary>
        //private bool ResolveLength( ValidationAttribute attribute ) {
        //    if ( ResolveMaxLength( attribute as MaxLengthAttribute ) )
        //        return true;
        //    return ResolveStringLength( attribute as StringLengthAttribute );
        //}

        ///// <summary>
        ///// 解析最大长度验证器
        ///// </summary>
        //private bool ResolveMaxLength( MaxLengthAttribute attribute ) {
        //    if ( attribute == null )
        //        return false;
        //    _expressionInfo.MaxLength = attribute.Length;
        //    _expressionInfo.StringLengthMessage = attribute.GetErrorMessage();
        //    return true;
        //}

        ///// <summary>
        ///// 解析字符串长度验证器
        ///// </summary>
        //private bool ResolveStringLength( StringLengthAttribute attribute ) {
        //    if ( attribute == null )
        //        return false;
        //    _expressionInfo.MaxLength = attribute.MaximumLength;
        //    if ( attribute.MinimumLength > 0 )
        //        _expressionInfo.MinLength = attribute.MinimumLength;
        //    _expressionInfo.StringLengthMessage = attribute.GetErrorMessage();
        //    return true;
        //}

        ///// <summary>
        ///// 解析电子邮件验证器
        ///// </summary>
        //private bool ResolveEmail( EmailAddressAttribute attribute ) {
        //    if ( attribute == null )
        //        return false;
        //    _expressionInfo.Email = true;
        //    _expressionInfo.EmailMessage = attribute.GetErrorMessage();
        //    return true;
        //}

        ///// <summary>
        ///// 解析Url验证器
        ///// </summary>
        //private bool ResolveUrl( UrlAttribute attribute ) {
        //    if ( attribute == null )
        //        return false;
        //    _expressionInfo.Url = true;
        //    _expressionInfo.UrlMessage = attribute.GetErrorMessage();
        //    return true;
        //}
    }
}
