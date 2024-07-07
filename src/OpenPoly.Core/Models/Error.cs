namespace OpenPoly.Core.Models;

public sealed record Error(string error, string error_message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}
