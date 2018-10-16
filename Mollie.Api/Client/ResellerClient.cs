using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Reseller;

namespace Mollie.Api.Client
{
    public class ResellerClient : BaseClient, IResellerClient
    {
        private readonly string _accountCreateCommand = "account-create";
        private readonly string _profileCreateCommand = "profile-create";
        private readonly string _accountDisconnectCommand = "disconnect-account";
        private readonly string _profilesCommand = "profiles";
        private readonly string _availablePayments = "available-payment-methods";

        public ResellerClient(MollieResellerConfigurationOptions options) : base("https://www.mollie.com", "v1", options, "/api/reseller"){}

        public Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request)
        {
            return PostAsync<CreateAccountResponse>(_accountCreateCommand, request);
        }

        public Task<CreateProfileResponse> CreateProfile(CreateProfileRequest request)
        {
            return PostAsync<CreateProfileResponse>(_profileCreateCommand, request);
        }

        public Task<DisconnectAccountResponse> DisconnectAccountAsync(DisconnectAccountRequest request)
        {
            return PostAsync<DisconnectAccountResponse>(_accountDisconnectCommand, request);
        }

        public Task<ProfilesResponse> GetProfiles(ProfilesRequest request)
        {
            return PostAsync<ProfilesResponse>(_profilesCommand, request);
        }

        public Task<AvailablePaymentsResponse> GetAvailablePayments(AvailablePaymentsRequest request)
        {
            return PostAsync<AvailablePaymentsResponse>(_availablePayments, request);
        }
    }

}
