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
        public double? IS_EPS { get; set; }
        public double? BEST_TARGET_PRICE { get; set; }
        public double? BEST_TARGET_MEDIAN { get; set; }
        public double? BEST_EBIT { get; set; }
        public double? EBIT { get; set; }
        public double? EBITDA { get; set; }
        public double? BEST_EBITDA { get; set; }
        public double? FREE_CASH_FLOW_YIELD { get; set; }
        public double? BEST_ESTIMATE_FCF { get; set; }
        //
        public double? Price_Earning { get; set; }
        public double? Price_To_Book { get; set; }
        public double? Price_To_Sale { get; set; }
        public double? Price_To_CF { get; set; }
        public double? PE_To_LTGR { get; set; }
        public double? DPS_To_EPS { get; set; }
        public double? Dividend_Per_Share { get; set; }
        public double? Sales { get; set; }
        public double? Net_Income { get; set; }
        public double? ROE { get; set; }
        public double? BEST_ROA { get; set; }
        public double? BEST_Gross_Margin { get; set; }
        public double? CF_From_Operation { get; set; }
        public double? ROA { get; set; }
        public double? Total_Asset { get; set; }
        public double? Financial_Leverage { get; set; }
        public double? LTDEBT_To_TotalAsset { get; set; }
        public double? EquityShares_Outstanding { get; set; }
        public double? Gross_Margin { get; set; }
        public double? Asset_TurnOver { get; set; }



        public DateTime Last_Update { get; set; }

        public EarningEstimate() { }
        public EarningEstimate(Stock stock, double BEST_EPS, double IS_EPS, double BEST_TARGET_PRICE, double BEST_TARGET_MEDIAN, double BEST_EBIT, double EBIT, double EBITDA, double BEST_EBITDA, double FREE_CASH_FLOW_YIELD, double BEST_ESTIMATE_FCF
            , double Price_Earning, double Price_To_Book, double Price_To_Sale, double Price_To_CF, double PE_To_LTGR, double DPS_To_EPS, double Dividend_Per_Share, double Sales, double Net_Income, double ROE, double BEST_ROA, double BEST_Gross_Margin,
            double CF_From_Operation, double ROA, double Total_Asset, double Financial_Leverage, double LTDEBT_To_TotalAsset, double EquityShares_Outstanding, double Gross_margin, double Asset_TurnOver, DateTime Last_Update)
        {
            this.Stock = stock;
            this.BEST_EPS = BEST_EPS;
            this.IS_EPS = IS_EPS;
            this.BEST_TARGET_PRICE = BEST_TARGET_PRICE;
            this.BEST_TARGET_MEDIAN = BEST_TARGET_MEDIAN;
            this.BEST_EBIT = BEST_EBIT;
            this.EBIT = EBIT;
            this.EBITDA = EBITDA;
            this.BEST_EBITDA = BEST_EBITDA;
            this.FREE_CASH_FLOW_YIELD = FREE_CASH_FLOW_YIELD;
            this.BEST_ESTIMATE_FCF = BEST_ESTIMATE_FCF;

            //
            this.Price_Earning = Price_Earning;
            this.Price_To_Book = Price_To_Book;
            this.Price_To_Sale = Price_To_Sale;
            this.Price_To_CF = Price_To_CF;
            this.PE_To_LTGR = PE_To_LTGR;
            this.DPS_To_EPS = DPS_To_EPS;
            this.Dividend_Per_Share = Dividend_Per_Share;
            this.Sales = Sales;
            this.Net_Income = Net_Income;
            this.ROE = ROE;
            this.BEST_ROA = BEST_ROA;
            this.BEST_Gross_Margin = BEST_Gross_Margin;
            this.CF_From_Operation = CF_From_Operation;
            this.ROA = ROA;
            this.Total_Asset = Total_Asset;
            this.Financial_Leverage = Financial_Leverage;
            this.LTDEBT_To_TotalAsset = LTDEBT_To_TotalAsset;
            this.EquityShares_Outstanding = EquityShares_Outstanding;
            this.Gross_Margin = Gross_Margin;
            this.Asset_TurnOver = Asset_TurnOver;





            this.Last_Update = Last_Update;

        }

    }
}
