namespace Mollie.Api.ContractResolvers {
    public class SnakeCasePropertyNamesContractResolver : DeliminatorSeparatedPropertyNamesContractResolver {
        public SnakeCasePropertyNamesContractResolver() : base('_') {
        }
    }
}