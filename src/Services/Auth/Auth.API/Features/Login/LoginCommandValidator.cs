namespace Auth.API.Features.Login;

public class LoginCommandValidator : Validator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(e => e.Email).NotEmpty().EmailAddress();
        RuleFor(e => e.Password).NotEmpty();
    }
}
