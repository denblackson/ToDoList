using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoList.DAL.Interfaces;
using ToDoList.Domain.Entity;
using ToDoList.Domain.Enum;
using ToDoList.Domain.Responce;
using ToDoList.Domain.ViewModels.Task;
using ToDoList.Sevice.Interfaces;

namespace ToDoList.Sevice.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly IBaseRepository<TaskEntity> _taskRepository;
        private ILogger<TaskService> _logger;

        public TaskService(IBaseRepository<TaskEntity> taskRepository, ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }


        public async Task<IBaseResponce<TaskEntity>> Create(CreateTaskViewModel model)
        {
            try
            {
                model.Validate();

                _logger.LogInformation($"Task creation query - {model.Name}");
                var task = await _taskRepository.GetAll()
                    .Where(x => x.Created.Date == DateTime.Today)
                    .FirstOrDefaultAsync(x => x.Name == model.Name);


                if (task != null)
                {
                    return new BaseResponce<TaskEntity>()
                    {
                        Description = "The Task with this description is already exists",
                        StatusCode = StatusCode.TaskAlreadyExists,
                    };
                }

                task = new TaskEntity()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Priority = model.Priority,
                    Created = DateTime.Now,
                    IsDone = false
                };
                await _taskRepository.Create(task);


                return new BaseResponce<TaskEntity>()
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Task created successfully"
                };

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"[TaskService.Create]: {e.Message}");

                return new BaseResponce<TaskEntity>()
                {
                    Description = $"{e.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
