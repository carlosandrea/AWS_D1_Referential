using System;
using System.Collections.Generic;
using System.Text;

namespace AWS_Referential.Enumeration
{
    public enum InstrumentType
    {
        STOCK,
        ETF,
        BOND,
        FUTURE,
        RIGHT
    }

    public enum CurrencyCode
    {
        EUR,
        USD,
        JPY,
        CHF,
        GBP,
        DKK,
        NOK,
        SEK,
        PLN,
        ZAR,

    }

    public enum DividendSource
    {
        MARKIT,
        BBG,
        CA
    }

    public enum DividendOrigin
    {
        ANALYST,
        COMPANY
    }

    public enum DividendForm
    {
        CASH,
        STOCK
    }

    public enum DividendType
    {
        YR,
        INTERIM,
        FIN,
        Q1,
        Q2,
        Q3,
        Q4,
        CR,
        CR2,
        CR3,
        INTPID,
        FINPID,
        SPEC2ND,
        COND,
        THE2NDINTPID,
        THE3RDINTPID,
        THE4THINTPID,
        THE2NDINT,
        THE3RDINT,
        THE4THINT,
        Q1PID,
        Q2PID,
        Q3PID,
        Q4PID,
        FINSO,
        SPEC,
        THE5THINTPID,
        SPEC3RD,
        THE5THINT,
        INT = INTERIM,
        CR4,
        M01,
        M02,
        M03,
        M04,
        M05,
        M06,
        M07,
        M08,
        M09,
        M10,
        M11,
        M12




    }

    public enum Status
    {
        ESTIMATE,
        CONFIRMED,
        UNKNOW,
        CANCEL,

    }

    public enum TaxJuridiction
    {
        FRANCE,
        GERMANY,
        SPAIN,
        ITALY,
        NETHERLANDS,
        UNITEDKINGDOM,
        IRELAND,
        BELGIUM,
        FINLAND,
        LUXEMBOURG,
        SWITZERLAND,
        ISLEOFMAN,
        JERSEY,
        MEXICO,
        NORWAY,
        POLAND,
        PORTUGAL,
        DENMARK,
        AUSTRIA,
        SWEDEN,
        FAROEISLAND,
        BERMUDA,
        GUERNSEY,
        MALTA,
        UNITEDSTATES,
        LIBERIA,
        PANAMA,
        CURACAO

    }

    public enum TaxCode
    {
        CAPITAL,
        DIVIDEND,
        REIT,
        EXEMPT,
        INCOME
    }

    public enum ObjectModification
    {
        INSTRUMENT,
        DIVIDEND,
        BASKETPRICE,
        COMPOSITION,
        Listing,
        EARNINGESTIMATE
    }

    public enum IndexReturnType
    {
        GROSS,
        NET,
        PRICE
    }

    public enum DerivativeType
    {
        FUTURE,
        TOTALRETURNFUTURE,
        DIVIDENDFUTURE
    }

    public enum PriceType
    {
        BID,
        ASK,
        CROSS
    }
}
