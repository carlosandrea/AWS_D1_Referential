using AWS_Referential.Enumeration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace AWS_Referential.Implementation
{
    public class Price
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UniqueId { get; set; }

        [ForeignKey("Listing")]
        public int ListingId { get; set; }
        public virtual Listing Listing { get; set; }

        public string Source { get; set; }
        public PriceType PriceType { get; set; }
        public DateTime Date { get; set; }
        public Double Value { get; set; }
        public Double? Quantity { get; set; }

        public Price() { }

        public Price(Listing Listing, String Source, PriceType PriceType, DateTime Date, Double Value, double quantity = 0)
        {
            this.Listing = Listing;
            this.ListingId = Listing.UniqueId;
            this.Source = Source;
            this.PriceType = PriceType;
            this.Date = Date;
            this.Value = Value;
            this.Quantity = quantity;
        }
    }
}

