using AWS_Referential.Enumeration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AWS_Referential.Implementation
{
    public class BasketPriceComponent
    {

        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UniqueId { get; set; }


        [ForeignKey("BasketPriceComposition")]
        public int BasketPriceCompositionId { get; set; }
        public virtual BasketPriceComposition BasketPriceComposition { get; set; }



        [ForeignKey("Instrument")]
        //public int InstrumentId { get; set; }
        public int InstrumentId { get; set; }
        public virtual Instrument Instrument { get; set; }



        [Required, Column(TypeName = "decimal(38,18)")]
        public decimal Weight { get; set; }
        [Required, Column(TypeName = "decimal(38,18)")]
        public decimal PriceAdjustmentFactor { get; set; }
        [Required, Column(TypeName = "decimal(38,18)")]
        public decimal Units { get; set; }
        [Required, Column(TypeName = "decimal(38,18)")]
        public decimal FreeFloatAdjustmentFactor { get; set; }
        [Required, Column(TypeName = "decimal(38,18)")]
        public decimal OtherAdjustmentFactor1 { get; set; }
        [Required, Column(TypeName = "decimal(38,18)")]
        public decimal OtherAdjustmentFactor2 { get; set; }


        #region constructeur
        public BasketPriceComponent() { }
        public BasketPriceComponent(BasketPriceComposition BasketPriceComposition, Instrument component, decimal weight, decimal priceadjustmentfactor, decimal units, decimal freefloatadjustmentfactor, decimal OtherAdjustmentFactor1, decimal OtherAdjustmentFactor2)
        {
            this.BasketPriceComposition = BasketPriceComposition;
            this.BasketPriceCompositionId = BasketPriceComposition.UniqueId;
            this.Instrument = component;
            this.InstrumentId = component.UniqueId;
            this.Weight = weight;
            this.PriceAdjustmentFactor = priceadjustmentfactor;
            this.Units = units;
            this.FreeFloatAdjustmentFactor = freefloatadjustmentfactor;
            this.OtherAdjustmentFactor1 = OtherAdjustmentFactor1;
            this.OtherAdjustmentFactor2 = OtherAdjustmentFactor2;
        }
        public BasketPriceComponent(Instrument component, decimal weight, decimal priceadjustmentfactor, decimal units, decimal freefloatadjustmentfactor, decimal OtherAdjustmentFactor1, decimal OtherAdjustmentFactor2)
        {
            this.InstrumentId = component.UniqueId;
            this.Instrument = component;
            this.Weight = weight;
            this.PriceAdjustmentFactor = priceadjustmentfactor;
            this.Units = units;
            this.FreeFloatAdjustmentFactor = freefloatadjustmentfactor;
            this.OtherAdjustmentFactor1 = OtherAdjustmentFactor1;
            this.OtherAdjustmentFactor2 = OtherAdjustmentFactor2;
        }
        #endregion
    }
}
