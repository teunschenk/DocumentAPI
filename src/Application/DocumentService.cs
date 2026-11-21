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
        await _documentRepository.AddAsync(document, cancellationToken);

        var isValid = await _validateDocumentHandler.HandleAsync(
            new ValidateDocumentCommand(document.Content),
            cancellationToken);

        if(!isValid)
        {
            throw new InvalidOperationException("Document is not valid.");
        }

        var approved = await _handler.HandleAsync(
            new ApproveDocumentCommand(document.Id),
            cancellationToken);
        
        return await GetByIdAsync(document.Id, cancellationToken);
    }

    public async Task<DocumentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _getDocumentByIdQueryHandler.HandleAsync(
            new GetDocumentByIdQuery(id),
            cancellationToken);
    }    
}
