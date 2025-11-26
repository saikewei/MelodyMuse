using MelodyMuse.Server.Configure; // 引用你的配置类
using MelodyMuse.Server.models;    // 引用你的 TokenModel 定义
using MelodyMuse.Server.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace MelodyMuse.Server.Controllers.Facades
{
    /// <summary>
    /// 【外观模式】
    /// 封装身份认证上下文的获取逻辑。
    /// 负责从 HTTP Header 中提取 Token，解析用户 ID 和用户信息。
    /// </summary>
    public class UserContextFacade
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        // 通过构造函数注入 HttpContextAccessor，这是访问 Request 的关键
        public UserContextFacade(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取当前请求中的原始 Token 字符串
        /// </summary>
        private string GetRawToken()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                throw new InvalidOperationException("当前不在 HTTP 请求上下文中");
            }

            // 获取 Authorization 头，通常格式为 "Bearer {token}"
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader))
            {
                throw new UnauthorizedAccessException("未提供 Authorization 请求头");
            }

            // 提取 Token 部分 (兼容有没有 "Bearer " 前缀的情况)
            var token = authHeader.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("令牌格式错误或为空");
            }

            return token;
        }

        /// <summary>
        /// 【核心方法 1】直接获取当前登录用户的 ID
        /// 如果获取失败，会直接抛出 UnauthorizedAccessException
        /// </summary>
        public string GetCurrentUserId()
        {
            var token = GetRawToken();

            // 使用你现有的工具类进行解析
            // 注意：这里假设 Token2Id 返回 null 表示失败
            var userId = TokenParser.Token2Id(token, JWTConfigure.serect_key);

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("令牌无效或已过期");
            }

            return userId;
        }

        /// <summary>
        /// 【核心方法 2】获取当前登录用户的完整 Token 信息 (包含用户名、手机号等)
        /// </summary>
        public ParsedTokenModel GetCurrentUserTokenInfo()
        {
            var token = GetRawToken();

            // 使用你现有的工具类进行解析
            var tokenModel = TokenParser.ParseToken(token, JWTConfigure.serect_key);

            if (tokenModel == null)
            {
                throw new UnauthorizedAccessException("无法解析令牌信息");
            }

            return tokenModel;
        }
    }
}
