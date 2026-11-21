using DocumentAPI.Application.Documents;
using DocumentAPI.Domain.Documents;

namespace DocumentAPI.Application.Handlers;

/// <summary>
/// Update the document with a mutation
/// </summary>
/// <param name="documentRepository"></param>
/// <param name="timeProvider"></param>
public sealed class MutateDocumentHandler(
    IDocumentRepository documentRepository,
    TimeProvider timeProvider)
{
    public async Task<bool> HandleAsync(
        MutateDocumentCommand command,
        CancellationToken cancellationToken)
    {
        var document = await documentRepository.GetByIdAsync(command.DocumentId, cancellationToken);
        if (document is null)
        {
            return false;
        }

        // Add random data to the document content at the end of the content
        document.AddContent(GetRandomData());

        await documentRepository.SaveAsync(document, cancellationToken);
        return true;
    }

    private string GetRandomData()
    {
        var randomString = Guid.NewGuid().ToString("N");

        var data = $"\n\nDateTime: {timeProvider.GetUtcNow():O}";
        data += $"\nRandom Data: {randomString}";
        return data;
    }
}
