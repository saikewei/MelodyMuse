namespace MelodyMuse.Server.Services.Factories
{
    public enum UploadType
    {
        Audio,
        Lyrics
    }
    public class UploadHandlerFactory
    {
        // 工厂方法：根据类型生产处理器
        public static IFtpUploadHandler CreateHandler(UploadType type)
        {
            return type switch
            {
                UploadType.Audio => new AudioUploadHandler(),
                UploadType.Lyrics => new LyricsUploadHandler(),
                _ => throw new ArgumentException("Invalid upload type")
            };
        }
    }
}
