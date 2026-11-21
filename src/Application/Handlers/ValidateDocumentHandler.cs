using DocumentAPI.Application.Documents;

namespace DocumentAPI.Application.Handlers;

/// <summary>
/// Validate the document to see if it's really a text file. 
/// </summary>
public sealed class ValidateDocumentHandler()
{
    public bool Handle(ValidateDocumentCommand command)
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
