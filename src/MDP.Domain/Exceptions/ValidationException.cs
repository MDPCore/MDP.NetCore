using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.Domain
{
    public class ValidationException : DomainException
    {
        // Constructors
        public ValidationException(List<ValidationResult> validationResultList) : base(CreateMessage(validationResultList))
        {
            #region Contracts

            if (validationResultList == null) throw new ArgumentException($"{nameof(validationResultList)}=null");

            #endregion

            // Require
            if (validationResultList.Count == 0) throw new ArgumentException($"{nameof(validationResultList)}.Count=0");

            // Default
            this.ValidationResultList = validationResultList;
        }


        // Properties
        public List<ValidationResult> ValidationResultList { get; private set; }


        // Methods
        private static string CreateMessage(List<ValidationResult> validationResultList)
        {
            #region Contracts

            if (validationResultList == null) throw new ArgumentException($"{nameof(validationResultList)}=null");

            #endregion

            // MessageDictionary
            var messageDictionary = validationResultList
                .SelectMany(e => e.MemberNames.Select(name => new { name, e.ErrorMessage }))
                .GroupBy(x => x.name)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToList());

            // Return
            return JsonSerializer.Serialize(messageDictionary);
        }
    }
}
