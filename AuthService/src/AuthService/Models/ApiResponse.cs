namespace AuthService.Models
{
    public class ApiResponse<T>
    {
        public string Method { get; set; }
        public string Url { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse(string method, string url, int statusCode, string message, T data = default)
        {
            Method = method;
            Url = url;
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        public static ApiResponse<T> Success(string method, string url, int statusCode, T data, string message = "Success")
        {
            return new ApiResponse<T>(method, url, statusCode, message, data);
        }

        public static ApiResponse<T> Error(string method, string url, int statusCode, string message)
        {
            return new ApiResponse<T>(method, url, statusCode, message);
        }
    }
}
