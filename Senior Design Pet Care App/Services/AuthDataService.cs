using System.Security.Claims;
using Senior_Design_Pet_Care_App.Entities;
using Senior_Design_Pet_Care_App.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Senior_Design_Pet_Care_App.Services
{
    /// <summary>
    /// Authenticate users against the Users table in the SQLite database.
    /// </summary>
    public class AuthDataService : IAuthDataService
    {
        private readonly AuthDbContext _db;
        private readonly PasswordHasher<User> _pwHasher;

        public AuthDataService(AuthDbContext db)
        {
            _db = db;
            _pwHasher = new PasswordHasher<User>();
        }

        public async Task<ServiceResponse<ClaimsPrincipal>> LoginAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new ServiceResponse<ClaimsPrincipal>(null, false, "Email is required");
            }

            if (string.IsNullOrEmpty(password))
            {
                return new ServiceResponse<ClaimsPrincipal>(null, false, "Password is required");
            }

            var normalizedEmail = email.Trim().ToLowerInvariant();

            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);

            if (user == null)
            {
                return new ServiceResponse<ClaimsPrincipal>(null, false, "Unknown user, please sign up first");
            }

            var verificationResult = _pwHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return new ServiceResponse<ClaimsPrincipal>(null, false, "Invalid password");
            }

            // Determine role
            var role = string.IsNullOrEmpty(user.Role)
                ? "User" : user.Role;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, "jwt");
            var principal = new ClaimsPrincipal(identity);

            return new ServiceResponse<ClaimsPrincipal>(principal, true, "Login successful");
        }

        public async Task<ServiceResponse<bool>> RegisterAsync(string email, string password, string? role = null)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new ServiceResponse<bool>(false, false, "Email is required");
            }

            if (string.IsNullOrEmpty(password) || password.Length < 6)
            {
                return new ServiceResponse<bool>(false, false, "Password is required and must be at least 6 characters");
            }

            var normalizedEmail = email.Trim().ToLowerInvariant();

            var exists = await _db.Users.AnyAsync(u => u.Email.ToLower() == normalizedEmail);
            if (exists)
            {
                return new ServiceResponse<bool>(false, false, "A user with that email already exists");
            }

            var user = new User
            {
                Email = normalizedEmail,
                Role = string.IsNullOrWhiteSpace(role) ? "User" : role
            };

            user.PasswordHash = _pwHasher.HashPassword(user, password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return new ServiceResponse<bool>(true, true, "Registration successful");
        }
    }
}