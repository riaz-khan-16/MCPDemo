public class User
        {
            public string id { get; set; }
            public string email { get; set; }
            public string role { get; set; }
            public string password {set; get;}
            public string department { get; set; }
            
            public string name { get; set; }
        }


public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;

    public List<string> Members { get; set; } = new();
    public List<TaskItem> Tasks { get; set; } = new();
    public List<string> UserIds { get; set; } = new();
}


public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Assignee { get; set; } = string.Empty;

    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public Guid ProjectId { get; set; }
    public string Project { get; set; } = string.Empty;
}
