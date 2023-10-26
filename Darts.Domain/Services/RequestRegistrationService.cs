using Darts.Contract;
using Darts.Domain.Abstracts.Factories;
using Darts.Domain.Validation;
using System;
using RequestRegistration = Darts.Domain.DomainObjects.RequestRegistration;

namespace Darts.Domain.Services
{
   public class RequestRegistrationService : BaseService<RequestRegistration>
   {
      public RequestRegistrationService(IGenericContextFactory dbContextFactory) 
         : base(dbContextFactory, new RequestRegistrationValidator())
      {
      }

      public override RequestRegistration Get(long id)
      {
         return Get(x => x.Id == id);
      }

      public PageResult<RequestRegistration> GetRequestRegistrations(Filter pageInfo = null)
      {
         return new PageResult<RequestRegistration>
         {
            Items = GetAll(null, null, x => x.OrderBy(pageInfo, x => x.Created), pageInfo),
            Count = Count()
         };
      }

      public ValidationResult<RequestRegistration, RequestRegistrationValidationResult> Confirm(long id)
      {
         ValidationResult<RequestRegistration, RequestRegistrationValidationResult> result = new ValidationResult<RequestRegistration, RequestRegistrationValidationResult>(RequestRegistrationValidationResult.NotFound);
        
         var registration = Get(id);

         if(registration != null)
         {
            if(registration.Processed.HasValue)
            {
               result = new ValidationResult<RequestRegistration, RequestRegistrationValidationResult>(RequestRegistrationValidationResult.AlreadyProcessed);
            }
            else if (!string.IsNullOrEmpty(registration.BanReason))
            {
               result = new ValidationResult<RequestRegistration, RequestRegistrationValidationResult>(RequestRegistrationValidationResult.Banned);
            }
            else 
            {
               Transaction(r =>
               {
                  registration.Processed = DateTimeOffset.UtcNow;
                  r.Update(registration);
               });

               result = new ValidationResult<RequestRegistration, RequestRegistrationValidationResult>(registration, RequestRegistrationValidationResult.Success);
            }
         }

         return result;

      }

      public ValidationResult<RequestRegistration, RequestRegistrationValidationResult> Edit(RequestRegistration requestRegistration)
      {
         ValidationResult<RequestRegistration, RequestRegistrationValidationResult> result = new ValidationResult<RequestRegistration, RequestRegistrationValidationResult>(RequestRegistrationValidationResult.NotFound);

         var registration = Get(x => x.Email.Equals(requestRegistration.Email));

         if (registration != null)
         {
            if (registration.Processed.HasValue)
            {
               registration.Email = requestRegistration.Email;
               registration.Message = requestRegistration.Message;
               registration.FirstName = requestRegistration.FirstName;
               registration.LastName = requestRegistration.LastName;
               registration.BanReason = requestRegistration.BanReason;

               Transaction(r =>
               {
                  r.Update(registration);
               });

               result = new ValidationResult<RequestRegistration, RequestRegistrationValidationResult>(registration, RequestRegistrationValidationResult.Success);
            }
            else
            {
               result = new ValidationResult<RequestRegistration, RequestRegistrationValidationResult>(RequestRegistrationValidationResult.NotConfirmed);
            }
         }

         return result;
      }

      public ValidationResult<RequestRegistration, RequestRegistrationValidationResult> RequestRegistration(RequestRegistration requestRegistration)
      {
         ValidationResult<RequestRegistration, RequestRegistrationValidationResult> result;
         var registration = Get(x => x.Email.Equals(requestRegistration.Email));
         if (registration == null)
         {
            Transaction(r =>
            {
               requestRegistration.Created = DateTimeOffset.UtcNow;
               r.Add(requestRegistration);
            });

            result = new ValidationResult<RequestRegistration, RequestRegistrationValidationResult>(requestRegistration, RequestRegistrationValidationResult.Success);
         }
         else
         {
            if (!string.IsNullOrEmpty(registration.BanReason))
            {
               result = new ValidationResult<RequestRegistration, RequestRegistrationValidationResult>(RequestRegistrationValidationResult.Banned);
            }
            else
            {
               result = new ValidationResult<RequestRegistration, RequestRegistrationValidationResult>(RequestRegistrationValidationResult.AlreadyProcessed);
            }
         }

         return result;

      }
   }
}
