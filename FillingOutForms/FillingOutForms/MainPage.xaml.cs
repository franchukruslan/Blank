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
        #region Fields

        private List<int> countrykeys;

        #endregion

        public enum TypeData
        {
            Country,
            Cities,
            University
        };

        public MainPage()
        {
            InitializeComponent();
            GetDataFromVk("https://api.vk.com/api.php?oauth=1&method=database.getCountries&v=5.5&need_all=1&count=400", TypeData.Country);
        }

        public async Task GetDataFromVk(string url, TypeData type)
        {
            string jsonString;
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                {
                    var stream = await httpClient.GetStreamAsync(url);
                    StreamReader reader = new StreamReader(stream);
                    jsonString = reader.ReadToEnd();

                    var elements = new Dictionary<int, string>();

                    var responseCountries = JArray.Parse(JObject.Parse(jsonString)["response"]["items"].ToString());

                    foreach (var countryInResponse in responseCountries)
                    {
                        elements.Add((int)countryInResponse["id"], (string)countryInResponse["title"]+" "+(string)countryInResponse["region"]);
                    }
                    switch(type)
                    {
                        case TypeData.Country:
                            if(elements==null)
                            {
                                await DisplayAlert("Ошибка", "Не удалось получить список стран. Проверте соединение с интернетом.", "OK");
                            }
                            else
                            {
                                addCountry(elements);
                                countrykeys = elements.Keys.ToList();
                            }
                            break;
                        case TypeData.Cities:
                            addCities(elements);
                            break;
                        case TypeData.University:
                            addUniversiti(elements);
                            break;
                    }
                }
            }
        }

        private void addCountry(Dictionary<int, string> listCountry)
        {

            foreach (var country in listCountry)
            {
                countryList.Items.Add(country.Value);
            }
        }

        private void addCities(Dictionary<int, string> cities)
        {
            listCities.Suggestions = cities.Values;
        }

        private void addUniversiti(Dictionary<int, string> universiti)
        {
            listUniversity.Suggestions = universiti.Values;
        }

        private void unFocusFirsName(object sender, EventArgs e)
        {
            Entry firsName = (Entry)sender;
            if (firsName.Text != "" && firsName.Text!=null)
            {
                lastName.IsEnabled = true;
            }
        }

        private void unFocusLastName(object sender, EventArgs e)
        {
            Entry lastName = (Entry)sender;
            if (lastName.Text != "" && lastName.Text != null)
            {
                countryList.IsEnabled = true;
            }
        }

        private async void listCities_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (listCities.Text != null && listCities.Text != "")
            {
                await GetDataFromVk("https://api.vk.com/api.php?oauth=1&method=database.getCities&v=5.5&q=" + listCities.Text + "&need_all=1&offset=0&count=10&country_id=" + countrykeys[countryList.SelectedIndex].ToString(), TypeData.Cities);
            }
        }

        private void countryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            listCities.IsAvailable = true;
        }

        private async void listUniversity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(listUniversity.Text != null && listUniversity.Text != "")
            {
                await GetDataFromVk("https://api.vk.com/api.php?oauth=1&method=database.getUniversities&v=5.5&count=10&q=" + listUniversity.Text, TypeData.University);
            }
        }

        private void listCities_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            listUniversity.IsAvailable = true;
        }

        private void listUniversity_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            completed.IsEnabled = true;
        }

        private async void completed_Clicked(object sender, EventArgs e)
        {
            if(checkekFields())
                await Navigation.PushAsync(new CompletedBlank(firstName.Text, lastName.Text, countryList.Items[countryList.SelectedIndex], listCities.Text, listUniversity.Text));
            else
            {
                await DisplayAlert("Ошибка", "Одно из полей не заполнено.", "OK");
            }
        }

        private bool checkekFields()
        {
            if (firstName.Text != null && firstName.Text != "" && lastName.Text != null && lastName.Text != "" && listCities.Text != null && listCities.Text != "" && listUniversity.Text != null && listUniversity.Text != "")
                return true;
            else return false;
        }
    }
}
