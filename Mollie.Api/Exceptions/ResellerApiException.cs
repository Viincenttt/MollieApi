using System;
using Mollie.Api.Extensions;
using Mollie.Api.Models.Reseller;

namespace Mollie.Api.Exceptions
{
    public class ResellerApiException : Exception
    {
        public ResellerApiErrorMessage Details { get; set; }

        public ResellerApiException(string xmlResponse)
        {
            Details = ParseXmlResponse(xmlResponse);
        }

        protected ResellerApiException()
        {
        }

        protected ResellerApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public override string ToString()
        {
            return Details != null ? $"{Details?.Code}: {Details?.Message}" : base.ToString();
        }

        private static ResellerApiErrorMessage ParseXmlResponse(string xmlResponse)
        {
            var xmlObject = xmlResponse.Deserialize<BaseResellerResponse>();

            return new ResellerApiErrorMessage
            {
                Code = xmlObject.ResultCode,
                Message = xmlObject.ResultMessage
            };
        }
    }
}
