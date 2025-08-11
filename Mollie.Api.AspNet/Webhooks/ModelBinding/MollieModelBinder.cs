using System.Reflection;
using Microsoft.AspNetCore.Http;
using Mollie.Api.Framework;
using Mollie.Api.Models;

namespace Mollie.Api.AspNet.Webhooks.ModelBinding;

public class MollieModelBinder<T> where T : IEntity {
    public T? Model { get; init; }

    public static async ValueTask<MollieModelBinder<T>?> BindAsync(HttpContext context, ParameterInfo parameter) {
        JsonConverterService jsonConverterService = new();

        var request = context.Request;
        request.EnableBuffering();

        var reader = new StreamReader(request.Body);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        try {
            var result = jsonConverterService.Deserialize<T>(body);
            return new MollieModelBinder<T>() {
                Model = result
            };
        }
        catch (Exception) {
            return null;
        }
    }
}
