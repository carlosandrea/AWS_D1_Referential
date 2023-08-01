using AWS_Referential.Enumeration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AWS_Referential.Implementation
{
    public class Derivative : Instrument
    {
        public virtual Listing Listing { get; set; }
        public virtual Instrument Underlying { get; set; }
        public DerivativeType DerivativeType { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Double PointValue { get; set; }
        public virtual ICollection<Modification> Modifications { get; set; }

        public Derivative() { }

        public Derivative(string Name, string Isin, string Figi, string Cusip, Listing Listing, Instrument Underlying, DerivativeType DerivativeType, DateTime ExpirationDate, double PointValue)
        {
            this.Name = Name;
            this.Isin = Isin;
            this.Figi = Figi;
            this.Cusip = Cusip;
            this.ActivationDate = DateTime.Today;
            this.Listing = Listing;
            this.Underlying = Underlying;
            this.DerivativeType = DerivativeType;
            this.ExpirationDate = ExpirationDate;
            this.PointValue = PointValue;
            this.Modifications = new List<Modification>();

        }

        public Derivative(string Name, string Isin, String Figi, String Cusip)
        {
            this.Name = Name;
            this.Isin = Isin;
            this.Figi = Figi;
            this.Cusip = Cusip;
            this.ActivationDate = DateTime.Today;
            this.Modifications = new List<Modification>();


        }
    }
}
