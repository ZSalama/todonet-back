namespace todo_back.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public Todo()
        {
            Title = "Untitled";
            Category = "Uncategorized";
        }
        
        public Todo(string title, string category, string? description = null, DateTime? dueDate = null)
        {
            Title = title;
            Category = category;
            Description = description;
            DueDate = dueDate;
        }

    }
}