using Darts.Contract;
using System;
using System.Linq;

namespace Darts.Api.Extensions
{
    public static class PageResultExtensions
    {
        public static PageResult<TContract> ToContract<TContract, TDomain>(this PageResult<TDomain> domainResult, Func<TDomain, TContract> mapper)
        {
            return new PageResult<TContract>
            {
                Count = domainResult.Count,
                Items = domainResult.Items.Select(x => mapper(x))
            };
        }
    }
}
