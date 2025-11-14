namespace Web.Blazor.Models.ResponseModel
{
    public class BaseResponse<T>
    {
        public BaseResponse()
        {
            Errors = new List<string>();
        }
        public bool Success { get; set; }
        public string Response { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }

    }
}
