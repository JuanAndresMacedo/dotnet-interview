﻿namespace TodoApi.Dtos.TodoItemDtos
{
    public class TodoItemDto
    {
        public long Id { get; set; }
        public required string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
