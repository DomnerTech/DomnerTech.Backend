using Bas24.CommandQuery;
using DomnerTech.Backend.Application.IRepo;

namespace DomnerTech.Backend.Application.Features.Localizes.Handlers;

public sealed class ResolveErrorLocalizeCommandHandler(IErrorMessageLocalizeRepo errorMessageLocalize) : IRequestHandler<ResolveErrorLocalizeCommand, string>
{
    public async Task<string> Handle(ResolveErrorLocalizeCommand request, CancellationToken cancellationToken)
    {
        return await errorMessageLocalize.ResolveAsync(request.ErrorCode, request.Lang, cancellationToken);
    }
}