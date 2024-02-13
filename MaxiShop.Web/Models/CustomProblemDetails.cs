namespace MaxiShop.Web.Models
{
    public class CustomProblemDetails
    {
        internal int status;

        public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
        public string Title { get; internal set; }
        public string Type { get; internal set; }
        public string Details { get; internal set; }
    }
}
