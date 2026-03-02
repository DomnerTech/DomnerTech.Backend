using DomnerTech.Backend.Application.Enums;
using Microsoft.AspNetCore.Http;

namespace DomnerTech.Backend.Application.Extensions;

public static class HttpContextExtensions
{
    public static string GetCurrentLanguage(this IHttpContextAccessor accessor)
    {
        var lang = accessor.HttpContext?.Request.Headers["lang"].FirstOrDefault()?.ToLower() ?? "En";
        return GetCurrentLanguageInternal(lang);
    }
    public static string GetCurrentLanguage(this HttpContext httpContext)
    {
        var lang = httpContext.Request.Headers["lang"].FirstOrDefault()?.ToLower() ?? "En";
        return GetCurrentLanguageInternal(lang);
    }

    private static string GetCurrentLanguageInternal(string lang)
    {
        var languageSupport = Enum.GetValues<LanguageSupportType>();
        return languageSupport.Any(x => x.ToName().Equals(lang, StringComparison.CurrentCultureIgnoreCase))
            ? lang
            : LanguageSupportType.En.ToName().ToLower();
    }
}