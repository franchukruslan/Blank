using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FillingOutForms
{
	public partial class MainPage : ContentPage
	{

        public class VkCountry
        {
            public int Id { get; }
            public string Title { get; }
            public VkCountry(int countryId, string countryTitle)
            {
                Id = countryId;
                Title = countryTitle;
            }
        }

        public MainPage()
        {
            InitializeComponent();
            Task<List<VkCountry>> listCountry = FetchAsync("https://api.vk.com/api.php?oauth=1&method=database.getCountries&v=5.5&need_all=1&count=400");
            //AddCountry(listCountry);
        }

        public void AddCountry(List<VkCountry> listCountry)
        {

            foreach (var country in listCountry)
            {
                countryList.Items.Add(country.Title);
            }
        }

        public async Task<List<VkCountry>> FetchAsync(string url)
        {
            string jsonString;
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                {
                    var stream = await httpClient.GetStreamAsync(url);
                    StreamReader reader = new StreamReader(stream);
                    jsonString = reader.ReadToEnd();

                    var listOfCountries = new List<VkCountry>();

                    var responseCountries = JArray.Parse(JObject.Parse(jsonString)["response"]["items"].ToString());

                    foreach (var countryInResponse in responseCountries)
                    {
                        var vkCountry = new VkCountry((int)countryInResponse["id"], (string)countryInResponse["title"]);

                        listOfCountries.Add(vkCountry);
                    }
                    AddCountry(listOfCountries);

                    return listOfCountries;
                }
            }
        }

        private void focusFirstName(object sender, EventArgs e)
        {
            Entry firsName = (Entry)sender;
            if (firsName.Text == "Имя")
            {
                firsName.Text = "";
            }
        }

        private void unFocusFirsName(object sender, EventArgs e)
        {
            Entry firsName = (Entry)sender;
            if (firsName.Text != "Имя" && firsName.Text != "")
            {
                lastName.IsEnabled = true;
            }
            else
            {
                firsName.Text = "Имя";
            }
        }

        private void focusLastName(object sender, EventArgs e)
        {
            Entry lastName = (Entry)sender;
            if (lastName.Text == "Фамилия")
            {
                lastName.Text = "";
                //System.Threading.Thread.Sleep(10000);
            }
        }

        private void unFocusLastName(object sender, EventArgs e)
        {
            Entry lastName = (Entry)sender;
            if (lastName.Text != "Фамилия" && lastName.Text != "")
            {
                countryList.IsEnabled = true;
            }
            else
            {
                lastName.Text = "Фамилия";
            }
        }

        private async void Completed_Clicked(object sender, EventArgs e)
        {
        }
    }
}
