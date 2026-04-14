# Agents Guide — Mollie API Client for .NET

This document describes the project structure, conventions, and guidelines for AI agents working in this codebase.

---

## Project Overview

This is an open-source .NET library that wraps the [Mollie REST API](https://docs.mollie.com/). It is published as two NuGet packages:

| Package | Path | Purpose |
|---|---|---|
| `Mollie.Api` | `src/Mollie.Api` | Core API client library targeting `netstandard2.0` and `net8.0` |
| `Mollie.Api.AspNet` | `src/Mollie.Api.AspNet` | ASP.NET-specific webhook helpers |

Tests live under `tests/` and samples under `samples/Mollie.WebApplication.Blazor`.

---

## Solution Structure

```
src/
  Mollie.Api/
    Client/          # One concrete client class per Mollie API resource
    Client/Abstract/ # One interface per client
    Models/          # Request and response record types, grouped by resource
    JsonConverters/  # Custom System.Text.Json converters
    Framework/       # Auth, idempotency, retry policies, JSON service
    Extensions/      # Extension methods (IEnumerable, Dictionary helpers)
    Options/         # MollieOptions, MollieClientOptions
    DependencyInjection.cs
  Mollie.Api.AspNet/
    Webhooks/        # Model binders and signature filter
tests/
  Mollie.Tests.Unit/
    Client/          # One test class per client
    Models/          # Serialisation/deserialisation tests
    Framework/
  Mollie.Tests.Integration/
samples/
  Mollie.WebApplication.Blazor/
```

---

## Key Conventions

### Language & Target

- **C# 12** (`LangVersion` is set to `12`). Use modern language features such as `required` members, primary constructors, collection expressions, and `record` types where appropriate.
- The library targets **`netstandard2.0`** (via PolySharp for back-fill) and **`net8.0`**.
- Nullable reference types are **enabled** (`<Nullable>enable</Nullable>`). Always annotate nullability correctly.

### Naming

| Artifact | Convention | Example |
|---|---|---|
| Client interface | `I{Resource}Client` | `IPaymentClient` |
| Client class | `{Resource}Client` | `PaymentClient` |
| Request model | `{Resource}Request` | `PaymentRequest` |
| Update request | `{Resource}UpdateRequest` | `PaymentUpdateRequest` |
| Response model | `{Resource}Response` | `PaymentResponse` |
| List response | `ListResponse<{Resource}Response>` | `ListResponse<PaymentResponse>` |
| Test class | `{Resource}ClientTests` | `PaymentClientTests` |

### Client Pattern

Every API resource follows this pattern:

1. **Interface** in `Client/Abstract/I{Resource}Client.cs` — inherits `IBaseMollieClient`, all public methods documented with XML `<summary>` / `<param>` / `<returns>` comments.
2. **Implementation** in `Client/{Resource}Client.cs` — inherits `BaseMollieClient`, implements the interface.
3. **Two constructors** on every concrete client:
   - `(string apiKey, HttpClient? httpClient = null)` — for manual instantiation.
   - `[ActivatorUtilitiesConstructor] (MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)` — used by DI.
4. **Register** the new client pair in `DependencyInjection.cs` via `RegisterMollieApiClient<IFooClient, FooClient>`.

```csharp
// Typical method signature
public async Task<FooResponse> GetFooAsync(
    string fooId,
    bool testmode = false,
    CancellationToken cancellationToken = default) {

    ValidateRequiredUrlParameter(nameof(fooId), fooId);
    var queryParameters = BuildQueryParameters(testmode: testmode);
    return await GetAsync<FooResponse>(
        $"foos/{fooId}{queryParameters.ToQueryString()}",
        cancellationToken: cancellationToken).ConfigureAwait(false);
}
```

Key rules:
- Always call `ValidateRequiredUrlParameter` for ID path segments.
- Always call `ValidateApiKeyIsOauthAccesstoken()` when a parameter requires an OAuth token (e.g., `profileId`, `testmode`, `applicationFee`).
- Always pass `cancellationToken` through and call `.ConfigureAwait(false)` on every `await`.
- Use the protected helpers `GetAsync`, `PostAsync`, `PatchAsync`, `DeleteAsync`, `GetListAsync` from `BaseMollieClient` — never call `HttpClient` directly.
- Build query strings using the `BuildQueryParameters` helper and the `ToQueryString()` extension method.

### Model Pattern

- **Request** and **Response** types are `record` types.
- Required fields are annotated with the `required` keyword.
- Optional fields are nullable (`string?`, `DateTime?`, etc.).
- All properties have XML `<summary>` documentation comments.
- Use `[JsonPropertyName("...")]` only when the JSON key differs from the C# property name.
- Use `[JsonConverter(typeof(RawJsonConverter))]` for `Metadata` fields that hold raw JSON.
- JSON is handled by `System.Text.Json` — do **not** add a dependency on `Newtonsoft.Json`.
- Response models that implement `IEntity` expose an `Id` property.
- Embedded resources are placed in a nested `{Resource}EmbeddedResponse` type annotated with `[JsonPropertyName("_embedded")]`.
- HAL-style link objects are placed in a nested `{Resource}ResponseLinks` type annotated with `[JsonPropertyName("_links")]` and typed as `UrlObjectLink<T>`.

### Extensions

Use the dictionary/list extension helpers in `Mollie.Api.Extensions` for building query parameters:

```csharp
result.AddValueIfNotNullOrEmpty("include", someValue);
result.AddValueIfTrue("testmode", testmode);
includeList.AddValueIfTrue("details.qrCode", includeQrCode);
return includeList.ToIncludeParameter();
```

---

## Testing

### Framework & Libraries

| Library | Purpose |
|---|---|
| **xUnit** | Test runner |
| **Shouldly** | Fluent assertions (`value.ShouldBe(...)`) |
| **RichardSzalay.MockHttp** | Mock `HttpClient` responses |
| **Moq** | Mocking dependencies when needed |

### Unit Test Conventions

- Every client class has a corresponding `{Resource}ClientTests` class in `tests/Mollie.Tests.Unit/Client/`.
- All test classes inherit `BaseClientTests` which provides `CreateMockHttpMessageHandler(...)`.
- Follow the **Given / When / Then** (or **Arrange / Act / Assert**) comment pattern within each test.
- Tests are `async Task` with the `[Fact]` or `[Theory]` attribute.
- Never hit real Mollie endpoints in unit tests — use `MockHttpMessageHandler` to return canned JSON.
- Store expected JSON response strings as `private const string` fields in the test class.
- Use `mockHttp.VerifyNoOutstandingExpectation()` to assert all expected HTTP calls were made.

```csharp
[Fact]
public async Task GetFooAsync_WithValidId_ResponseIsDeserializedCorrectly() {
    // Given
    const string fooId = "foo_123";
    const string jsonResponse = "{ ... }";
    var mockHttp = CreateMockHttpMessageHandler(
        HttpMethod.Get,
        $"{BaseMollieClient.DefaultBaseApiEndPoint}foos/{fooId}",
        jsonResponse);
    var client = new FooClient("test_api_key", mockHttp.ToHttpClient());

    // When
    FooResponse result = await client.GetFooAsync(fooId);

    // Then
    result.Id.ShouldBe(fooId);
    mockHttp.VerifyNoOutstandingExpectation();
}
```

---

## Adding a New API Resource

Follow these steps in order:

1. **Response model** — `src/Mollie.Api/Models/{Resource}/Response/{Resource}Response.cs`
2. **Request model** (if applicable) — `src/Mollie.Api/Models/{Resource}/Request/{Resource}Request.cs`
3. **Interface** — `src/Mollie.Api/Client/Abstract/I{Resource}Client.cs`
4. **Implementation** — `src/Mollie.Api/Client/{Resource}Client.cs`
5. **Register in DI** — add `RegisterMollieApiClient<I{Resource}Client, {Resource}Client>` to `DependencyInjection.cs`
6. **Unit tests** — `tests/Mollie.Tests.Unit/Client/{Resource}ClientTests.cs`
7. **Sample page** (optional) — add a Blazor page under `samples/Mollie.WebApplication.Blazor/Pages/{Resource}/`

---

## Authentication

- **API key** authentication: string starting with `live_` or `test_`.
- **OAuth** (access token) authentication: string starting with `access_`.
- The `IMollieSecretManager` abstraction allows custom multi-tenant scenarios.
- `BaseMollieClient.ValidateApiKeyIsOauthAccesstoken()` enforces that OAuth-only parameters are not used with a plain API key.

---

## Error Handling

- HTTP errors are thrown as `MollieApiException` which contains a `MollieErrorMessage` with `Status`, `Title`, and `Detail`.
- Callers should catch `MollieApiException` to handle Mollie-specific API errors.

---

## Idempotency

- Every HTTP request automatically gets a random `Idempotency-Key` header (UUID).
- Callers can supply a custom key via `client.WithIdempotencyKey("my-key")` which returns an `IDisposable` scope.

---

## Style & Formatting

- Braces on the **same line** for namespace, class, and method declarations (Allman is **not** used).
- Indentation: **tabs** (4-space visual width as configured in the IDE).
- Opening braces for single-statement `if`/`foreach` blocks are optional but generally omitted for single-line bodies.
- `var` is used where the type is obvious from the right-hand side.
- `ConfigureAwait(false)` on every `await` inside library code.
- XML documentation on all public members.

