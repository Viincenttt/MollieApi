using System.Net;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Error;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Unit.Models {
    public class MollieResultTests {
        [Fact]
        public void EnsureSuccess_SuccessfulResult_ReturnsResult() {
            // Arrange
            var result = new MollieResult { Success = true };

            // Act
            MollieResult returnedResult = result.EnsureSuccess();

            // Assert
            returnedResult.ShouldBeSameAs(result);
        }

        [Fact]
        public void EnsureSuccess_SuccessfulTypedResult_ReturnsData() {
            // Arrange
            var result = new MollieResult<string> {
                Success = true,
                Data = "data"
            };

            // Act
            string data = result.EnsureSuccess();

            // Assert
            data.ShouldBe("data");
        }

        [Fact]
        public void EnsureSuccess_FailedResult_ThrowsMollieApiException() {
            // Arrange
            var error = new MollieErrorMessage {
                Status = 422,
                Title = "Unprocessable Entity",
                Detail = "The description is invalid"
            };
            var result = new MollieResult {
                Success = false,
                HttpStatusCode = HttpStatusCode.UnprocessableEntity,
                Error = error
            };

            // Act
            var exception = Should.Throw<MollieApiException>(() => result.EnsureSuccess());

            // Assert
            exception.Details.ShouldBe(error);
            exception.Result.ShouldBeSameAs(result);
            exception.Message.ShouldBe("Unprocessable Entity - The description is invalid");
        }

        [Fact]
        public void EnsureSuccess_FailedTypedResult_ThrowsMollieApiException() {
            // Arrange
            var error = new MollieErrorMessage {
                Status = 404,
                Title = "Not Found",
                Detail = "No payment exists with token tr_123"
            };
            var result = new MollieResult<string> {
                Success = false,
                HttpStatusCode = HttpStatusCode.NotFound,
                Error = error
            };

            // Act
            var exception = Should.Throw<MollieApiException>(() => result.EnsureSuccess());

            // Assert
            exception.Details.ShouldBe(error);
            exception.Result.ShouldBeSameAs(result);
        }

        [Fact]
        public void EnsureSuccess_FailedTypedResultAccessedAsBaseType_ThrowsMollieApiException() {
            // Arrange
            MollieResult result = new MollieResult<string> {
                Success = false,
                Error = new MollieErrorMessage {
                    Status = 404,
                    Title = "Not Found",
                    Detail = "No payment exists with token tr_123"
                }
            };

            // Act & assert
            Should.Throw<MollieApiException>(() => result.EnsureSuccess());
        }

        [Fact]
        public void EnsureSuccess_FailedResultWithoutError_ThrowsMollieApiExceptionWithUnknownError() {
            // Arrange
            var result = new MollieResult {
                Success = false,
                HttpStatusCode = HttpStatusCode.BadGateway,
                ResponseBody = "Bad gateway"
            };

            // Act
            var exception = Should.Throw<MollieApiException>(() => result.EnsureSuccess());

            // Assert
            exception.Details.Status.ShouldBe(502);
            exception.Details.Title.ShouldBe("Unknown error");
            exception.Details.Detail.ShouldBe("Bad gateway");
        }
    }
}
