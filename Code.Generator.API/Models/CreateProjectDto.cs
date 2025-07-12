namespace Code.Generator.API.Models
{
    public class CreateProjectDto
    {
        public string Name { get; set; }      
        public string Requirements { get; set; }
        public string? Description { get; set; }
    }
}
