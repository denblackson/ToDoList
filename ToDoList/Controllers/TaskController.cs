﻿using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.ViewModels.Task;
using ToDoList.Sevice.Interfaces;

namespace ToDoList.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskViewModel model)
        {
            var responce = await _taskService.Create(model);

            if (responce.StatusCode == Domain.Enum.StatusCode.Ok)
            {
                return Ok(new { description = responce.Description });
            }


            return BadRequest(new { description = responce.Description });
        }


        public async Task<IActionResult> TaskHandler()
        {
            var response = await _taskService.GetTasks();
            return Json(new { data = response.Data });
        }

    }
}