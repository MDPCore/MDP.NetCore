using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleepZone.Todos.WebApp
{
    public partial class SnapshotController : Controller
    {
        // Fields
        private readonly TodoContext _todoContext = null;


        // Constructors
        public SnapshotController(TodoContext todoContext)
        {
            #region Contracts

            if (todoContext == null) throw new ArgumentException(nameof(todoContext));

            #endregion

            // Default
            _todoContext = todoContext; 
        }
    }

    public partial class SnapshotController : Controller
    {
        // Methods
        public ActionResult<FindLastnResultModel> FindLast([FromBody] FindLastActionModel actionModel)
        {
            #region Contracts

            if (actionModel == null) throw new ArgumentException(nameof(actionModel));

            #endregion

            // Return
            return (new FindLastnResultModel()
            {
                Snapshot = _todoContext.SnapshotRepository.FindLast()
            });
        }


        // Class
        public class FindLastActionModel
        {
            // Properties
           
        }

        public class FindLastnResultModel
        {
            // Properties
            public Snapshot Snapshot { get; set; }
        }
    }
}
