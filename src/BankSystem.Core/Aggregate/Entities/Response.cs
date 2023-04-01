using System.ComponentModel.DataAnnotations;

namespace BankSystem.Core.Aggregate.Entities
{
    public class Response
    {
        public Guid RequestId { get; set; }
        [RegularExpression(@"^\d{2}$")]
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public object Data { get; set; }
    }
}
