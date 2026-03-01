using System.Linq.Expressions;
using System.Reflection;

namespace DomnerTech.Backend.Application.Pagination;

public sealed class CursorField<T>(Expression<Func<T, object>> fieldFunc, bool descending = true)
{
    public Expression<Func<T, object>> FieldFunc { get; } = fieldFunc;
    public bool Descending { get; } = descending;

    public static CursorField<T> FromProperty(PropertyInfo prop, bool descending = false)
    {
        ArgumentNullException.ThrowIfNull(prop);
        var param = Expression.Parameter(typeof(T), "i");
        var propAccess = Expression.Property(param, prop);
        var converted = Expression.Convert(propAccess, typeof(object));
        var lambda = Expression.Lambda<Func<T, object>>(converted, param);
        return new CursorField<T>(lambda, descending);
    }

    public string GetFieldName()
    {
        var body = FieldFunc.Body;

        // unwrap conversions (object boxing)
        if (body is UnaryExpression { NodeType: ExpressionType.Convert } ue)
            body = ue.Operand;

        return body switch
        {
            // Member access: i => i.Prop
            MemberExpression me => me.Member.Name,
            // Constant: i => "PropName" (legacy or explicit name)
            ConstantExpression { Value: string s } => s,
            _ => throw new InvalidOperationException("Unsupported expression for cursor field name extraction.")
        };
    }
}