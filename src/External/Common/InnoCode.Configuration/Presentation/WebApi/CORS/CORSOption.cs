namespace InnoCode.Configuration.Presentation.WebApi.CORS;

public sealed class CORSOption
{
    public string[] Origins { get; set; } = [];

    public string[] Headers { get; set; } = [];

    public string[] Methods { get; set; } = [];

    public bool AreCredentialsAllowed { get; set; }
}
