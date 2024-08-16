namespace DirectoryOfNaturalPersons.Domain.Exceptions;

public class NotFoundException : Exception
{
    public bool ShowMessage { get; set; }

    public NotFoundException(string message, bool showMessage) : base(message)
    {
        ShowMessage = showMessage;
    }
}