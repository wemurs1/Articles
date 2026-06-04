using Auth.Domain.Users;

namespace Auth.Domain.Events;

public record UserCreated(User user, string passwordResetToken);