using ToDoList.Domain.Entity;
using ToDoList.Domain.Responce;
using ToDoList.Domain.ViewModels.Task;

namespace ToDoList.Sevice.Interfaces
{
    public interface ITaskService
    {
        Task<IBaseResponce<TaskEntity>> Create(CreateTaskViewModel model);
        Task<IBaseResponce<IEnumerable<TaskViewModel>>> GetTasks();
    }

   
}
