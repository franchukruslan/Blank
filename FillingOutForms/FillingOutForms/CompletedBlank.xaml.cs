using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FillingOutForms
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompletedBlank : ContentPage
	{
		public CompletedBlank (string fName, string lName, string countrys, string cities, string universities)
		{
			InitializeComponent ();
            firstName.Text = "Имя: "+fName;
            lastName.Text = "Фамилия: "+lName;
            country.Text = "Страна: "+countrys;
            city.Text = "Город: "+cities;
            university.Text = "Университет: "+universities;
		}
    }
}
