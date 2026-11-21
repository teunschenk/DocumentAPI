using DocumentAPI.Application.Documents;
using DocumentAPI.Domain.Documents;

namespace DocumentAPI.Application.Handlers;

public sealed class ValidateDocumentHandler(
    IDocumentRepository documentRepository,
    TimeProvider timeProvider)
{
    public async Task<bool> HandleAsync(
        ValidateDocumentCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Content))
        {
            return false;
        }

        //check if the content contains binary data
        if (command.Content.Any(c => char.IsControl(c) && c != '\r' && c != '\n' && c != '\t'))
        {
            return false;
        }

        return true;
    }
}
