using AWS_Referential.Enumeration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AWS_Referential.Implementation
{
    public class BasketPrice : Instrument
    {
        public virtual Listing Listing { get; set; }

        public IndexReturnType IndexReturnType { get; set; }

        public virtual ICollection<BasketPriceComposition> PriceComposition { get; set; }
        public virtual ICollection<Modification> Modifications { get; set; }

        #region constructeur
        public BasketPrice() { }

        public BasketPrice(string Name, string Isin, string Figi, string Cusip, Listing listing, IndexReturnType returnType, BasketPriceComposition basketPriceComposition)
        {
            this.Name = Name;
            this.Isin = Isin;
            this.Figi = Figi;
            this.Cusip = Cusip;
            this.ActivationDate = DateTime.Today;
            this.Listing = listing;
            this.IndexReturnType = returnType;
            this.PriceComposition = new List<BasketPriceComposition>();
            this.PriceComposition.Add(basketPriceComposition);
            this.Modifications = new List<Modification>();
        }

        public BasketPrice(string Name, string Isin, string Figi, string Cusip, IndexReturnType returnType)
        {
            this.Name = Name;
            this.Isin = Isin;
            this.Figi = Figi;
            this.Cusip = Cusip;
            this.ActivationDate = DateTime.Today;
            this.IndexReturnType = returnType;
            this.Modifications = new List<Modification>();
            this.PriceComposition = new List<BasketPriceComposition>();
        }
        #endregion

    }
}
