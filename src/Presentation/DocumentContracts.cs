namespace DocumentAPI.Presentation.Documents;

public sealed record UploadDocumentRequest(string Title, string Content);

public sealed record MutateDocumentRequest(string Content, string? Title);
