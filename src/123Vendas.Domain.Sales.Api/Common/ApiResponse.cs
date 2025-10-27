namespace _123Vendas.Domain.Sales.Api.Common
{
    public class ApiResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;
    }

    public class ApiResponseWithData<T> : ApiResponse
    {
        public T? Data { get; set; }
    }
}
