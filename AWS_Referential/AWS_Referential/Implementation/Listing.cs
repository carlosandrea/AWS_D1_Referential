using AWS_Referential.Enumeration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AWS_Referential.Implementation
{
    public class Listing
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UniqueId { get; set; }

        [ForeignKey("Instrument")]
        public int InstrumentId { get; set; }
        public virtual Instrument Instrument { get; set; }

        public string Ric { get; set; }
        public string Mic { get; set; }
        public string PrimaryMic { get; set; }
        public string BloombergCode { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(100)]
        public string Sedol { get; set; }
        public string Cusip { get; set; }
        public string Name { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        [Column(TypeName = "decimal(38,18)")]
        public decimal CurrencyMultiplier { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime? DesactivationDate { get; set; }

        #region constructeur
        public Listing(string Ric, string Mic, String PrimaryMic, string BloombergCode, string Sedol, string Cusip, string Name, CurrencyCode currencyCode, decimal currencymultiplier)
        {
            this.Ric = Ric;
            this.Mic = Mic;
            this.PrimaryMic = PrimaryMic;
            this.BloombergCode = BloombergCode;
            this.Sedol = Sedol;
            this.Cusip = Cusip;
            this.Name = Name;
            this.CurrencyCode = currencyCode;
            this.CurrencyMultiplier = currencymultiplier;
            this.ActivationDate = DateTime.Today;
        }
        public Listing() { }
        public Listing(string Ric, string Mic, String PrimaryMic, string BloombergCode, string Sedol, string Cusip, string Name, CurrencyCode currencyCode, decimal currencymultiplier, Instrument instrument)
        {
            this.Ric = Ric;
            this.Mic = Mic;
            this.PrimaryMic = PrimaryMic;
            this.BloombergCode = BloombergCode;
            this.Sedol = Sedol;
            this.Cusip = Cusip;
            this.Name = Name;
            this.CurrencyCode = currencyCode;
            this.CurrencyMultiplier = currencymultiplier;
            this.Instrument = instrument;
            this.InstrumentId = instrument.UniqueId;
            this.ActivationDate = DateTime.Today;
        }
        #endregion
    }
}
