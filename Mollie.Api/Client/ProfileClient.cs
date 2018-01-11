using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Profile.Request;
using Mollie.Api.Models.Profile.Response;

namespace Mollie.Api.Client {
    public class ProfileClient : BaseMollieClient, IProfileClient {
        public ProfileClient(string apiKey) : base(apiKey) {
        }

        public async Task<ProfileResponse> CreateProfileAsync(ProfileRequest request) {
            return await PostAsync<ProfileResponse>("profiles", request)
                .ConfigureAwait(false);
        }

        public async Task<ProfileResponse> GetProfileAsync(string profileId) {
            return await GetAsync<ProfileResponse>($"profiles/{profileId}")
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ProfileResponse>> GetProfileListAsync(int? offset = null, int? count = null) {
            return await GetListAsync<ListResponse<ProfileResponse>>("profiles", offset, count)
                .ConfigureAwait(false);
        }

        public async Task<ProfileResponse> UpdateProfileAsync(string profileId, ProfileRequest request) {
            return await PostAsync<ProfileResponse>($"profiles/{profileId}", request)
                .ConfigureAwait(false);
        }

        public async Task DeleteProfileAsync(string profileId) {
            await DeleteAsync($"profiles/{profileId}")
                .ConfigureAwait(false);
        }

        public async Task<ListResponseSimple<ApiKey>> GetProfileApiKeyListAsync(string profileId) {
            return await GetListAsync<ListResponseSimple<ApiKey>>($"profiles/{profileId}/apikeys", null, null)
                .ConfigureAwait(false);
        }

        public async Task<ApiKey> GetProfileApiKeyAsync(string profileId, Mode mode) {
            return await GetAsync<ApiKey>($"profiles/{profileId}/apikeys/{mode.ToString().ToLower()}")
                .ConfigureAwait(false);
        }

        public async Task<ApiKey> ResetProfileApiKeyAsync(string profileId, Mode mode) {
            return await PostAsync<ApiKey>($"profiles/{profileId}/apikeys/{mode.ToString().ToLower()}", null)
                .ConfigureAwait(false);
        }
    }
}