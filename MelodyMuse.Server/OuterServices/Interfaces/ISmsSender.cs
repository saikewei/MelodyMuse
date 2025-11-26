namespace MelodyMuse.Server.OuterServices.Interfaces
{
    /// 通用的短信发送载荷（不包含任何特定厂商的字段）
    public class GenericSmsMessage
    {
        /// 目标手机号（不含国家码，适配器负责处理国家码）
        public string PhoneNumber { get; set; }

        /// 模板参数集合。
        /// 不同的厂商/模板需要不同数量的参数，使用数组按顺序传递。
        public string[] TemplateParams { get; set; }
    }

    /// 目标接口 (Target)：所有短信服务适配器必须实现此接口
    public interface ISmsSender
    {
        Task<bool> SendAsync(GenericSmsMessage message);
    }
}
