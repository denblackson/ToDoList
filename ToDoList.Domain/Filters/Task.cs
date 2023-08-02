
using ToDoList.Domain.Enum;

namespace ToDoList.Domain.Filters
{
    public class TaskFilter
    {
        public string Name { get; set; }
        public Priority? Priority { get; set; }
    }
}
