namespace Infrastructure.BusinessObject
{
    public class ValidationModel<T>
    {
        public T Data { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }
}
