using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AWS_Referential.Implementation
{
    public class DividendError
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UniqueId { get; set; }
        public string MarkitId { get; set; }
        public string Ticker { get; set; }
        public string Reason { get; set; }
        public DateTime InsertionDate { get; set; }

        public DividendError() { }
        public DividendError(string MarkitId, string Ticker, string Reason)
        {
            this.MarkitId = MarkitId;
            this.Ticker = Ticker;
            this.Reason = Reason;
            this.InsertionDate = DateTime.Today;
        }
    }
}
