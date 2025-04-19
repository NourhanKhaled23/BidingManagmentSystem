using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepo;

        // 🔒 For demo: memory store for tokens
        private static readonly ConcurrentDictionary<string, string> _resetTokens = new();

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        // ✅ Register user
        public async Task<User> RegisterUserAsync(string fullName, string email, string password, string role)
        {
            var existingUser = await _userRepo.GetByEmailAsync(email);
            if (existingUser != null)
                throw new Exception("Email is already registered.");

            if (password.Length < 6)
                throw new Exception("Password must be at least 6 characters long.");

            var passwordHash = HashPassword(password);
            var user = new User(fullName, email, passwordHash, role);
            await _userRepo.AddAsync(user);
            return user;
        }

        // ✅ Authenticate user
        public async Task<User> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Email and password must not be empty.");

            var user = await _userRepo.GetByEmailAsync(email);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            return user;
        }

        // ✅ Send reset token (for demo: returns token directly)
        public async Task SendPasswordResetTokenAsync(string email)
        {
            var user = await _userRepo.GetByEmailAsync(email);
            if (user == null)
                return; // Don't reveal user existence

            var token = Guid.NewGuid().ToString();
            _resetTokens[email] = token;

            // 💡 In production, you would send this token via email.
            Console.WriteLine($"[ResetToken] Token for {email}: {token}");
        }

        // ✅ Reset password using token
        public async Task ResetPasswordAsync(string token, string newPassword)
        {
            var entry = _resetTokens.FirstOrDefault(x => x.Value == token);
            if (entry.Key == null)
                throw new Exception("Invalid or expired token.");

            var user = await _userRepo.GetByEmailAsync(entry.Key);
            if (user == null)
                throw new Exception("User not found.");

            var hashedPassword = HashPassword(newPassword);
            user.UpdatePassword(hashedPassword);
            await _userRepo.UpdateAsync(user);

            _resetTokens.TryRemove(entry.Key, out _); // Remove used token
        }
    }
}
