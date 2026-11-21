using DocumentAPI.Domain.Documents;

namespace DocumentAPI.Application.Models;

public sealed record DocumentDto(
    Guid Id,
    string Content);
