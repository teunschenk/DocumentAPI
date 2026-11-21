using DocumentAPI.Application.Commands;
using DocumentAPI.Application.Handlers;
using DocumentAPI.Application.Models;
using DocumentAPI.Application.Queries;
using DocumentAPI.Domain.Documents;

namespace DocumentAPI.Application.Documents;

public sealed class DocumentService
{
    private readonly IDocumentRepository _documentRepository;
    private readonly ApproveDocumentHandler _handler;
    private readonly GetDocumentByIdQueryHandler _getDocumentByIdQueryHandler;
    private readonly ValidateDocumentHandler _validateDocumentHandler;

    /// <summary>
    /// Validates, approves and stores a document
    /// </summary>
    /// <param name="documentRepository"></param>
    /// <param name="handler"></param>
    /// <param name="getDocumentByIdQueryHandler"></param>
    /// <param name="validateDocumentHandler"></param>
    public DocumentService(
        IDocumentRepository documentRepository,
        ApproveDocumentHandler handler,
        GetDocumentByIdQueryHandler getDocumentByIdQueryHandler,
        ValidateDocumentHandler validateDocumentHandler)
    {
        _documentRepository = documentRepository;
        _handler = handler;
        _getDocumentByIdQueryHandler = getDocumentByIdQueryHandler;
        _validateDocumentHandler = validateDocumentHandler;
    }

    public async Task<DocumentDto> UploadAsync(UploadDocumentCommand command, CancellationToken cancellationToken)
    {
        var document = Document.Create(command.Content);

        // Validate the document content
        var isValid = await _validateDocumentHandler.HandleAsync(
            new ValidateDocumentCommand(document.Content),
            cancellationToken);

        if(!isValid)
        {
            throw new InvalidOperationException("Document is not valid.");
        }

        // Store the document in the repository
        await _documentRepository.AddAsync(document, cancellationToken);

        // Approve the document
        var approved = await _handler.HandleAsync(
            new ApproveDocumentCommand(document.Id),
            cancellationToken);

        // Retrieve the document with approval by its ID and return it
        return await GetByIdAsync(document.Id, cancellationToken);
    }

    public async Task<DocumentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _getDocumentByIdQueryHandler.HandleAsync(
            new GetDocumentByIdQuery(id),
            cancellationToken);
    }    
}
