using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Darts.Contract;
using Darts.Domain.Abstracts;
using Darts.Domain.DomainObjects;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using User = Darts.Domain.DomainObjects.User;

namespace Darts.Api.Services
{
    public class AuthenticateService
    {
        private readonly AppSettings _appSettings;
        private readonly IUserRepository _userRepository;
        private readonly IPassword _password;

        public AuthenticateService(IOptions<AppSettings> appSettings, IUserRepository userRepository, IPassword password)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            _password = password;
        }

        public bool Register(User user)
        {
            bool result = false;
            var existingUser = _userRepository.GetUser(user.Email);

            if (existingUser == null)
            {
                user.Created = DateTime.Now;
                user.UserRole = UserRole.User;
                user.Password = _password.Hash(user.Password);
                user.IsActive = true;

                _userRepository.Add(user);
                _userRepository.Save();

                result = true;
            }

            return result;
        }

        public User AuthenticateUser(string email, string password)
        {
            User user = _userRepository.GetUser(email);

            if (user != null)
            {
                bool validPassword = _password.Verify(password, user.Password);

                if (!validPassword)
                {
                    user = null;
                }
            }

            return user;
        }

        public JwtToken Authenticate(string email, string password)
        {
            JwtToken jwtToken = null;
            var user = _userRepository.GetUser(email);

            if (user != null && user.IsActive)
            {
                bool validPassword = _password.Verify(password, user.Password);

                if (validPassword)
                {
                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));

                    List<Claim> authClaims = new List<Claim>();

                    authClaims.Add(new Claim(ClaimTypes.Sid, user.Id.ToString()));
                    authClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                    authClaims.Add(new Claim(ClaimTypes.Role, user.UserRole == UserRole.User ? Roles.UserRole : Roles.SuperUserRole));
                    authClaims.Add(new Claim(ClaimTypes.Email, email));

                    var token = new JwtSecurityToken(
                          expires: DateTime.Now.AddDays(7),
                          claims: authClaims,
                          signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                          );

                    jwtToken = new JwtToken { Token = new JwtSecurityTokenHandler().WriteToken(token), ExpiresAt = token.ValidTo };
                }
            }

            return jwtToken;
        }
    }
}
