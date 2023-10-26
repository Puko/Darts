using System;
namespace Darts.Api.Mappers
{
    public static class Mapper
    {
        public static TContract From<TDomain, TContract>(TDomain domain) where TContract : class, new()
        {
            var contract = Activator.CreateInstance<TContract>();
            var publicProperties = domain.GetType().GetProperties(
                System.Reflection.BindingFlags.Public
               | System.Reflection.BindingFlags.Instance);

            foreach (var item in publicProperties)
            {
                var info = contract.GetType().GetProperty(item.Name);
                if (info != null && info.CanWrite)
                {
                    info.SetValue(contract, item.GetValue(domain));
                }
            }

            return contract;
        }
    }
}
