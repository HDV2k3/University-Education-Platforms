namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.BaseResponse
{
    public class ObjectResponse
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public ObjectResponse()
        {
        }

        public ObjectResponse(bool succeeded, string? message, object? data = null)
        {
            Succeeded = succeeded;
            Message = message;
            Data = data;
        }
    }
}
