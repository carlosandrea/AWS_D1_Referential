using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AWS_Referential.Implementation
{
    public class EarningEstimate
    {
        [Key, Column(Order = 0), ForeignKey("Stock")]
        public int UniqueId { get; set; }
        public virtual Stock Stock { get; set; }
        public double? BEST_EPS { get; set; }
       



        public DateTime Last_Update { get; set; }

        public EarningEstimate() { }
        public EarningEstimate(Stock stock, double BEST_EPS,dateteTime Last_Update)
        {
            this.Stock = stock;
            this.BEST_EPS = BEST_EPS;
           




            this.Last_Update = Last_Update;

        }

    }
}
