namespace BankSystem.Core.Aggregate.Entities
{
    public class Response
    {
        public Guid RequestId { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime Date { get; set; }
    }
}
