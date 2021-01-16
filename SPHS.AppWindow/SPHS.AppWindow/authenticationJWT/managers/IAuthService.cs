﻿using System.Security.Claims;
using System.Collections.Generic;
using AuthenticationService.Models;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AuthenticationService.Managers
{
    public interface IAuthService
    {
        string SecretKey { get; set; }

        bool IsTokenValid(string token);
        string GenerateToken();
        IEnumerable<Claim> GetTokenClaims(string token);
    }

    public class JWTService : IAuthService
    {
        #region Members
        /// <summary>
        /// The secret key we use to encrypt out token with.
        /// </summary>
        public string SecretKey { get; set; }
        #endregion

        #region Constructor
        public JWTService()
        {
            SecretKey = "SGF0IERhdSBOaG8gLSBNYW5jaGVzdGVyIFVuaXRlZA==";
        }
        #endregion

        #region Public Methods
        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Given token is null or empty.");

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GenerateToken()
        {
            IAuthContainerModel model = GetJWTContainerModel("admin", "admin", "5efaa4649af6b52540cd855a");

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(model.Claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(model.ExpireMinutes)),
                SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(), model.SecurityAlgorithm)
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            string token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return token;
        }

        public static JWTContainerModel GetJWTContainerModel(string account, string role, string companyId)
        {
            DateTimeOffset time = DateTime.Now;
            return new JWTContainerModel()
            {
                Claims = new Claim[]
                {
                    new Claim("account", account),
                    new Claim("role", role),
                    new Claim("companyId", companyId),
                    new Claim("time", time.ToUnixTimeMilliseconds().ToString()),
                }
            };
        }

        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Given token is null or empty.");

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return tokenValid.Claims;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Private Methods
        private SecurityKey GetSymmetricSecurityKey()
        {
            byte[] symmetricKey = Convert.FromBase64String(SecretKey);
            return new SymmetricSecurityKey(symmetricKey);
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = GetSymmetricSecurityKey()
            };
        }
        #endregion
    }
}