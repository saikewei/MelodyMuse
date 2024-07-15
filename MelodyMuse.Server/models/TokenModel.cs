/*
    JWT生成与解析存放信息的模型
 */


namespace MelodyMuse.Server.models
{
    //Token解析可获得UserId,Username,UserPhone
    public class ParsedTokenModel
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public string UserPhone { get; set; }

    }



    //生成JWT需要UserId,UserName,UserPhone    
    //JWT只会在login时生成，其他页面只需要解析即可
    public class GenerateTokenModel
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public string UserPhone { get; set; }
    }
}
