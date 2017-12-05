using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json.Linq;

namespace SimpleRestApi.Views
{
    public class CurrencyBase
    {
        public string Base { get; set; }
        public string Date { get; set; }
        public string Rate { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExchangeRate : ContentPage
	{
        public ObservableCollection<CurrencyBase> Items { get; set; }

        public ExchangeRate ()
		{
			InitializeComponent();

            GetExchangeRate();

            BindingContext = this;
        }

        public void GetExchangeRate()
        {
            try
            {
                var url = "http://api.fixer.io/latest";
                var request = WebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";
                request.Timeout = 15000;

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var content = reader.ReadToEnd();

                        var baseCurrency = JObject.Parse(content)["base"];
                        var date = JObject.Parse(content)["date"];
                        var properties = JObject.Parse(content)["rates"];

                        Items = new ObservableCollection<CurrencyBase>
                        {
                            new CurrencyBase { Base = $"1 {baseCurrency.Value<string>()} equals", Date = date.Value<string>(), Rate = $"{properties["ZAR"].Value<string>()} South African Rand" },
                            new CurrencyBase { Base = $"1 {baseCurrency.Value<string>()} equals", Date = date.Value<string>(), Rate = $"{properties["USD"].Value<string>()} US Dollar" },
                            new CurrencyBase { Base = $"1 {baseCurrency.Value<string>()} equals", Date = date.Value<string>(), Rate = $"{properties["GBP"].Value<string>()} British Pound" }
                        };                       
                    }
                }
            }
            catch (Exception)
            {
                Items = new ObservableCollection<CurrencyBase>
                {
                   new CurrencyBase { Base = "Updating ZAR", Date = "", Rate = $"EU Central Bank Updating Information" },
                   new CurrencyBase { Base = "Updating USD", Date = "", Rate = $"EU Central Bank Updating Information" },
                   new CurrencyBase { Base = "Updating GBP", Date = "", Rate = $"EU Central Bank Updating Information" }
                };
            }
        }
    }
}