﻿//Licence URLs:
//https://www.viacode.com/viacode-incident-management-license/
//https://www.viacode.com/gnu-affero-general-public-license/

using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace ITSMConnector
{
    internal static class KeyVault
    {
        public static string Login { get; private set; }
        public static string Password { get; private set; }

        private static readonly string VaultUrl = System.Environment.GetEnvironmentVariable("KeyVaultUrl");

        static KeyVault()
        {
            GetSecrets();
        }

        private static void GetSecrets()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            using (var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback)))
            {
                var secretLogin = keyVaultClient.GetSecretAsync(VaultUrl, nameof(Login));
                secretLogin.Wait();
                Login = secretLogin.Result.Value;
                var secretPassword = keyVaultClient.GetSecretAsync(VaultUrl, nameof(Password));
                secretPassword.Wait();
                Password = secretPassword.Result.Value;
            }
        }
    }
}