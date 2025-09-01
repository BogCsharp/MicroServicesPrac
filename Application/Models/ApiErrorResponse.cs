namespace Web.Models
{
    public class ApiErrorResponse
    {
        public  required string Message {  get; set; }
        public required int Code { get; set; }
        public string? Description { get; set; }
    }
}
