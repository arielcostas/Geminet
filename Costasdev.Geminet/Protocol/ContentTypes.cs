namespace Costasdev.Geminet.Protocol;

//ReSharper disable file InconsistentNaming
//ReSharper disable file UnusedMember.Global
public class ContentTypes
{
    public static class Text
    {
        public const string GEMINI = "text/gemini";
        public const string PLAIN = "text/plain";
        public const string HTML = "text/html";
        public const string MARKDOWN = "text/markdown";
    }

    public static class Image
    {
        public const string GIF = "image/gif";
        public const string JPEG = "image/jpeg";
        public const string PNG = "image/png";
        public const string SVG = "image/svg+xml";
    }

    public static class Audio
    {
        public const string MPEG = "audio/mpeg";
        public const string OGG = "audio/ogg";
        public const string WAV = "audio/wav";
    }

    public static class Video
    {
        public const string MPEG = "video/mpeg";
        public const string OGG = "video/ogg";
        public const string WEBM = "video/webm";
    }
}