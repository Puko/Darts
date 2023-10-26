using Darts.Contract;
using System;

namespace Darts.Api.Extensions
{
    public static class ValidationResultExtensions
    {
        public static ValidationResult<TContract, TEnum> ToContract<TDomain, TEnum, TContract>(this ValidationResult<TDomain, TEnum> result, Func<TDomain, TContract> mapper) 
            where TDomain : class
            where TContract : class
        {
            if(result.Entity != null)
            {
                return new ValidationResult<TContract, TEnum>(mapper(result.Entity), result.Validation);
            }
            else 
            {
                return new ValidationResult<TContract, TEnum>(result.Validation);
            }
        }
    }
}
