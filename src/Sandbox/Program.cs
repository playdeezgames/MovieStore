using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Sandbox
{
    internal class Program
    {
        private const string KEY_VAULT_URI = "KEY_VAULT_URI";
        private const string SECRET_NAME = "SECRET_NAME";

        static void Main(string[] args)
        {
            string secret = GetConnectionString();

            Console.WriteLine($"{secret}");
            Console.ReadLine();
        }

        private static string GetConnectionString()
        {
            var vaultUri = new Uri(Environment.GetEnvironmentVariable(KEY_VAULT_URI)!);
            var secretName = Environment.GetEnvironmentVariable(SECRET_NAME);
            var credential = new DefaultAzureCredential();
            var client = new SecretClient(vaultUri, credential);
            var secret = client.GetSecret(secretName).Value.Value;
            return secret;
        }
    }
}
