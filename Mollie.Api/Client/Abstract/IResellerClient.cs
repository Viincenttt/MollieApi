using System.Threading.Tasks;
using Mollie.Api.Models.Reseller;
using DisconnectAccountResponse = Mollie.Api.Models.Reseller.DisconnectAccountResponse;

namespace Mollie.Api.Client.Abstract
{
    public interface IResellerClient
    {
        Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request);

        Task<DisconnectAccountResponse> DisconnectAccountAsync(DisconnectAccountRequest request);

        Task<CreateProfileResponse> CreateProfile(CreateProfileRequest request);

        Task<ProfilesResponse> GetProfiles(ProfilesRequest request);
    }
}
