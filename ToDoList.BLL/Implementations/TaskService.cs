using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoList.DAL.Interfaces;
using ToDoList.Domain.Entity;
using ToDoList.Domain.Enum;
using ToDoList.Domain.Extentions;
using ToDoList.Domain.Filters;
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

        public async Task<IBaseResponce<IEnumerable<TaskViewModel>>> GetTasks(TaskFilter filter)
        {
            try
            {
                var tasks = await _taskRepository.GetAll()
                    .WhereIf(!string.IsNullOrWhiteSpace(filter.Name), x => x.Name == filter.Name)
                    .WhereIf(filter.Priority.HasValue, x => x.Priority == filter.Priority)
                    .Select(x => new TaskViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        IsDone = x.IsDone == true ? "Finished" : "Not finished",
                        //Priority = x.Priority.GetDisplayName(),
                        Priority = x.Priority.ToString(),

                        Created = x.Created.ToLongDateString(),
                    })
                    .ToListAsync();


                return new BaseResponce<IEnumerable<TaskViewModel>>()
                {
                    Data = tasks,
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"[TaskService.GetTasks]: {e.Message}");

                return new BaseResponce<IEnumerable<TaskViewModel>>()
                {
                    Description = $"{e.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
