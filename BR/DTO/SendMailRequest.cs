namespace BR.DTO
{
    public class SendMailRequest
    {
        public int RecipentId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
