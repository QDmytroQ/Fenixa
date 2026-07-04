using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure;

public sealed class UserPasswordHasher : IUserPasswordHasher
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string HashPassword(User user, string password) =>
        _passwordHasher.HashPassword(user, password);
}
