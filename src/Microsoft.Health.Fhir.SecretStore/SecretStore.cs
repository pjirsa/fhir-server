// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Health.Fhir.Core.Features.SecretStore;

namespace Microsoft.Health.Fhir.SecretStore
{
    public class SecretStore : ISecretStore
    {
        private IKeyVaultClient _keyVaultClient;
        private Uri _keyVaultUri;

        public SecretStore(IKeyVaultClient keyVaultClient, Uri keyVaultUri)
        {
            EnsureArg.IsNotNull(keyVaultClient, nameof(keyVaultClient));
            EnsureArg.IsNotNull(keyVaultUri, nameof(keyVaultUri));

            _keyVaultClient = keyVaultClient;
            _keyVaultUri = keyVaultUri;
        }

        public async Task<string> GetSecret(string secretName)
        {
            EnsureArg.IsNotNullOrWhiteSpace(secretName);

            SecretBundle result = await _keyVaultClient.GetSecretAsync(_keyVaultUri.AbsoluteUri, secretName);
            return result.Value;
        }

        public async Task<bool> SetSecret(string secretName, string secretValue)
        {
            EnsureArg.IsNotNullOrWhiteSpace(secretName, nameof(secretName));
            EnsureArg.IsNotNullOrWhiteSpace(secretValue, nameof(secretValue));

            SecretBundle result = await _keyVaultClient.SetSecretAsync(_keyVaultUri.AbsoluteUri, secretName, secretValue);

            return true;
        }

        public async Task<bool> DeleteSecret(string secretName)
        {
            EnsureArg.IsNotNullOrWhiteSpace(secretName, nameof(secretName));

            SecretBundle result = await _keyVaultClient.DeleteSecretAsync(_keyVaultUri.AbsoluteUri, secretName);
            return true;
        }
    }
}
