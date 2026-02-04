using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Session.Request;
using Mollie.Api.Models.Session.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface ISessionClient : IBaseMollieClient {

        /// <summary>
        /// Create a new Session.
        /// </summary>
        /// <param name="request">The Session request object containing the Session details</param>
        /// <param name="cancellationToken">Token to cancel the request</param>
        /// <returns>The Session object created by Mollie</returns>
        Task<SessionResponse> CreateSessionAsync(SessionRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve a single Session by its ID.
        /// </summary>
        /// <param name="sessionId">The Session ID of the Session to retrieve</param>
        /// <param name="testmode">Indicates whether the Session is in test mode or not</param>
        /// <param name="cancellationToken">Token to cancel the request</param>
        /// <returns>The Session object retrieved by Mollie</returns>
        Task<SessionResponse> GetSessionAsync(string sessionId, bool testmode = false, CancellationToken cancellationToken = default);

    }
}
