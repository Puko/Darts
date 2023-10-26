using Darts.Contract;

namespace Darts.Api.Mappers
{
    public static class UserMapper
    {
        public static User Map(Domain.DomainObjects.User user)
        {
            var result = Mapper.From<Domain.DomainObjects.User, User>(user);
            result.Password = null;
            result.IsSuperUser = user.UserRole == Domain.DomainObjects.UserRole.SuperUser;

            return result;
        }
    }
}
