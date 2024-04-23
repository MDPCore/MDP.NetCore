using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLab.Todos
{
    public class Todo
    {
        // Properties
        public string TodoId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;
    }
}
