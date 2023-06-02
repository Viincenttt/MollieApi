using System.Reflection;

namespace Mollie.WebApplication.Blazor.Framework; 

public static class StaticStringListBuilder {
    public static IEnumerable<string> GetStaticStringList(Type type) {
        foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public)) {
            string value = fieldInfo.GetValue(null).ToString();
            yield return value;
        }
    }
}