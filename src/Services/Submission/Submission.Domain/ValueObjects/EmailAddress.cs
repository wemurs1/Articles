using System.Text.RegularExpressions;

namespace Submission.Domain.ValueObjects;

public class EmailAddress : StringValueObject
{
    private EmailAddress(string value) => Value = value;

    public static EmailAddress Create(string value)
    {
        Guard.ThrowIfNullOrWhiteSpace(value);
        if (!IsValidEmail(value)) throw new ArgumentException("Invalid email format");
        return new EmailAddress(value);
    }

    private static bool IsValidEmail(string email)
    {
        const string emailRegEx = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailRegEx, RegexOptions.IgnoreCase);
    }
}
