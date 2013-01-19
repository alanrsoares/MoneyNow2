using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;

namespace MoneyNow2.ServiceInterface.Services
{
    [Route("/CurrencyConverter/{amount}/{from}/{to}")]
    public class CurrencyConverter : IReturn<CurrencyConverterResponse>
    {
        public decimal Amount { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public CurrencyConverter(decimal amount, string from, string to)
        {
            Amount = amount;
            From = from;
            To = to;
        }
    }

    public class CurrencyConverterResponse
    {
        public CurrencyInfo Info { get; set; }
        public string Result { get; set; }
    }

    public class CurrencyConverterRepository
    {
        private const string BaseUri = "http://www.google.com/finance/converter?a={0}&from={1}&to={2}";

        public string[] GetConversion(CurrencyConverter request)
        {
            var endPoint = BaseUri.Fmt(request.Amount, request.From, request.To);

            const string strRegex = @"<span class=bld>(.+?)\</span\>";

            using (var client = new WebClient())
            {
                var response = client.DownloadString(endPoint);
                var regex = new Regex(strRegex, RegexOptions.None);
                var match = regex.Match(response).Groups[1].ToString();
                return match.Split(' ');
            }
        }
    }

    public class CurrencyConverterService : Service
    {
        public CurrencyConverterRepository CurrencyConverterRepository { get; set; }
        public CurrencyInfoRepository CurrencyInfoRepository { get; set; }

        public dynamic Get(CurrencyConverter request)
        {
            var conversionResult = CurrencyConverterRepository.GetConversion(request);
            return new CurrencyConverterResponse
                       {
                           Info = CurrencyInfoRepository.GetById(request.To),
                           Result = conversionResult[0]
                       };
        }
    }
}
