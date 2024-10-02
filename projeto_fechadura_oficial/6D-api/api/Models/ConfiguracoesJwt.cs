namespace _6D.Models
{
    public class ConfiguracoesJwt
    {
        public string Secret { get; set; }

        public int TokenLifetimeMinutes { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
