namespace Costasdev.Geminet.Protocol;

//ReSharper disable file InconsistentNaming
//ReSharper disable file UnusedMember.Global
//ReSharper disable file AutoPropertyCanBeMadeGetOnly.Global
public class Response
{
    public int StatusCode { get; set; }
    public string Meta { get; set; }
    public string Body { get; set; }

    public Response(string body) : this(StatusCodes.SUCCESS, ContentTypes.Text.GEMINI, body)
    {
    }

    public Response(int statusCode, string meta = ContentTypes.Text.GEMINI, string body = "")
    {
        StatusCode = statusCode;
        Meta = meta;
        Body = body;
    }

    public override string ToString()
    {
        return $"{StatusCode} {Meta}\r\n{Body}";
    }
}