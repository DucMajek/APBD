using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorialController : ControllerBase
    {
        private readonly IConfiguration _config;

        public TutorialController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto dto)
        {

            if (!(dto.UserName.ToLower() == "michal" && dto.Password == "hello-world"))
            {
                return Unauthorized("Wrong username or password");
            }

            var tokenHanlder = new JwtSecurityTokenHandler();
            var tokenDescription = new SecurityTokenDescriptor
            {
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!)),
                        SecurityAlgorithms.HmacSha256
                )
            };
            var token = tokenHanlder.CreateToken(tokenDescription);
            var stringifiedToken = tokenHanlder.WriteToken(token);

            var refTokenDescription = new SecurityTokenDescriptor
            {
                Issuer = _config["JWT:RefIssuer"],
                Audience = _config["JWT:RefAudience"],
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:RefKey"]!)),
                        SecurityAlgorithms.HmacSha256
                )
            };
            var refToken = tokenHanlder.CreateToken(refTokenDescription);
            var stringifiedRefToken = tokenHanlder.WriteToken(refToken);
            return Ok(new LoginResponseDto
            {
                Token = stringifiedToken,
                RefreshToken = stringifiedRefToken
            });
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken(RefreshTokenDto dto)
        {
            var tokenHanlder = new JwtSecurityTokenHandler();
            try
            {
                tokenHanlder.ValidateToken(dto.RefreshToken, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["JWT:RefIssuer"],
                    ValidAudience = _config["JWT:RefAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:RefKey"]!))
                }, out SecurityToken validatedToken);
                return Ok(true + " " + validatedToken);
            }
            catch
            {
                return Unauthorized();
            }
        }

        //Generated password does not work with /verify-password endpoint!
        [HttpGet("hash-password/{password}")]
        public IActionResult HashPassword(string password)
        {

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                new byte[] { 0 },
                10,
                HashAlgorithmName.SHA512,
                1
            );

            return Ok(Convert.ToHexString(hash));
        }

        [HttpGet("hash-password-with-salt/{password}")]
        public IActionResult HashPasswordWithSalt(string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            return Ok(passwordHasher.HashPassword(new User(), password));
        }

        [HttpPost("verify-password")]
        public IActionResult VerifyPassword(VerifyPasswordDto dto)
        {
            var passwordHasher = new PasswordHasher<User>();
            return Ok(passwordHasher.VerifyHashedPassword(new User(), dto.Hash, dto.Password) == PasswordVerificationResult.Success);
        }

        /*[HttpPost("verify-password-with-salt")]*/
    }

    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; } = null!;
    }

    public class VerifyPasswordDto
    {
        public string Password { get; set; } = null!;
        public string Hash { get; set; } = null!;
    }

    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }

    public class User
    {
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
    }


    /*Końcówka do logowania
     * 1. ściągamy użytkownika po username
     * 2. sprawdzamy hasło z hashem z bazy danych
     * 3. w wypadku poprawnego hasła wydajemy użytkownikowi access token oraz refresh token
     */

    /*Końcówka do rejestracji
     * 1. sprawdzamy czy użtowkonim o danym username istnieje
     * 2. hashujemy hasło
     * 3. zapisujemy użytkownika do bazy danych
     */

    /*Końcówka do refreshowania sesji
     * 1. sprawdzamy czy podany przez użytkownika refresh token jest poprawny oraz wciąż "żywy"
     * 2. wydajemy nowy access token oraz refresh token
     */
}
