using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Infrastructure.Options
{
    public sealed class CryptoOptions
    {
        public required string PrivateKey { get; init; }
        public required string PublicKey { get; init; }

        public required string Issuer { get; init; }
        public required string Audience { get; init; }
    }
}
