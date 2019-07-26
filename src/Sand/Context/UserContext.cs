using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sand.Dependency;
using Sand.Exceptions;
using Sand.Extensions;
using Sand.Helpers;

namespace Sand.Context
{
    /// <summary>
    /// 
    /// </summary>
    public interface IThird { }

    /// <summary>
    /// 用户信息
    /// </summary>
    public interface IUserContext : IDependency
    {
        /// <summary>
        /// 登录编号
        /// </summary>
        string LoginKey { get; set; }
        /// <summary>
        /// 登录帐号
        /// </summary>
        string LoginCode { get; set; }
        /// <summary>
        /// 登录人
        /// </summary>
        string LoginName { get; set; }
        /// <summary>
        /// 租户编号
        /// </summary>
        string TenantId { get; set; }
        /// <summary>
        /// 门店编号
        /// </summary>
        string StroeId { get; set; }
        /// <summary>
        /// 微信识别号
        /// </summary>
        string WxOpenId { get; set; }
        /// <summary>
        /// 微信识别号
        /// </summary>
        IThird ThirdData { get; set; }
        /// <summary>
        /// 系统用户角色类型
        /// </summary>
        SystemUserType Type { get; set; }
        /// <summary>
        /// 设置值
        /// </summary>
        void Set(IUserContext userContext);
    }

    /// <summary>
    /// 用户
    /// </summary>
    public class DefaultUserContext : IUserContext
    {
        /// <summary>
        /// 登录编号
        /// </summary>
        public string LoginKey { get; set; }
        /// <summary>
        /// 登录帐号
        /// </summary>
        public string LoginCode { get; set; }
        /// <summary>
        /// 登录人
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 租户编号
        /// </summary>
        public string TenantId { get; set; }
        /// <summary>
        /// 门店编号
        /// </summary>
        public string StroeId { get; set; }
        /// <summary>
        /// 微信识别号
        /// </summary>
        public string WxOpenId { get; set; }
        /// <summary>
        /// 系统用户角色类型
        /// </summary>
        public SystemUserType Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IThird ThirdData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public DefaultUserContext(IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext.User.Claims;
            if (!claims.Any()) return ;
            if (claims.FirstOrDefault(t => t.Type == "id").Value == null) throw new Warning("登录已经过期请重新登录", "401");
            this.LoginKey = claims.FirstOrDefault(t => t.Type == "id").Value.SafeString().ToAesDecrypt();
            this.LoginName = claims.FirstOrDefault(t => t.Type == "name").Value;
            this.TenantId = claims.FirstOrDefault(t => t.Type == "id2").Value.SafeString().ToAesDecrypt();
            this.StroeId = claims.FirstOrDefault(t => t.Type == "id3").Value.SafeString().ToAesDecrypt();
            this.WxOpenId = claims.FirstOrDefault(t => t.Type == "id4").Value.SafeString().ToAesDecrypt();
            if (claims.FirstOrDefault(t => t.Type == "id5") != null)
            {
                this.Type = (SystemUserType)claims.FirstOrDefault(t => t.Type == "id5").Value.SafeString().ToAesDecrypt().ToInt();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userContext"></param>
        public void Set(IUserContext userContext)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class TestUserContext : IUserContext
    {
        /// <summary>
        /// 登录编号
        /// </summary>
        public string LoginKey { get; set; }
        /// <summary>
        /// 登录帐号
        /// </summary>
        public string LoginCode { get; set; }
        /// <summary>
        /// 登录人
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 租户编号
        /// </summary>
        public string TenantId { get; set; }
        /// <summary>
        /// 门店编号
        /// </summary>
        public string StroeId { get; set; }
        /// <summary>
        /// 微信识别号
        /// </summary>
        public string WxOpenId { get; set; }
        /// <summary>
        /// 系统用户角色类型
        /// </summary>
        public SystemUserType Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IThird ThirdData { get; set; }

        /// <summary>
        /// 设置测试数据
        /// </summary>
        public TestUserContext()
        {
            LoginKey = "001";
            LoginCode = "002";
            LoginName = "测试";
            StroeId = "000";
            TenantId = "000";
        }
        /// <summary>
        /// 设置测试数据
        /// </summary>
        /// <param name="userContext"></param>
        public void Set(IUserContext userContext)
        {
            this.LoginKey = userContext.LoginKey;
            this.LoginCode = userContext.LoginCode;
            this.LoginName = userContext.LoginCode;
            this.TenantId = userContext.TenantId;
            this.StroeId = userContext.StroeId;
        }
    }

    /// <summary>
    /// 系统用户角色类型
    /// </summary>
    public enum SystemUserType
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        Unknow = 0,
        /// <summary>
        /// 超级管理员
        /// </summary>
        SuperAdmin = 1,
        /// <summary>
        /// 连锁诊所管理员
        /// </summary>
        TenantAdmin = 2,
        /// <summary>
        /// 门店管理员
        /// </summary>
        StroeAdmin = 3
    }
}
