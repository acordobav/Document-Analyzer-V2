using System;

using AuthLibrary.Configuration;
using AuthLibrary.Factory;

using System.Collections.Generic;
using System.Security.Claims;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            AuthServiceConfig.Config.KeycloakHost = "localhost";
            AuthServiceConfig.Config.KeycloakPort = "8080";
            AuthServiceConfig.Config.RealmName = "docanalyzer";

            IAuthServiceFactory authFactory = new AuthServiceFactory();

            string token1 = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICIwUmllVXRFdGRhVERmNUV2Q0s1OGpla2lsWTl0VEF6a1JMVkt6T1BjN1FnIn0.eyJleHAiOjE2MjE2ODcyODEsImlhdCI6MTYyMTY4MDA4MSwiYXV0aF90aW1lIjoxNjIxNjgwMDcwLCJqdGkiOiI3YjQ2Yjg4Zi05ZDM2LTRkNGEtODYwNC01NmEzYzg1NmNmNzciLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjgwODAvYXV0aC9yZWFsbXMvZG9jYW5hbHl6ZXIiLCJhdWQiOiJhY2NvdW50Iiwic3ViIjoiYmIzN2FkMmQtOWJlMC00NjFiLTg0N2ItNTE5ZjJjZjE0ZjUzIiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiZG9jYW5hbHl6ZXItcmVhY3QtY2xpZW50Iiwibm9uY2UiOiIwNTU3Y2ZlMi1kNTQ5LTQxYWUtODg3OS03MjA1NjliZGU5MTgiLCJzZXNzaW9uX3N0YXRlIjoiZDQwZWU3ODAtNzljYS00MzFhLThjOWUtOWQwNDUxNzc1ZWI0IiwiYWNyIjoiMCIsImFsbG93ZWQtb3JpZ2lucyI6WyJodHRwOi8vbG9jYWxob3N0OjMwMDAiXSwicmVhbG1fYWNjZXNzIjp7InJvbGVzIjpbImRlZmF1bHQtcm9sZXMtZG9jYW5hbHl6ZXIiLCJvZmZsaW5lX2FjY2VzcyIsInVtYV9hdXRob3JpemF0aW9uIl19LCJyZXNvdXJjZV9hY2Nlc3MiOnsiYWNjb3VudCI6eyJyb2xlcyI6WyJtYW5hZ2UtYWNjb3VudCIsIm1hbmFnZS1hY2NvdW50LWxpbmtzIiwidmlldy1wcm9maWxlIl19fSwic2NvcGUiOiJvcGVuaWQgcHJvZmlsZSBlbWFpbCIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwibmFtZSI6IkFydHVybyBDb3Jkb2JhIiwicHJlZmVycmVkX3VzZXJuYW1lIjoiYXJ0dXJvY3YxNkBnbWFpbC5jb20iLCJnaXZlbl9uYW1lIjoiQXJ0dXJvIiwiZmFtaWx5X25hbWUiOiJDb3Jkb2JhIiwiZW1haWwiOiJhcnR1cm9jdjE2QGdtYWlsLmNvbSJ9.l0bhn3MhM38yS-JMYEvIehKVqr5oNtLggLEcVTJm3ngbhX4E5ZF2C1jFmtf6efOSQ4BSWApTzF34IymmjDcTWoPWJjSiGCByhFlUu7IhlcpPOHP_KRIPNhywBpjOiQ0M_Frt_mvqTvedYn90EqzNPRZdb5QY1-NpbRCvevRIUBmu-uXqVMzJg-kP1fw-ZHcTjH_5BBVwoBPhtMzCNF8cSBZJ_Tj1ziwRzBcvnn8cj8ASFq1uspIL3qqWA_lS9ocfAi5lPkF3awlJn71ppJwbJ7Ak8rUjKyAN4MukEsy8dIhgRD5seqYKnTGQLW1X2WyjbBi-gDD7FOtzh_01uVMFQg";
            bool result = authFactory.Authorization.Authorize(token1);
            Console.WriteLine(result);

            /*
            ClaimsPrincipal claims = authFactory.Authorization.Claims;
            string owner = claims.FindFirst("email").Value;
            Console.WriteLine(owner);
            */


        }
    }
}
