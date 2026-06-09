using Blocks.Core;
using Blocks.Domain.ValueObjects;

namespace Auth.Domain.Persons.ValueObjects;

public class HonourificTitle : StringValueObject
{
    private HonourificTitle(string value) => Value = value;

    public static HonourificTitle FromString(string honourific)
    {
        Guard.ThrowIfNullOrWhiteSpace(honourific);
        return new HonourificTitle(honourific.Trim());
    }

    public static HonourificTitle? FromEnum(Honourific? honourific)
    {
        if (honourific.HasValue) return new HonourificTitle(honourific!.ToString()!);

        return null;
    }
}
