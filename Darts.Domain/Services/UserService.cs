using Darts.Contract;
using Darts.Domain.Abstracts;
using Darts.Domain.DomainObjects;
using System;
using System.Linq;
using System.Linq.Expressions;
using ChangePassword = Darts.Domain.DomainObjects.ChangePassword;
using User = Darts.Domain.DomainObjects.User;

namespace Darts.Domain.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPassword _password;

        public UserService(IUserRepository userRepository, IPassword password)
        {
            _userRepository = userRepository;
            _password = password;
        }

        public User Get(long id)
        {
            return _userRepository.GetSingle(x => x.Id == id);
        }

        public PageResult<User> GetAll(Filter pageInfo)
        {
            var items = _userRepository.GetAll(GetSearchExpression(pageInfo.SearchText), null, x => x.OrderBy(pageInfo, x => x.Id), pageInfo?.Skip != null ? pageInfo.Skip : null, pageInfo?.PageSize != null ? pageInfo.PageSize : null);

            return new PageResult<User>
            {
                Items = items.Where(x => x.UserRole != UserRole.SuperUser),
                Count = _userRepository.Count()
            };
        }

        public bool ChangePassword(long userId, ChangePassword changePassword)
        {
            bool result = false; 
            var user = _userRepository.GetSingle(x => x.Id == userId);
            if (user != null)
            {
                bool validPassword = _password.Verify(changePassword.OldPassword, user.Password);

                if (validPassword)
                {
                    user.Password = _password.Hash(changePassword.NewPassword);
                    _userRepository.Update(user);
                    _userRepository.Save();

                    result = true;
                }
            }

            return result;
        }

        public void Update(User user)
        {
            _userRepository.Update(user);
            _userRepository.Save();
        }

        public User Add(User user)
        {
            _userRepository.Add(user);
            _userRepository.Save();

            return user;
        }

        private static Expression<Func<User, bool>> GetSearchExpression(string search)
        {
            if (search == null)
            {
                return null;
            }

            search = search.ToLowerInvariant();
            return x => x.FirstName.ToLower().StartsWith(search) ||
                  x.LastName.ToLower().StartsWith(search) || x.Email.ToLower().StartsWith(search);

        }
    }
}
