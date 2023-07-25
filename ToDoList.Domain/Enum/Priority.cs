using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Domain.Enum
{
    public enum Priority
    {
    // [Display(Name = "Легка")]
        Easy = 1,
        Medium = 2,
        Hard = 3
    }
}
