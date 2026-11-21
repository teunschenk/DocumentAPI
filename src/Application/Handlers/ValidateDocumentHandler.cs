using DocumentAPI.Application.Documents;
using DocumentAPI.Domain.Documents;

namespace DocumentAPI.Application.Handlers;

/// <summary>
/// Validate the document to see if it's really a text file. 
/// </summary>
/// <param name="documentRepository"></param>
/// <param name="timeProvider"></param>
public sealed class ValidateDocumentHandler()
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
