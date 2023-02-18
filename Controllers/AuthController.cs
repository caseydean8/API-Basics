using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Controllers
{
    // class Microsoft.AspNetCore.Authorization.AuthorizeAttribute (+ 2 overloads)
    // Specifies that the class or method that this attribute is applied to requires the specified authorization.
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _config = config;
        }

        // class Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute (+ 1 overload)
        // Specifies that the class or method that this attribute is applied to does not require authorization\.
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                string sqlCheckUserExists = "SELECT * FROM TutorialAppSchema.Auth WHERE Email = '" +
                  userForRegistration.Email + "'";
                IEnumerable<string> existingUsers = _dapper.LoadData<string>(sqlCheckUserExists);
                if (existingUsers.Count() == 0)
                {
                    byte[] passwordSalt = new byte[128 / 8];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }

                    byte[] passwordHash = GetPasswordHash(userForRegistration.Password, passwordSalt);

                    string sqlAddAuth = @"
                        INSERT INTO TutorialAppSchema.Auth
                            ([Email],
                            [PasswordHash],
                            [PasswordSalt]) 
                        VALUES
                            ('" + userForRegistration.Email +
                            "', @PasswordHash, @PasswordSalt)";

                    List<SqlParameter> sqlParameters = new List<SqlParameter>();

                    // Create passwordSalt
                    // SqlParameter.SqlParameter(string parameterName, SqlDbType dbType) (+ 6 overloads)
                    // Initializes a new instance of the `SqlParameter` class that uses the parameter name and the data type\.
                    SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                    passwordSaltParameter.Value = passwordSalt;

                    // Create passwordHash
                    SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                    passwordHashParameter.Value = passwordHash;

                    // Add passwordSalt and Hash
                    sqlParameters.Add(passwordSaltParameter);
                    sqlParameters.Add(passwordHashParameter);

                    // bool DataContextDapper.ExecuteSqlWithParameters(string sql, List<SqlParameter> parameters)
                    if (_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
                    {
                        string sqlAddUser = @"
                        INSERT INTO TutorialAppSchema.Users
                            (
                            [FirstName],
                            [LastName],
                            [Email],
                            [Gender],
                            [Active])
                        VALUES
                            (
                             '" + userForRegistration.FirstName +
                             "', '" + userForRegistration.LastName +
                             "', '" + userForRegistration.Email +
                             "', '" + userForRegistration.Gender +
                             "',1)";
                        if (_dapper.ExecuteSql(sqlAddUser))
                        {
                            return Ok();
                        }
                        throw new Exception("Failed to Add User");
                    }
                    throw new Exception("Failed to Register User");
                }
                throw new Exception("User with this email already exists!");
            }
            throw new Exception("Passwords do not match!");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            string sqlForHashAndSalt = @"
                SELECT
                    [PasswordHash],
                    [PasswordSalt]
                FROM TutorialAppSchema.Auth
                WHERE Email='" + userForLogin.Email + "'";

            UserForLoginConfirmationDto userForConfirmation = _dapper
              .LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt);

            byte[] passwordHash = GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);

            // if (passwordHash == userForConfirmation.PasswordHash) // won't work
            for (int index = 0; index < passwordHash.Length; index++)
            {
                if (passwordHash[index] != userForConfirmation.PasswordHash[index])
                {
                    return StatusCode(401, "Incorrect password");
                }
            }

            string userIdSql = @"
                SELECT UserId FROM TutorialAppSchema.Users
                WHERE Email = '" +
                userForLogin.Email + "'";

            int userId = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string> {
                {"token", CreateToken(userId)}
                });
        }

        [HttpGet("RefreshToken")]
        public string RefreshToken()
        {
            string userIdSql = @"
                SELECT UserId FROM TutorialAppSchema.Users
                WHERE UserId = '" +
                 User.FindFirst("userId")?.Value + "'";

            int userId = _dapper.LoadDataSingle<int>(userIdSql);
            return CreateToken(userId);
        }

        private byte[] GetPasswordHash(string password, byte[] passwordSalt)
        {
            // Add PasswordKey to passwordSalt
            string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value +
              Convert.ToBase64String(passwordSalt);

            return KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
                );
        }

        private string CreateToken(int userId)
        {
            // I don't know why this isn't colored properly
            Claim[] claims = new Claim[] {
                new Claim("userId", userId.ToString())
            };

            string? tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;

            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        tokenKeyString != null ? tokenKeyString : ""
                    )
                );

            SigningCredentials credentials = new SigningCredentials(
                    tokenKey,
                    SecurityAlgorithms.HmacSha512Signature
                );

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(1)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}












