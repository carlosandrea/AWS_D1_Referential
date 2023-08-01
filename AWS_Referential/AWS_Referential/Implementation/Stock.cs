using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AWS_Referential.Implementation
{
    public class Stock : Instrument
    {
        public virtual ICollection<Listing> Listings { get; set; }
        public virtual ICollection<Dividend> Dividends { get; set; }

        public Stock() { }
        public Stock(string Name, string Isin, String Figi, String Cusip)
        {
            this.Name = Name;
            this.Isin = Isin;
            this.Figi = Figi;
            this.Cusip = Cusip;
            this.ActivationDate = DateTime.Today;
            this.Listings = new List<Listing>();
            this.Dividends = new List<Dividend>();
        }
    }
}
