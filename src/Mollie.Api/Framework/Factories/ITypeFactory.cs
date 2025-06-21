namespace Mollie.Api.Framework.Factories;

internal interface ITypeFactory<T> {
    T Create(string? type);
}
