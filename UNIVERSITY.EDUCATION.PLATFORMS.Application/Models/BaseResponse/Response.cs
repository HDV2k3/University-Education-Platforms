namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.BaseResponse
{
    public class Response<T>
    {
        public Response()
        {
            Succeeded = true;
            Errors = new List<string>();
        }

        public Response(T data, string? message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            Errors = new List<string>();
        }

        public Response(string? message)
        {
            Succeeded = true;
            Message = message;
            Errors = new List<string>();
        }

        public Response(bool succeed, string? message)
        {
            Succeeded = succeed;
            Message = message;
            Errors = new List<string>();
        }

        public Response(bool succeed, string? message, List<string>? errors)
        {
            Succeeded = succeed;
            Message = message;
            Errors = errors ?? new List<string>();
        }

        public Response(T data, bool succeed, string? message)
        {
            Data = data;
            Succeeded = succeed;
            Message = message;
            Errors = new List<string>();
        }

        public Response(T data, bool succeed, string? message, List<string>? errors)
        {
            Data = data;
            Succeeded = succeed;
            Message = message;
            Errors = errors ?? new List<string>();
        }

        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new();
        public T? Data { get; set; }
        public string? Title { get; set; }
    }
}
