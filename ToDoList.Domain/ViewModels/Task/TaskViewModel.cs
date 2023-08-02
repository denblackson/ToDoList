namespace ToDoList.Domain.ViewModels.Task
{
    public class TaskViewModel
    {
        public long Id { get; set; } 
        
        //[Display(Name = "Назва")]
        public string Name { get; set; }
        public string IsDone { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public string Created { get; set; }
    }
}
