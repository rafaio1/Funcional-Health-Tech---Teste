namespace FHT.Infra.Seguranca.Config
{
    public class ConfiguracaoJwt
    {
        public string SecretKey { get; set; }

        public string MasterPassword { get; set; }

        public string Environment { get; set; }

        public int Expiration { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
