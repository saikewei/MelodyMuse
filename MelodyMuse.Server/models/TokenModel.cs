namespace MelodyMuse.Server.models
{
    public class ParsedTokenModel
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public string UserPhone { get; set; }

    }


    public class GenerateTokenModel
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public string UserPhone { get; set; }
    }
}
