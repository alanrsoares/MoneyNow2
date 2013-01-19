using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace MoneyNow2.ServiceInterface.Services
{
    [Route("/currencyinfos")]
    [Route("/currencyinfos/{Id}")]
    public class CurrencyInfos : IReturn<List<CurrencyInfo>>
    {
        public string Id { get; set; }
        public CurrencyInfos(string id)
        {
            Id = id;
        }
    }

    #region Model
    public class CurrencyInfo
    {
        public string RegionDisplayName { get; set; }
        public string RegionName { get; set; }
        public string RegionNativeName { get; set; }
        public string TwoLetterIsoRegionName { get; set; }
        public string CurrencyEnglishName { get; set; }
        public string CurrencyNativeName { get; set; }
        public string CurrencySymbol { get; set; }
        public string ISOCurrencySymbol { get; set; }
        public string FlagUrl
        {
            get
            {
                return ISOCurrencySymbol == "EUR"
                           ? string.Format("http://www.geonames.de/flag-eu.gif")
                           : string.Format("http://www.geonames.org/flags/x/{0}.gif", TwoLetterIsoRegionName.ToLower());
            }
        }

        public CurrencyInfo(string regionDisplayName, string regionEnglishName, string regionNativeName, string twoLetterIsoRegionName, string currencyEnglishName, string currencyNativeName, string currencySymbol, string isoCurrencySymbol)
        {
            RegionDisplayName = regionDisplayName;
            RegionName = regionEnglishName;
            RegionNativeName = regionNativeName;
            TwoLetterIsoRegionName = twoLetterIsoRegionName;
            CurrencyEnglishName = currencyEnglishName;
            CurrencyNativeName = currencyNativeName;
            CurrencySymbol = currencySymbol;
            ISOCurrencySymbol = isoCurrencySymbol;
        }
    }
    #endregion

    #region Repository
    public class CurrencyInfoRepository
    {
        public IList<CurrencyInfo> CurrencyInfos { get; set; }

        public CurrencyInfoRepository()
        {
            var validCurrencies = new[] { "AED", "ANG", "ARS", "AUD", "BDT", "BGN", "BHD", "BND", "BOB", "BRL", "BWP", "CAD", "CHF", "CLP", "CNY", "COP", "CRC", "CZK", "DKK", "DOP", "DZD", "EEK", "EGP", "EUR", "FJD", "GBP", "HKD", "HNL", "HRK", "HUF", "IDR", "ILS", "INR", "JMD", "JOD", "JPY", "KES", "KRW", "KWD", "KYD", "KZT", "LBP", "LKR", "LTL", "LVL", "MAD", "MDL", "MKD", "MUR", "MVR", "MXN", "MYR", "NAD", "NGN", "NIO", "NOK", "NPR", "NZD", "OMR", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG", "QAR", "RON", "RSD", "RUB", "SAR", "SCR", "SEK", "SGD", "SKK", "SLL", "SVC", "THB", "TND", "TRY", "TTD", "TWD", "TZS", "UAH", "UGX", "USD", "UYU", "UZS", "VEF", "VND", "XOF", "YER", "ZAR", "ZMK" };

            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(c => !c.IsNeutralCulture && c.Name.Length > 0);

            CurrencyInfos = new List<CurrencyInfo>();

            foreach (var region in cultures
                                    .Select(culture => new RegionInfo(culture.LCID))
                                    .Where(region =>
                                        CurrencyInfos.FirstOrDefault(r => r.ISOCurrencySymbol == region.ISOCurrencySymbol) == null
                                        && validCurrencies.Contains(region.ISOCurrencySymbol)))
            {
                CurrencyInfos.Add(new CurrencyInfo(
                                   region.DisplayName,
                                   region.EnglishName,
                                   region.NativeName,
                                   region.TwoLetterISORegionName,
                                   region.CurrencyEnglishName,
                                   region.CurrencyNativeName,
                                   region.CurrencySymbol,
                                   region.ISOCurrencySymbol));
            }
        }

        public IEnumerable<CurrencyInfo> GetAll()
        {
            return CurrencyInfos;
        }

        public CurrencyInfo GetById(string id)
        {
            return CurrencyInfos.FirstOrDefault(c => c.ISOCurrencySymbol == id);
        }
    }
    #endregion

    #region REST Service
    public class CurrencyInfoService : Service
    {
        public CurrencyInfoRepository Repository { get; set; }

        public object Any(CurrencyInfos currencyInfos)
        {
            return string.IsNullOrEmpty(currencyInfos.Id)
                       ? (object)Repository.GetAll()
                       : Repository.GetById(currencyInfos.Id);
        }
    }
    #endregion
}
