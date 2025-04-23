using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EXE_API_Backend.Repositories.Interface;
using System.Security.Cryptography;
using EXE_API_Backend.Models.DTO;
using EXE_API_Backend.Models.Model;
using EXE_API_Backend.Services;

namespace EXE_API_Backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IEmailService _emailService;

        public AuthController(IConfiguration configuration, IUnitOfWork unitOfWork, IRefreshTokenService refreshTokenService, IEmailService emailService)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _refreshTokenService = refreshTokenService;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            var user = await _unitOfWork.UserRepository.GetByUsernameAsync(model.userName);
            if (user != null && VerifyPassword(model.password, user.passwordHash))
            {
                var token = GenerateJwtToken(user.userName, user.role.ToString());
                var refreshToken = await _refreshTokenService.GenerateRefreshToken(user.userName);
                return StatusCode(200, ApiResponse<object>.Ok(200, new { AccessToken = token, RefreshToken = refreshToken }, "Login successful"));
            }

            return StatusCode(401, ApiResponse<string>.Error(401, "Invalid username or password"));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            var existingUser = await _unitOfWork.UserRepository.GetByUsernameAsync(model.userName);
            if (existingUser != null)
            {
                return StatusCode(400, ApiResponse<string>.Error(400, "Username already exists"));
            }

            var existingEmail = await _unitOfWork.UserRepository.GetByEmailAsync(model.email);
            if (existingEmail != null)
            {
                return StatusCode(400, ApiResponse<string>.Error(400, "Email already exists"));
            }

            var user = new Models.Model.User
            {
                userName = model.userName,
                passwordHash = HashPassword(model.password),
                email = model.email,
                fullName = model.fullName
            };

            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return StatusCode(201, ApiResponse<string>.Ok(201, null, "User registered successfully"));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var isValid = await _refreshTokenService.ValidateRefreshToken(request.UserId, request.RefreshToken);
            if (!isValid)
            {
                return StatusCode(401, ApiResponse<string>.Error(401, "Invalid refresh token"));
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return StatusCode(401, ApiResponse<string>.Error(401, "User not found"));
            }

            var newAccessToken = GenerateJwtToken(user.userName, user.role.ToString());
            var newRefreshToken = await _refreshTokenService.GenerateRefreshToken(request.UserId);
            await _refreshTokenService.RevokeRefreshToken(request.UserId, request.RefreshToken);
            return StatusCode(200, ApiResponse<object>.Ok(200, new { AccessToken = newAccessToken, RefreshToken = newRefreshToken }, "Token refreshed successfully"));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
        {
            await _refreshTokenService.RevokeRefreshToken(request.UserId, request.RefreshToken);
            return StatusCode(200, ApiResponse<string>.Ok(200, null, "Logged out successfully"));
        }

        private string GenerateJwtToken(string username, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == passwordHash;
        }
    }

    public class RefreshTokenRequest
    {
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
    }
} 