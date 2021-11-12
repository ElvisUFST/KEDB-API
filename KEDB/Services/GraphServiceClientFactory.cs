using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;

namespace KEDB.Services
{
    //Opretter en Authentication provider og Graph Service Client 
    static class GraphServiceClientFactory
    {
        public static GraphServiceClient Create(string clientId, string tenantId, string secret)
        {
            if (clientId == null) throw new ArgumentNullException(nameof(clientId));
            if (tenantId == null) throw new ArgumentNullException(nameof(tenantId));
            if (secret == null) throw new ArgumentNullException(nameof(secret));

            // Initiate client application
            var confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(clientId)
                    .WithTenantId(tenantId)
                    .WithClientSecret(secret)
                    .Build();

            var authProvider = new ClientCredentialProvider(confidentialClientApplication);

            return new GraphServiceClient(authProvider);
        }
    }
}