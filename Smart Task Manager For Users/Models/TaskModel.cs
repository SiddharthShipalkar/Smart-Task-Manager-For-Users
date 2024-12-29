using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Task_Manager_For_Users.Models
{
    public class TaskModel
    {

        public Guid Id { get; set; }  
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string Category { get; set; }

        public string TaskPriority { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
