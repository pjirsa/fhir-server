// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Threading.Tasks;

namespace Microsoft.Health.Fhir.Core.Features.SecretStore
{
    public interface ISecretStore
    {
        Task<string> GetSecret(string secretName);

        Task<bool> SetSecret(string secretName, string secretValue);

        Task<bool> DeleteSecret(string secretName);
    }
}
