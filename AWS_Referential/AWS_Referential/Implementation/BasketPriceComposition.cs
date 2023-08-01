using AWS_Referential.Enumeration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AWS_Referential.Implementation
{
    public class BasketPriceComposition
    {

        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UniqueId { get; set; }

        [ForeignKey("BasketPrice")]
        public int BasketPriceId { get; set; }
        public virtual BasketPrice BasketPrice { get; set; }


        public DateTime CompositionStartDate { get; set; }
        public DateTime? CompositionEndDate { get; set; }
        public virtual ICollection<BasketPriceComponent> Composition { get; set; }

        [Required, Column(TypeName = "decimal(38,18)")]
        public decimal Divisor { get; set; }

        #region constructucteur
        public BasketPriceComposition() { }
        public BasketPriceComposition(BasketPrice basketPrice, decimal divisor)
        {
            this.BasketPrice = basketPrice;
            this.BasketPriceId = basketPrice.UniqueId;
            this.CompositionStartDate = DateTime.Today;
            this.Composition = new List<BasketPriceComponent>();
            this.Divisor = divisor;
        }
        public BasketPriceComposition(BasketPrice basketPrice, decimal divisor, DateTime ComPositionDate)
        {
            this.BasketPrice = basketPrice;
            this.BasketPriceId = basketPrice.UniqueId;
            this.CompositionStartDate = ComPositionDate;
            this.Composition = new List<BasketPriceComponent>();
            this.Divisor = divisor;
        }
        #endregion

    }
}
