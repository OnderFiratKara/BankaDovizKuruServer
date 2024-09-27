using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace BankaDovizKuruServer
{
    public class FxRateModel
    {
        public string Code { get; set; }          
        public string Description { get; set; }   
        public decimal Buy { get; set; }          
        public decimal Sell { get; set; }         
    }

    public class FxRateResponse
    {
        public List<FxRateModel> Data { get; set; }
        public bool Success { get; set; }
    }
    public class IsBankasiKurModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal FxRateBuy { get; set; }
        public decimal FxRateSell { get; set; }
    }

    public class IsBankasiResponse
    {
        public List<IsBankasiKurModel> Data { get; set; }
        public bool Success { get; set; }
    }

    public class DenizBankKurModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
    }

    public class DenizBankResponse
    {
        public List<DenizBankKurModel> Data { get; set; }
        public bool Success { get; set; }
    }

    public class HalkBankKurModel
    {

    }

    public class YapiKrediKurModel
    {
        public string code { get; set; }
        public decimal sell { get; set; }
        public decimal buy { get; set; }
        public string lastUpdate { get; set; }
        public decimal Change { get; set; }
        public string Date { get; set; }
        public decimal PreviousDayBuyingPrice { get; set; }
        public decimal PreviousDaySellingPrice { get; set; }
        public string DailyStatus { get; set; }
    }

    public class YapiKrediResponse
    {
        public List<YapiKrediKurModel> d { get; set; }
    }

    public class AkbankKurModel
    {
        public string Title { get; set; }       
        public decimal DovizAlis { get; set; }  
        public decimal DovizSatis { get; set; } 
        public string USDCaprazKur { get; set; } 
        public string KurTuru { get; set; }     
    }

    public class AkbankResponse
    {
        public string GetCurrencyRatesResult { get; set; }
    }

    public class AkbankResult
    {
        public List<AkbankKurModel> cur { get; set; }
        public string date { get; set; }
    }

    public class QnbFinansBankKurModel
    {
        public decimal BuyingExchangeRateField { get; set; }
        public decimal SellingExchangeRateField { get; set; }
        public string CurrencyNameField { get; set; }
        public string CurrencyTypeField { get; set; }
    }

    public class QnbFinansBankResponse
    {
        public List<QnbFinansBankKurModel> Data { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}

