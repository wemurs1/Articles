using System.Text.RegularExpressions;
using Blocks.Core;
using Blocks.Domain.ValueObjects;

namespace Auth.Domain.Persons.ValueObjects;

public class EmailAddress : StringValueObject
{
    private EmailAddress(string value)
    {
        Value = value;
        NormalisedEmail = value.ToUpperInvariant();
    }

    public string NormalisedEmail { get; internal set; }

    public static EmailAddress Create(string value)
    {
        Guard.ThrowIfNullOrWhiteSpace(value);
        Guard.ThrowIfFalse(IsValidEmail(value), "Invalid email format");
        return new EmailAddress(value);
    }

    private static bool IsValidEmail(string email)
    {
        const string emailRegEx = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailRegEx, RegexOptions.IgnoreCase);
    }

    public static implicit operator EmailAddress(string value) => Create(value);

    public static implicit operator string(EmailAddress email) => email.Value;

    public override int GetHashCode() => NormalisedEmail.GetHashCode();
}
