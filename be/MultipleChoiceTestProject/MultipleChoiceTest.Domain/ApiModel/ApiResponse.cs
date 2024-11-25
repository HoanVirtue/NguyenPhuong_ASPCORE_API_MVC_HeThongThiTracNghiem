namespace MultipleChoiceTest.Domain.ApiModel
{
    // Class to hold the response
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }

        public ApiResponse()
        {
            Success = true;
        }
        public static ApiResponse<T> SuccessWithData<T>(T data, string? message = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        // Success response without data
        public static ApiResponse<T> SuccessWithData<T>(string? message = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = default,
                Message = message
            };
        }

        // Error response
        public static ApiResponse<T> ErrorResponse<T>(string? message, T? data = default)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = data,
                Message = message
            };
        }
    }
}
