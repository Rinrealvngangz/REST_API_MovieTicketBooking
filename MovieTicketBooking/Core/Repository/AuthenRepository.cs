using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.IRepository;
using Dtos;
using MovieTicketBookingAPI.Data.Entities;
using MovieTicketBookingAPI.Data;
using Core.Config;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Utilities.Exceptions;
namespace Core.Repository
{
   public class AuthenRepository : GenericRepository<RefreshToken> , IAuthenRepository
    {
        private readonly JwtConfig _jwtConfig;
        private readonly UserManager<User> _userManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private  List<Claim>Claim =null;
        public AuthenRepository(AppDbContext appDbContext,
                                IOptionsMonitor<JwtConfig> optionsMonitor,
                                UserManager<User> userManager,
                                TokenValidationParameters tokenValidationParameters) :base(appDbContext)
        {
            _jwtConfig = optionsMonitor.CurrentValue;
            _userManager = userManager;
            _tokenValidationParameters = tokenValidationParameters;
        }
     
        public async Task<RefreshTokenDtos> GenerateToken(User user)
        {
            var key = _jwtConfig.KeySecret;
            var roles = await _userManager.GetRolesAsync(user);
            var jwtHandler = new JwtSecurityTokenHandler();
          
    

             Claim = new List<Claim>
            {
                new Claim("ID",user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                
            };
            foreach (var item in roles)
            {
                Claim.Add(new Claim(ClaimTypes.Role, item));
            }
            
            var ClaimIdentity = new ClaimsIdentity(Claim);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = ClaimIdentity,
                Expires = DateTime.UtcNow.AddSeconds(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha256)
            };

            var JwtSecurityToken = jwtHandler.CreateToken(tokenDescriptor);

            var JwtToken = jwtHandler.WriteToken(JwtSecurityToken);

            var refreshToken = new RefreshToken
            {
                JwtId = JwtSecurityToken.Id,
                UserId = user.Id,
                IsUsed = false,
                IsRevorked = false,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Token = RandomString(35) + Guid.NewGuid()
            };
             await _dbSet.AddAsync(refreshToken);
            var refreshTokenDtos = new RefreshTokenDtos
            {
                Success = true,
                Token = JwtToken,
                RefreshToken = refreshToken.Token
            };

            return refreshTokenDtos;
        }

        public async Task<RefreshTokenDtos> VerifyRefreshToken(RefreshTokenDtos refreshToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidate = tokenHandler.ValidateToken(refreshToken.Token, _tokenValidationParameters, out SecurityToken securityToken);

                if (securityToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.CurrentCultureIgnoreCase);
                    if (result == false)
                    {
                        return null;
                    }

                }

                var utcExpryDate = long.Parse(tokenValidate.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var verifyDate = UnixTimeStampToDateTime(utcExpryDate);
                if (verifyDate > DateTime.UtcNow)
                {
                    return new RefreshTokenDtos
                    {
                        Success = false,
                        Erros = new List<string>() {
                        "Token has not yet expired"
                    }
                    };
                }

                var storedToken = await _dbSet.FirstOrDefaultAsync(x => x.Token == refreshToken.RefreshToken);

                if (storedToken == null)
                {
                    return new RefreshTokenDtos
                    {
                        Success = false,
                        Erros = new List<string>() {
                        "Token is not exist"
                    }
                    };
                }
                if (storedToken.IsUsed)
                {
                    return new RefreshTokenDtos
                    {
                        Success = false,
                        Erros = new List<string>() {
                        "Token has been used"
                    }
                    };
                }
                if (storedToken.IsRevorked)
                {
                    return new RefreshTokenDtos
                    {
                        Success = false,
                        Erros = new List<string>() {
                        "Token has been revoked"
                    }
                    };
                }
                var jti = tokenValidate.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return new RefreshTokenDtos
                    {
                        Success = false,
                        Erros = new List<string>() {
                        "Token is not matched"
                    }
                    };
                }

                // update current token 
                var newStoredToken = storedToken with
                {
                    IsUsed = true,

                };
              _dbSet.Update(newStoredToken);

                var user = await _userManager.FindByIdAsync(newStoredToken.UserId.ToString());
             
               return await GenerateToken(user);
            }
            catch(Exception er)
            {
                throw new MovieTicketBookingExceptions(er.Message);
            }
          

        }
        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }
        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }
}
