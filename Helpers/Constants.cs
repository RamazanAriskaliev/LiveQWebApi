using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveQ.Api.Helpers
{
    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id";
            }

            public static class JwtClaims
            {
                public const string Creator = "Creator";
                public const string Administrator = "Administrator";
                public const string Subscriber = "Subscriber";
                
            }

            public static class AppData
            {
                public const string FromAddress = "ramazan@liveq.com";


            }
        }        
    }
}
