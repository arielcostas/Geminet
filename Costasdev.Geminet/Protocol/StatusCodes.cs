namespace Costasdev.Geminet.Protocol;

//ReSharper disable file InconsistentNaming
//ReSharper disable file UnusedMember.Global
public static class StatusCodes
{
    /**
     * The requested resource accepts a line of textual user input.
     */
    public const byte INPUT = 10;

    /**
     * The requested resource accepts a line of sensitive user input.
     * The user agent should hide the user's input as it is typed to prevent shoulder surfing.
     */
    public static byte SENSITIVE_INPUT = 11;

    /**
     * The request was handled successfully and a response body will follow the response header.
     */
    public static byte SUCCESS = 20;

    /**
     * The server is redirecting the client to a new location for the requested resource. There is no response body.
     */
    public static byte REDIRECT_TEMPORARY = 30;

    /**
    /**
     * The requested resource should be consistently requested from the new URL provided in future.
     */
    public static byte REDIRECT_PERMANENT = 31;

    /**
     * <para>
     * The request has failed. There is no response body. The nature of the failure is temporary, i.e. an identical
     * request MAY succeed in the future.
     * </para>
     * <para>
     * The contents of &lt;META&gt; may provide additional information on the failure, and should be displayed to human users.
     * </para>
     */
    public static byte TEMPORARY_FAILURE = 40;

    public static byte SERVER_UNAVAILABLE = 41;
    public static byte CGI_ERROR = 42;
    public static byte PROXY_ERROR = 43;
    public static byte SLOW_DOWN = 44;
    public static byte PERMANENT_FAILURE = 50;
    public static byte NOT_FOUND = 51;
    public static byte GONE = 52;
    public static byte PROXY_REQUEST_REFUSED = 53;
    public static byte BAD_REQUEST = 59;
    public static byte CLIENT_CERTIFICATE_REQUIRED = 60;
    public static byte CERTIFICATE_NOT_AUTHORIZED = 61;
    public static byte CERTIFICATE_NOT_VALID = 62;
}