namespace Senior_Design_Pet_Care_App.Entities
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string? Message { get; set; }

        public ServiceResponse(T? data = default, bool success = true, string? message = null)
        {
            Data = data;
            Success = success;
            Message = message;
        }
    }
}