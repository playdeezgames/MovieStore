using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Data.SqlClient;

namespace MovieStore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var connection = new SqlConnection(GetDatabaseConnectionString());
            connection.Open();
            MainMenu.Run(connection);
            connection.Close();
        }

        private static string GetDatabaseConnectionString()
        {
            var vaultUri = new Uri("https://splorr-kv.vault.azure.net/");
            var secretName = "SplorrDatabase";
            var credential = new DefaultAzureCredential();
            var client = new SecretClient(vaultUri, credential);
            return client.GetSecret(secretName).Value.Value;
        }
    }
}
