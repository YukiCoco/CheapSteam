namespace CheamSteam.UI.Services
{
    public class TokenProvider
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string XsrfToken {get; set; }
    }

    public class InitialApplicationState
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string XsrfToken { get; set; }
    }
}