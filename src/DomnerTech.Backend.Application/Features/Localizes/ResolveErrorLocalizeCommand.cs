using Bas24.CommandQuery;

namespace DomnerTech.Backend.Application.Features.Localizes;

public sealed record ResolveErrorLocalizeCommand(string ErrorCode, string Lang) : IRequest<string>;