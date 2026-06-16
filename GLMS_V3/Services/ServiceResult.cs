namespace GLMS.Services
{
    public class ServiceResult
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public static ServiceResult Ok()
        {
            return new ServiceResult
            {
                Success = true
            };
        }

        public static ServiceResult Fail(string message)
        {
            return new ServiceResult
            {
                Success = false,
                Message = message
            };
        }
    }
}