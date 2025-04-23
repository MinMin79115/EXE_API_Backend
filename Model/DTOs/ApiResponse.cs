namespace EXE_API_Backend.Models.DTO
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponse(bool success, int statusCode, string message, T data = default)
        {
            Success = success;
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        // Phương thức tiện ích để tạo phản hồi thành công
        public static ApiResponse<T> Ok(int statusCode, T data, string message = "Request successful")
        {
            return new ApiResponse<T>(true, statusCode, message, data);
        }

        // Phương thức tiện ích để tạo phản hồi lỗi
        public static ApiResponse<T> Error(int statusCode, string message, T data = default)
        {
            return new ApiResponse<T>(false, statusCode, message, data);
        }
    }
} 