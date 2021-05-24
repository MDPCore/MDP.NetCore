//using Quartz;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SleepZone.Todos.Hosting
//{
//    public class TodoContextJob : IJob
//    {
//        // Fields
//        private readonly TodoContext _todoContext = null;


//        // Constructors
//        public TodoContextJob(TodoContext todoContext)
//        {
//            #region Contracts

//            if (todoContext == null) throw new ArgumentException(nameof(todoContext));

//            #endregion

//            // Default
//            _todoContext = todoContext;
//        }


//        // Methods
//        public Task Execute(IJobExecutionContext context)
//        {
//            #region Contracts

//            if (context == null) throw new ArgumentException(nameof(context));

//            #endregion

//            // Execute
//            return Task.Run(() =>
//            {
//                // Snapshot
//                _todoContext.Snapshot();
//            });
//        }
//    }
//}
