﻿using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.ClientLink.Response {
    public record ClientLinkResponseLinks {
        public required UrlLink ClientLink { get; init; }
        public required UrlLink Documentation { get; init; }
    }
}