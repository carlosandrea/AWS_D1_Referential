﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using MarkitAvailableCompositions;
//
//    var markitAvailableComposition = MarkitAvailableComposition.FromJson(jsonString);

namespace MarkitAvailableCompositions
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class MarkitAvailableComposition
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("result")]
        public Result Result { get; set; }
    }

    public partial class Meta
    {
        [JsonProperty("params")]
        public Params Params { get; set; }

        [JsonProperty("restrictions")]
        public Restrictions Restrictions { get; set; }

        [JsonProperty("apiVersion")]
        public string ApiVersion { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("lic")]
        public long Lic { get; set; }

        [JsonProperty("received")]
        public string Received { get; set; }

        [JsonProperty("node")]
        public string Node { get; set; }

        [JsonProperty("returned")]
        public string Returned { get; set; }
    }

    public partial class Params
    {
        [JsonProperty("user")]
        public User User { get; set; }
    }

    public partial class User
    {
        [JsonProperty("password")]
        public object Password { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("authorities")]
        public Authority[] Authorities { get; set; }

        [JsonProperty("accountNonExpired")]
        public bool AccountNonExpired { get; set; }

        [JsonProperty("accountNonLocked")]
        public bool AccountNonLocked { get; set; }

        [JsonProperty("credentialsNonExpired")]
        public bool CredentialsNonExpired { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("licenseID")]
        public long LicenseId { get; set; }

        [JsonProperty("rawData")]
        public RawData RawData { get; set; }
    }

    public partial class Authority
    {
        [JsonProperty("authority")]
        public string AuthorityAuthority { get; set; }
    }

    public partial class RawData
    {
        [JsonProperty("userID")]
        public long UserId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("isAccountLocked")]
        public bool IsAccountLocked { get; set; }

        [JsonProperty("changePasswordNextLogin")]
        public bool ChangePasswordNextLogin { get; set; }

        [JsonProperty("accountExpiryDate")]
        public DateTimeOffset AccountExpiryDate { get; set; }

        [JsonProperty("passwordExpiryDate")]
        public DateTimeOffset PasswordExpiryDate { get; set; }

        [JsonProperty("resetTokenExpiryDate")]
        public object ResetTokenExpiryDate { get; set; }

        [JsonProperty("lastLogin")]
        public long LastLogin { get; set; }

        [JsonProperty("clientLicenseId")]
        public long ClientLicenseId { get; set; }

        [JsonProperty("loginAttempts")]
        public long LoginAttempts { get; set; }

        [JsonProperty("resetToken")]
        public string ResetToken { get; set; }

        [JsonProperty("roles")]
        public object[] Roles { get; set; }
    }

    public partial class Restrictions
    {
    }

    public partial class Result
    {
        [JsonProperty("LicensedCorporateActionSources")]
        public object[] LicensedCorporateActionSources { get; set; }

        [JsonProperty("LicensedClassificationSchemes")]
        public string[] LicensedClassificationSchemes { get; set; }

        [JsonProperty("LicensedPropertySets")]
        public string[] LicensedPropertySets { get; set; }

        [JsonProperty("LicensedValueSets")]
        public string[] LicensedValueSets { get; set; }

        [JsonProperty("Indices")]
        public Etf[] Indices { get; set; }

        [JsonProperty("ETFs")]
        public Etf[] EtFs { get; set; }
    }

    public partial class Etf
    {
        [JsonProperty("SecurityID")]
        public long SecurityId { get; set; }

        [JsonProperty("ListingID")]
        public long? ListingId { get; set; }

        [JsonProperty("Bloomberg")]
        public string Bloomberg { get; set; }

        [JsonProperty("ExchangeTicker")]
        public string ExchangeTicker { get; set; }

        [JsonProperty("Isin")]
        public string Isin { get; set; }

        [JsonProperty("Cusip")]
        public string Cusip { get; set; }

        [JsonProperty("Ric")]
        public string Ric { get; set; }

        [JsonProperty("ListingCurrency")]
        public ListingCurrency? ListingCurrency { get; set; }

        [JsonProperty("Mic")]
        public Mic? Mic { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("SecurityType")]
        public SecurityType SecurityType { get; set; }

        [JsonProperty("Sedol")]
        public string Sedol { get; set; }

        [JsonProperty("Other")]
        public string Other { get; set; }

        [JsonProperty("HasDividends")]
        public bool HasDividends { get; set; }

        [JsonProperty("HasRebalance")]
        public bool HasRebalance { get; set; }

        [JsonProperty("HasIntradayChanges")]
        public bool HasIntradayChanges { get; set; }

        [JsonProperty("HasTPlus")]
        public bool HasTPlus { get; set; }

        [JsonProperty("ProviderName")]
        public string ProviderName { get; set; }

        [JsonProperty("HasOfficialWeightings")]
        public bool HasOfficialWeightings { get; set; }

        [JsonProperty("HasPoints")]
        public bool HasPoints { get; set; }

        [JsonProperty("CloseIsOfficial")]
        public string CloseIsOfficial { get; set; }

        [JsonProperty("OpenIsOfficial")]
        public string OpenIsOfficial { get; set; }

        [JsonProperty("LicensedForDividends")]
        public bool LicensedForDividends { get; set; }

        [JsonProperty("LicensedForPoints")]
        public bool LicensedForPoints { get; set; }

        [JsonProperty("LicensedOfficialWeightings")]
        public bool LicensedOfficialWeightings { get; set; }

        [JsonProperty("LicensedForTplus")]
        public bool LicensedForTplus { get; set; }

        [JsonProperty("IndexID")]
        public long IndexId { get; set; }

        [JsonProperty("ReleaseTime")]
        public string ReleaseTime { get; set; }

        [JsonProperty("LateTime")]
        public string LateTime { get; set; }

        [JsonProperty("Baskets", NullValueHandling = NullValueHandling.Ignore)]
        public Basket[] Baskets { get; set; }

        [JsonProperty("CalculationMask", NullValueHandling = NullValueHandling.Ignore)]
        public long? CalculationMask { get; set; }

        [JsonProperty("BasketName", NullValueHandling = NullValueHandling.Ignore)]
        public string BasketName { get; set; }

        [JsonProperty("LicensedDividendSources", NullValueHandling = NullValueHandling.Ignore)]
        public object[] LicensedDividendSources { get; set; }

        [JsonProperty("LicencedDividendSources", NullValueHandling = NullValueHandling.Ignore)]
        public LicencedDividendSource[] LicencedDividendSources { get; set; }
    }

    public partial class Basket
    {
        [JsonProperty("BasketName")]
        public BasketName BasketName { get; set; }

        [JsonProperty("CalculationMask")]
        public long CalculationMask { get; set; }
    }

    public enum BasketName { BetaCalculation, Calculation, Creation, Excluded, Holdings, MimsCalculation, MimsCreation, MimsRedemption, Redemption, Tracking };

    public enum LicencedDividendSource { MarkitDividends };

    public enum ListingCurrency { Aud, Brl, Cad, Chf, Clp, Cny, Czk, Dkk, Egp, Eur, Gbp, Gbx, Hkd, Huf, Idr, Inr, Isk, Jpy, Krw, Mxn, Myr, Nok, Php, Pln, Rub, Sek, Sgd, Thb, Try, Twd, Usd, Zar };

    public enum Mic { Arcx, Bats, Bvmf, Etfp, Misx, None, Roco, Wbah, Xade, Xams, Xasx, Xath, Xbkk, Xbom, Xbru, Xbud, Xcai, Xcse, Xdub, Xetr, Xhel, Xhkg, Xice, Xidx, Xjas, Xjse, Xkls, Xkrx, Xlom, Xlon, Xlux, Xmad, Xmce, Xmsm, Xncm, Xngo, Xngs, Xnms, Xnse, Xnys, Xosl, Xpar, Xphs, Xses, Xshe, Xshg, Xsto, Xstu, Xswx, Xtai, Xtks, Xtse, Xvtx, Xwar, Xwbo };

    public enum SecurityType { CustomBasket, Etc, Etf, Etn, Index };

    public partial class MarkitAvailableComposition
    {
        public static MarkitAvailableComposition FromJson(string json) => JsonConvert.DeserializeObject<MarkitAvailableComposition>(json, MarkitAvailableCompositions.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this MarkitAvailableComposition self) => JsonConvert.SerializeObject(self, MarkitAvailableCompositions.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                BasketNameConverter.Singleton,
                LicencedDividendSourceConverter.Singleton,
                ListingCurrencyConverter.Singleton,
                MicConverter.Singleton,
                SecurityTypeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class BasketNameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(BasketName) || t == typeof(BasketName?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "BetaCalculation":
                    return BasketName.BetaCalculation;
                case "Calculation":
                    return BasketName.Calculation;
                case "Creation":
                    return BasketName.Creation;
                case "Excluded":
                    return BasketName.Excluded;
                case "Holdings":
                    return BasketName.Holdings;
                case "MIMS Calculation":
                    return BasketName.MimsCalculation;
                case "MIMS Creation":
                    return BasketName.MimsCreation;
                case "MIMS Redemption":
                    return BasketName.MimsRedemption;
                case "Redemption":
                    return BasketName.Redemption;
                case "Tracking":
                    return BasketName.Tracking;
            }
            throw new Exception("Cannot unmarshal type BasketName");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (BasketName)untypedValue;
            switch (value)
            {
                case BasketName.BetaCalculation:
                    serializer.Serialize(writer, "BetaCalculation");
                    return;
                case BasketName.Calculation:
                    serializer.Serialize(writer, "Calculation");
                    return;
                case BasketName.Creation:
                    serializer.Serialize(writer, "Creation");
                    return;
                case BasketName.Excluded:
                    serializer.Serialize(writer, "Excluded");
                    return;
                case BasketName.Holdings:
                    serializer.Serialize(writer, "Holdings");
                    return;
                case BasketName.MimsCalculation:
                    serializer.Serialize(writer, "MIMS Calculation");
                    return;
                case BasketName.MimsCreation:
                    serializer.Serialize(writer, "MIMS Creation");
                    return;
                case BasketName.MimsRedemption:
                    serializer.Serialize(writer, "MIMS Redemption");
                    return;
                case BasketName.Redemption:
                    serializer.Serialize(writer, "Redemption");
                    return;
                case BasketName.Tracking:
                    serializer.Serialize(writer, "Tracking");
                    return;
            }
            throw new Exception("Cannot marshal type BasketName");
        }

        public static readonly BasketNameConverter Singleton = new BasketNameConverter();
    }

    internal class LicencedDividendSourceConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(LicencedDividendSource) || t == typeof(LicencedDividendSource?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Markit Dividends")
            {
                return LicencedDividendSource.MarkitDividends;
            }
            throw new Exception("Cannot unmarshal type LicencedDividendSource");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (LicencedDividendSource)untypedValue;
            if (value == LicencedDividendSource.MarkitDividends)
            {
                serializer.Serialize(writer, "Markit Dividends");
                return;
            }
            throw new Exception("Cannot marshal type LicencedDividendSource");
        }

        public static readonly LicencedDividendSourceConverter Singleton = new LicencedDividendSourceConverter();
    }

    internal class ListingCurrencyConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ListingCurrency) || t == typeof(ListingCurrency?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "AUD":
                    return ListingCurrency.Aud;
                case "BRL":
                    return ListingCurrency.Brl;
                case "CAD":
                    return ListingCurrency.Cad;
                case "CHF":
                    return ListingCurrency.Chf;
                case "CLP":
                    return ListingCurrency.Clp;
                case "CNY":
                    return ListingCurrency.Cny;
                case "CZK":
                    return ListingCurrency.Czk;
                case "DKK":
                    return ListingCurrency.Dkk;
                case "EGP":
                    return ListingCurrency.Egp;
                case "EUR":
                    return ListingCurrency.Eur;
                case "GBP":
                    return ListingCurrency.Gbp;
                case "GBX":
                    return ListingCurrency.Gbx;
                case "HKD":
                    return ListingCurrency.Hkd;
                case "HUF":
                    return ListingCurrency.Huf;
                case "IDR":
                    return ListingCurrency.Idr;
                case "INR":
                    return ListingCurrency.Inr;
                case "ISK":
                    return ListingCurrency.Isk;
                case "JPY":
                    return ListingCurrency.Jpy;
                case "KRW":
                    return ListingCurrency.Krw;
                case "MXN":
                    return ListingCurrency.Mxn;
                case "MYR":
                    return ListingCurrency.Myr;
                case "NOK":
                    return ListingCurrency.Nok;
                case "PHP":
                    return ListingCurrency.Php;
                case "PLN":
                    return ListingCurrency.Pln;
                case "RUB":
                    return ListingCurrency.Rub;
                case "SEK":
                    return ListingCurrency.Sek;
                case "SGD":
                    return ListingCurrency.Sgd;
                case "THB":
                    return ListingCurrency.Thb;
                case "TRY":
                    return ListingCurrency.Try;
                case "TWD":
                    return ListingCurrency.Twd;
                case "USD":
                    return ListingCurrency.Usd;
                case "ZAR":
                    return ListingCurrency.Zar;
            }
            throw new Exception("Cannot unmarshal type ListingCurrency");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ListingCurrency)untypedValue;
            switch (value)
            {
                case ListingCurrency.Aud:
                    serializer.Serialize(writer, "AUD");
                    return;
                case ListingCurrency.Brl:
                    serializer.Serialize(writer, "BRL");
                    return;
                case ListingCurrency.Cad:
                    serializer.Serialize(writer, "CAD");
                    return;
                case ListingCurrency.Chf:
                    serializer.Serialize(writer, "CHF");
                    return;
                case ListingCurrency.Clp:
                    serializer.Serialize(writer, "CLP");
                    return;
                case ListingCurrency.Cny:
                    serializer.Serialize(writer, "CNY");
                    return;
                case ListingCurrency.Czk:
                    serializer.Serialize(writer, "CZK");
                    return;
                case ListingCurrency.Dkk:
                    serializer.Serialize(writer, "DKK");
                    return;
                case ListingCurrency.Egp:
                    serializer.Serialize(writer, "EGP");
                    return;
                case ListingCurrency.Eur:
                    serializer.Serialize(writer, "EUR");
                    return;
                case ListingCurrency.Gbp:
                    serializer.Serialize(writer, "GBP");
                    return;
                case ListingCurrency.Gbx:
                    serializer.Serialize(writer, "GBX");
                    return;
                case ListingCurrency.Hkd:
                    serializer.Serialize(writer, "HKD");
                    return;
                case ListingCurrency.Huf:
                    serializer.Serialize(writer, "HUF");
                    return;
                case ListingCurrency.Idr:
                    serializer.Serialize(writer, "IDR");
                    return;
                case ListingCurrency.Inr:
                    serializer.Serialize(writer, "INR");
                    return;
                case ListingCurrency.Isk:
                    serializer.Serialize(writer, "ISK");
                    return;
                case ListingCurrency.Jpy:
                    serializer.Serialize(writer, "JPY");
                    return;
                case ListingCurrency.Krw:
                    serializer.Serialize(writer, "KRW");
                    return;
                case ListingCurrency.Mxn:
                    serializer.Serialize(writer, "MXN");
                    return;
                case ListingCurrency.Myr:
                    serializer.Serialize(writer, "MYR");
                    return;
                case ListingCurrency.Nok:
                    serializer.Serialize(writer, "NOK");
                    return;
                case ListingCurrency.Php:
                    serializer.Serialize(writer, "PHP");
                    return;
                case ListingCurrency.Pln:
                    serializer.Serialize(writer, "PLN");
                    return;
                case ListingCurrency.Rub:
                    serializer.Serialize(writer, "RUB");
                    return;
                case ListingCurrency.Sek:
                    serializer.Serialize(writer, "SEK");
                    return;
                case ListingCurrency.Sgd:
                    serializer.Serialize(writer, "SGD");
                    return;
                case ListingCurrency.Thb:
                    serializer.Serialize(writer, "THB");
                    return;
                case ListingCurrency.Try:
                    serializer.Serialize(writer, "TRY");
                    return;
                case ListingCurrency.Twd:
                    serializer.Serialize(writer, "TWD");
                    return;
                case ListingCurrency.Usd:
                    serializer.Serialize(writer, "USD");
                    return;
                case ListingCurrency.Zar:
                    serializer.Serialize(writer, "ZAR");
                    return;
            }
            throw new Exception("Cannot marshal type ListingCurrency");
        }

        public static readonly ListingCurrencyConverter Singleton = new ListingCurrencyConverter();
    }

    internal class MicConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Mic) || t == typeof(Mic?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "ARCX":
                    return Mic.Arcx;
                case "BATS":
                    return Mic.Bats;
                case "BVMF":
                    return Mic.Bvmf;
                case "ETFP":
                    return Mic.Etfp;
                case "MISX":
                    return Mic.Misx;
                case "NONE":
                    return Mic.None;
                case "ROCO":
                    return Mic.Roco;
                case "WBAH":
                    return Mic.Wbah;
                case "XADE":
                    return Mic.Xade;
                case "XAMS":
                    return Mic.Xams;
                case "XASX":
                    return Mic.Xasx;
                case "XATH":
                    return Mic.Xath;
                case "XBKK":
                    return Mic.Xbkk;
                case "XBOM":
                    return Mic.Xbom;
                case "XBRU":
                    return Mic.Xbru;
                case "XBUD":
                    return Mic.Xbud;
                case "XCAI":
                    return Mic.Xcai;
                case "XCSE":
                    return Mic.Xcse;
                case "XDUB":
                    return Mic.Xdub;
                case "XETR":
                    return Mic.Xetr;
                case "XHEL":
                    return Mic.Xhel;
                case "XHKG":
                    return Mic.Xhkg;
                case "XICE":
                    return Mic.Xice;
                case "XIDX":
                    return Mic.Xidx;
                case "XJAS":
                    return Mic.Xjas;
                case "XJSE":
                    return Mic.Xjse;
                case "XKLS":
                    return Mic.Xkls;
                case "XKRX":
                    return Mic.Xkrx;
                case "XLOM":
                    return Mic.Xlom;
                case "XLON":
                    return Mic.Xlon;
                case "XLUX":
                    return Mic.Xlux;
                case "XMAD":
                    return Mic.Xmad;
                case "XMCE":
                    return Mic.Xmce;
                case "XMSM":
                    return Mic.Xmsm;
                case "XNCM":
                    return Mic.Xncm;
                case "XNGO":
                    return Mic.Xngo;
                case "XNGS":
                    return Mic.Xngs;
                case "XNMS":
                    return Mic.Xnms;
                case "XNSE":
                    return Mic.Xnse;
                case "XNYS":
                    return Mic.Xnys;
                case "XOSL":
                    return Mic.Xosl;
                case "XPAR":
                    return Mic.Xpar;
                case "XPHS":
                    return Mic.Xphs;
                case "XSES":
                    return Mic.Xses;
                case "XSHE":
                    return Mic.Xshe;
                case "XSHG":
                    return Mic.Xshg;
                case "XSTO":
                    return Mic.Xsto;
                case "XSTU":
                    return Mic.Xstu;
                case "XSWX":
                    return Mic.Xswx;
                case "XTAI":
                    return Mic.Xtai;
                case "XTKS":
                    return Mic.Xtks;
                case "XTSE":
                    return Mic.Xtse;
                case "XVTX":
                    return Mic.Xvtx;
                case "XWAR":
                    return Mic.Xwar;
                case "XWBO":
                    return Mic.Xwbo;
            }
            throw new Exception("Cannot unmarshal type Mic");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Mic)untypedValue;
            switch (value)
            {
                case Mic.Arcx:
                    serializer.Serialize(writer, "ARCX");
                    return;
                case Mic.Bats:
                    serializer.Serialize(writer, "BATS");
                    return;
                case Mic.Bvmf:
                    serializer.Serialize(writer, "BVMF");
                    return;
                case Mic.Etfp:
                    serializer.Serialize(writer, "ETFP");
                    return;
                case Mic.Misx:
                    serializer.Serialize(writer, "MISX");
                    return;
                case Mic.None:
                    serializer.Serialize(writer, "NONE");
                    return;
                case Mic.Roco:
                    serializer.Serialize(writer, "ROCO");
                    return;
                case Mic.Wbah:
                    serializer.Serialize(writer, "WBAH");
                    return;
                case Mic.Xade:
                    serializer.Serialize(writer, "XADE");
                    return;
                case Mic.Xams:
                    serializer.Serialize(writer, "XAMS");
                    return;
                case Mic.Xasx:
                    serializer.Serialize(writer, "XASX");
                    return;
                case Mic.Xath:
                    serializer.Serialize(writer, "XATH");
                    return;
                case Mic.Xbkk:
                    serializer.Serialize(writer, "XBKK");
                    return;
                case Mic.Xbom:
                    serializer.Serialize(writer, "XBOM");
                    return;
                case Mic.Xbru:
                    serializer.Serialize(writer, "XBRU");
                    return;
                case Mic.Xbud:
                    serializer.Serialize(writer, "XBUD");
                    return;
                case Mic.Xcai:
                    serializer.Serialize(writer, "XCAI");
                    return;
                case Mic.Xcse:
                    serializer.Serialize(writer, "XCSE");
                    return;
                case Mic.Xdub:
                    serializer.Serialize(writer, "XDUB");
                    return;
                case Mic.Xetr:
                    serializer.Serialize(writer, "XETR");
                    return;
                case Mic.Xhel:
                    serializer.Serialize(writer, "XHEL");
                    return;
                case Mic.Xhkg:
                    serializer.Serialize(writer, "XHKG");
                    return;
                case Mic.Xice:
                    serializer.Serialize(writer, "XICE");
                    return;
                case Mic.Xidx:
                    serializer.Serialize(writer, "XIDX");
                    return;
                case Mic.Xjas:
                    serializer.Serialize(writer, "XJAS");
                    return;
                case Mic.Xjse:
                    serializer.Serialize(writer, "XJSE");
                    return;
                case Mic.Xkls:
                    serializer.Serialize(writer, "XKLS");
                    return;
                case Mic.Xkrx:
                    serializer.Serialize(writer, "XKRX");
                    return;
                case Mic.Xlom:
                    serializer.Serialize(writer, "XLOM");
                    return;
                case Mic.Xlon:
                    serializer.Serialize(writer, "XLON");
                    return;
                case Mic.Xlux:
                    serializer.Serialize(writer, "XLUX");
                    return;
                case Mic.Xmad:
                    serializer.Serialize(writer, "XMAD");
                    return;
                case Mic.Xmce:
                    serializer.Serialize(writer, "XMCE");
                    return;
                case Mic.Xmsm:
                    serializer.Serialize(writer, "XMSM");
                    return;
                case Mic.Xncm:
                    serializer.Serialize(writer, "XNCM");
                    return;
                case Mic.Xngo:
                    serializer.Serialize(writer, "XNGO");
                    return;
                case Mic.Xngs:
                    serializer.Serialize(writer, "XNGS");
                    return;
                case Mic.Xnms:
                    serializer.Serialize(writer, "XNMS");
                    return;
                case Mic.Xnse:
                    serializer.Serialize(writer, "XNSE");
                    return;
                case Mic.Xnys:
                    serializer.Serialize(writer, "XNYS");
                    return;
                case Mic.Xosl:
                    serializer.Serialize(writer, "XOSL");
                    return;
                case Mic.Xpar:
                    serializer.Serialize(writer, "XPAR");
                    return;
                case Mic.Xphs:
                    serializer.Serialize(writer, "XPHS");
                    return;
                case Mic.Xses:
                    serializer.Serialize(writer, "XSES");
                    return;
                case Mic.Xshe:
                    serializer.Serialize(writer, "XSHE");
                    return;
                case Mic.Xshg:
                    serializer.Serialize(writer, "XSHG");
                    return;
                case Mic.Xsto:
                    serializer.Serialize(writer, "XSTO");
                    return;
                case Mic.Xstu:
                    serializer.Serialize(writer, "XSTU");
                    return;
                case Mic.Xswx:
                    serializer.Serialize(writer, "XSWX");
                    return;
                case Mic.Xtai:
                    serializer.Serialize(writer, "XTAI");
                    return;
                case Mic.Xtks:
                    serializer.Serialize(writer, "XTKS");
                    return;
                case Mic.Xtse:
                    serializer.Serialize(writer, "XTSE");
                    return;
                case Mic.Xvtx:
                    serializer.Serialize(writer, "XVTX");
                    return;
                case Mic.Xwar:
                    serializer.Serialize(writer, "XWAR");
                    return;
                case Mic.Xwbo:
                    serializer.Serialize(writer, "XWBO");
                    return;
            }
            throw new Exception("Cannot marshal type Mic");
        }

        public static readonly MicConverter Singleton = new MicConverter();
    }

    internal class SecurityTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(SecurityType) || t == typeof(SecurityType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Custom Basket":
                    return SecurityType.CustomBasket;
                case "ETC":
                    return SecurityType.Etc;
                case "ETF":
                    return SecurityType.Etf;
                case "ETN":
                    return SecurityType.Etn;
                case "Index":
                    return SecurityType.Index;
            }
            throw new Exception("Cannot unmarshal type SecurityType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (SecurityType)untypedValue;
            switch (value)
            {
                case SecurityType.CustomBasket:
                    serializer.Serialize(writer, "Custom Basket");
                    return;
                case SecurityType.Etc:
                    serializer.Serialize(writer, "ETC");
                    return;
                case SecurityType.Etf:
                    serializer.Serialize(writer, "ETF");
                    return;
                case SecurityType.Etn:
                    serializer.Serialize(writer, "ETN");
                    return;
                case SecurityType.Index:
                    serializer.Serialize(writer, "Index");
                    return;
            }
            throw new Exception("Cannot marshal type SecurityType");
        }

        public static readonly SecurityTypeConverter Singleton = new SecurityTypeConverter();
    }
}
