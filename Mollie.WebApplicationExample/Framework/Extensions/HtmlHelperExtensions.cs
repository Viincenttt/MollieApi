using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mollie.WebApplicationExample.Framework.Extensions;

public static class HtmlHelperExtensions {
    public static IEnumerable<SelectListItem> GetStaticStringSelectList(this IHtmlHelper htmlHelper, Type type) {
        foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public)) {
            string value = fieldInfo.GetValue(null).ToString();

            yield return new SelectListItem() {
                Value = value,
                Text = value
            };
        }
    }
}
