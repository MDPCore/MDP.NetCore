using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Domain
{
    public static class Validator
    {
        // Methods
        public static void ValidateObject(object @object, IServiceProvider serviceProvider = null, IDictionary<object, object> items = null)
        {
            #region Contracts

            if (@object == null) throw new ArgumentException($"{nameof(@object)}=null");

            #endregion

            // ValidateObject
            var validationResultList = new List<ValidationResult>();
            if (System.ComponentModel.DataAnnotations.Validator.TryValidateObject(@object, new ValidationContext(@object, serviceProvider: serviceProvider, items: items), validationResultList, true) == false)
            {
                throw new MDP.Domain.ValidationException(validationResultList);
            }
        }

        public static void ValidateObject(object @object, ValidationContext validationContext)
        {
            #region Contracts

            if (@object == null) throw new ArgumentException($"{nameof(@object)}=null");
            if (validationContext == null) throw new ArgumentException($"{nameof(validationContext)}=null");

            #endregion

            // ValidateObject
            var validationResultList = new List<ValidationResult>();
            if (System.ComponentModel.DataAnnotations.Validator.TryValidateObject(@object, validationContext, validationResultList, true) == false)
            {
                throw new MDP.Domain.ValidationException(validationResultList);
            }
        }
    }
}
