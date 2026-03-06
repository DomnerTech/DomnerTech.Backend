using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.Enums;
using Microsoft.AspNetCore.Http;

namespace DomnerTech.Backend.Application.Extensions;

public static class HttpContextExtensions
{
    public static string GetCurrentLanguage(this IHttpContextAccessor accessor)
    {
        var lang = accessor.HttpContext?.Items[HeaderConstants.Lang]?.ToString() ?? nameof(LanguageSupportType.En).ToLower();
        return GetCurrentLanguageInternal(lang);
    }
    public static string GetCurrentLanguage(this HttpContext httpContext)
    {
        var lang = httpContext.Items[HeaderConstants.Lang]?.ToString() ?? nameof(LanguageSupportType.En).ToLower();
        return GetCurrentLanguageInternal(lang);
    }

    public static string GetClaim(this HttpContext httpContext, string claimType)
    {
        return httpContext.User.Claims.FirstOrDefault(c => c.Type == claimType)?.Value ?? string.Empty;
    }

    private static string GetCurrentLanguageInternal(string lang)
    {
        var languageSupport = Enum.GetValues<LanguageSupportType>();
        return languageSupport.Any(x => x.ToName(true).Equals(lang, StringComparison.CurrentCultureIgnoreCase))
            ? lang
            : nameof(LanguageSupportType.En).ToLower();
    }
}