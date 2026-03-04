using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Localizes.ErrorMessages;
using DomnerTech.Backend.Application.Pagination;

namespace DomnerTech.Backend.Application.Features.Localizes;

public class GetErrorMessagePageQuery :
    KeysetPageRequest,
    IRequest<BaseResponse<KeysetPageResult<ErrorMessageLocalizeDto>>>,
    ILogReqCreator,
    IValidatableRequest;