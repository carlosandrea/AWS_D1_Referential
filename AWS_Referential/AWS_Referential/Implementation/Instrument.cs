using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AWS_Referential.Implementation
{
    public class Instrument
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UniqueId { get; set; }
        public string Name { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(100)]
        public string Isin { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(100)]
        public string Figi { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(100)]
        public string Cusip { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime? DesactivationDate { get; set; }


        #region Constructeur
        public Instrument(string Name, string Isin, String Figi, String Cusip)
        {
            this.Name = Name;
            this.Isin = Isin;
            this.Figi = Figi;
            this.Cusip = Cusip;
            this.ActivationDate = DateTime.Today;

        }

        public Instrument() { }
        #endregion
    }
}
