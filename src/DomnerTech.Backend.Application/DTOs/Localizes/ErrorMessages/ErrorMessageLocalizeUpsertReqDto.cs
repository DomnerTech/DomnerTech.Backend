namespace DomnerTech.Backend.Application.DTOs.Localizes.ErrorMessages;

public record ErrorMessageLocalizeUpsertReqDto(string Key, Dictionary<string, string> Messages);