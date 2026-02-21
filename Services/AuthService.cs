using OBeefSoup.Data;
using OBeefSoup.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace OBeefSoup.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        // Session keys
        public const string SESSION_USER_ID = "AdminUserId";
        public const string SESSION_USER_ROLE = "AdminUserRole";
        public const string SESSION_USER_NAME = "AdminUserName";
        public const string SESSION_FULL_NAME = "AdminFullName";

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password + "OBeefSoup_Salt_2026");
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }

        public static bool VerifyPassword(string plainPassword, string storedHash)
        {
            var hash = HashPassword(plainPassword);
            return hash.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<AdminUser?> ValidateLoginAsync(string email, string password)
        {
            var user = await _context.AdminUsers
                .FirstOrDefaultAsync(u => (u.Email == email || u.Username == email) && u.IsActive);

            if (user == null) return null;
            if (!VerifyPassword(password, user.PasswordHash)) return null;

            return user;
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.AdminUsers.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.AdminUsers.AnyAsync(u => u.Email == email);
        }

        public async Task<AdminUser> CreateUserAsync(string username, string email, string password, string role, string fullName)
        {
            var user = new AdminUser
            {
                Username = username,
                Email = email,
                PasswordHash = HashPassword(password),
                Role = role,
                FullName = fullName,
                CreatedDate = DateTime.Now,
                IsActive = true
            };
            _context.AdminUsers.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<AdminUser>> GetAllUsersAsync()
        {
            return await _context.AdminUsers.OrderBy(u => u.Role).ThenBy(u => u.Username).ToListAsync();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.AdminUsers.FindAsync(id);
            if (user == null) return false;
            _context.AdminUsers.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleUserStatusAsync(int id)
        {
            var user = await _context.AdminUsers.FindAsync(id);
            if (user == null) return false;
            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
