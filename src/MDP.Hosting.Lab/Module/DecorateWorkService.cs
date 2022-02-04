using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting.Lab
{
    public class DecorateWorkService : WorkService
    {
        // Fields
        private readonly WorkService _workService = null;


        // Constructors
        public DecorateWorkService(WorkService workService)
        {
            #region Contracts

            if (workService == null) throw new ArgumentException(nameof(workService));

            #endregion

            // Default
            _workService = workService;
        }


        // Methods
        public string GetValue()
        {
            // Return
            return _workService.GetValue() + " ZZZ";
        }
    }
}
