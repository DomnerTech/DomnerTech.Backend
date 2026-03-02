using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;

namespace DomnerTech.Backend.Application.Features.Localizes;

public sealed record ErrorMessageLocalizeUpsertCommand(string Key, Dictionary<string, string> Messages) : 
    IRequest<BaseResponse<bool>>, ILogReqCreator, IValidatableRequest;