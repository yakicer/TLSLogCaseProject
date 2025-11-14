namespace Web.UI.Models.ResponseModels
{
    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public string Response { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new();
    }

}
