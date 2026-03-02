using System.ComponentModel;
using System.Reflection;

namespace DomnerTech.Backend.Application.Extensions;

public static class EnumExtensions
{
    /* =========================
     *  ENUM → STRING
     * ========================= */

    extension<T>(T value) where T : struct, Enum
    {
        public string ToName(bool isLower = false) => isLower ? value.ToString().ToLower() : value.ToString();

        public string ToDescription()
        {
            var member = typeof(T).GetMember(value.ToString()).FirstOrDefault();
            var attribute = member?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
    }

    /* =========================
     *  STRING → ENUM
     * ========================= */

    extension(string value)
    {
        public T ToEnum<T>(bool ignoreCase = true)
            where T : struct, Enum
        {
            return string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Value cannot be null or empty.", nameof(value)) : Enum.Parse<T>(value, ignoreCase);
        }

        public bool TryToEnum<T>(out T result, bool ignoreCase = true)
            where T : struct, Enum
        {
            return Enum.TryParse(value, ignoreCase, out result);
        }

        public T ToEnumOrDefault<T>(T defaultValue, bool ignoreCase = true)
            where T : struct, Enum
        {
            return Enum.TryParse(value, ignoreCase, out T result)
                ? result
                : defaultValue;
        }
    }
}