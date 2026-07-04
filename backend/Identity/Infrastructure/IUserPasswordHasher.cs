using Identity.Domain.Entities;

namespace Identity.Infrastructure;

public interface IUserPasswordHasher
{
    string HashPassword(User user, string password);

    bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
}
