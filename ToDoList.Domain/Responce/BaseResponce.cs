﻿using ToDoList.Domain.Enum;

namespace ToDoList.Domain.Responce
{
    public class BaseResponce<T> : IBaseResponce<T>
    {
        public string Description { get; set; }
        public StatusCode StatusCode { get; set; }
        public T Data { get; set; }
    }


    public interface IBaseResponce<T>
    {
        string Description { get;  }
        StatusCode StatusCode { get; }
        T Data { get; }
    }
}
