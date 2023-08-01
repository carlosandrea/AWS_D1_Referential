using AWS_Referential.Enumeration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AWS_Referential.Implementation
{
    public class Dividend
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UniqueId { get; set; }

        [ForeignKey("Instrument")]
        public int InstrumentId { get; set; }
        public virtual Instrument Instrument { get; set; }

        public CurrencyCode DividendCurrency { get; set; }
        [Column(TypeName = "decimal(38,18)")]
        public decimal CurrencyMultiplier { get; set; }
        [Column(TypeName = "decimal(38,18)")]
        public decimal FxRate { get; set; }
        [Column(TypeName = "decimal(38,18)")]
        public decimal GrossAmount { get; set; }
        public DividendSource DividendSource { get; set; }
        public DividendForm DividendForm { get; set; }
        public Status DividendStatus { get; set; }
        public DateTime ExDate { get; set; }
        public DateTime PayDate { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime? AgmDate { get; set; }
        public Status AgmStatus { get; set; }

        public DividendType DividendType { get; set; }
        public Boolean IsScriptOptionnal { get; set; }
        public TaxCode TaxCode { get; set; }
        public TaxJuridiction TaxJuridiction { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(100)]
        public string? MarkitId { get; set; }
        public string Notes { get; set; }
        public string? OtherEstimates { get; set; }
        public virtual ICollection<Modification> Modifications { get; set; }

        #region Constructeur
        public Dividend(CurrencyCode dividendcurrency, decimal currencymultiplier, decimal fxrate, decimal grossamount, DividendSource dividendSource, DividendForm dividendForm,
            Status dividendStatus, DateTime exdate, DateTime paydate, DateTime recordate, DateTime AgmDate, Status agmstatus, DividendType dividendtype, Boolean isscriptoptionnal, TaxCode taxcode, TaxJuridiction taxjuridiction,
            string? markitId, string notes, string otherestimates, Instrument instrument)
        {
            this.DividendCurrency = dividendcurrency;
            this.CurrencyMultiplier = currencymultiplier;
            this.FxRate = fxrate;
            this.GrossAmount = grossamount;
            this.DividendSource = dividendSource;
            this.DividendForm = dividendForm;
            this.DividendStatus = dividendStatus;
            this.ExDate = exdate;
            this.PayDate = paydate;
            this.RecordDate = recordate;
            this.AgmDate = AgmDate;
            this.AgmStatus = agmstatus;
            this.DividendType = dividendtype;
            this.IsScriptOptionnal = isscriptoptionnal;
            this.TaxCode = taxcode;
            this.TaxJuridiction = taxjuridiction;
            this.Notes = notes;
            this.OtherEstimates = otherestimates;
            this.Modifications = new List<Modification>();
            this.LastUpdate = DateTime.Today;
            this.Instrument = instrument;
            this.InstrumentId = instrument.UniqueId;
            this.MarkitId = markitId;
        }

        public Dividend(CurrencyCode dividendcurrency, decimal currencymultiplier, decimal fxrate, decimal grossamount, DividendSource dividendSource, DividendForm dividendForm,
                    Status dividendStatus, DateTime exdate, DateTime paydate, DateTime recordate, DateTime AgmDate, Status agmstatus, DividendType dividendtype, Boolean isscriptoptionnal, TaxCode taxcode,
                    TaxJuridiction taxjuridiction, string notes, string otherestimates, Instrument instrument)
        {
            this.DividendCurrency = dividendcurrency;
            this.CurrencyMultiplier = currencymultiplier;
            this.FxRate = fxrate;
            this.GrossAmount = grossamount;
            this.DividendSource = dividendSource;
            this.DividendForm = dividendForm;
            this.DividendStatus = dividendStatus;
            this.ExDate = exdate;
            this.PayDate = paydate;
            this.RecordDate = recordate;
            this.AgmDate = AgmDate;
            this.AgmStatus = agmstatus;
            this.DividendType = dividendtype;
            this.IsScriptOptionnal = isscriptoptionnal;
            this.TaxCode = taxcode;
            this.TaxJuridiction = taxjuridiction;
            this.Notes = notes;
            this.OtherEstimates = otherestimates;
            this.Modifications = new List<Modification>();
            this.LastUpdate = DateTime.Today;
            this.Instrument = instrument;
            this.InstrumentId = instrument.UniqueId;
        }

        public Dividend(CurrencyCode dividendcurrency, decimal currencymultiplier, decimal fxrate, decimal grossamount, DividendSource dividendSource, DividendForm dividendForm,
                    Status dividendStatus, DateTime exdate, DateTime paydate, DateTime recordate, DateTime AgmDate, Status agmstatus, DividendType dividendtype, Boolean isscriptoptionnal,
                    TaxCode taxcode, TaxJuridiction taxjuridiction, string? markitId, string notes, string otherestimates)
        {
            this.DividendCurrency = dividendcurrency;
            this.CurrencyMultiplier = currencymultiplier;
            this.FxRate = fxrate;
            this.GrossAmount = grossamount;
            this.DividendSource = dividendSource;
            this.DividendForm = dividendForm;
            this.DividendStatus = dividendStatus;
            this.ExDate = exdate;
            this.PayDate = paydate;
            this.RecordDate = recordate;
            this.AgmDate = AgmDate;
            this.AgmStatus = agmstatus;
            this.DividendType = dividendtype;
            this.IsScriptOptionnal = isscriptoptionnal;
            this.TaxCode = taxcode;
            this.TaxJuridiction = taxjuridiction;
            this.Notes = notes;
            this.OtherEstimates = otherestimates;
            this.Modifications = new List<Modification>();
            this.LastUpdate = DateTime.Today;
            this.MarkitId = markitId;
        }
        public Dividend(CurrencyCode dividendcurrency, decimal currencymultiplier, decimal fxrate, decimal grossamount, DividendSource dividendSource, DividendForm dividendForm,
                    Status dividendStatus, DateTime exdate, DateTime paydate, DateTime recordate, DateTime AgmDate, Status agmstatus, DividendType dividendtype, Boolean isscriptoptionnal, TaxCode taxcode,
                    TaxJuridiction taxjuridiction, string notes, string otherestimates)
        {
            this.DividendCurrency = dividendcurrency;
            this.CurrencyMultiplier = currencymultiplier;
            this.FxRate = fxrate;
            this.GrossAmount = grossamount;
            this.DividendSource = dividendSource;
            this.DividendForm = dividendForm;
            this.DividendStatus = dividendStatus;
            this.ExDate = exdate;
            this.PayDate = paydate;
            this.RecordDate = recordate;
            this.AgmDate = AgmDate;
            this.AgmStatus = agmstatus;
            this.DividendType = dividendtype;
            this.IsScriptOptionnal = isscriptoptionnal;
            this.TaxCode = taxcode;
            this.TaxJuridiction = taxjuridiction;
            this.Notes = notes;
            this.OtherEstimates = otherestimates;
            this.Modifications = new List<Modification>();
            this.LastUpdate = DateTime.Today;
        }
        public Dividend() { }
        #endregion
    }
}
