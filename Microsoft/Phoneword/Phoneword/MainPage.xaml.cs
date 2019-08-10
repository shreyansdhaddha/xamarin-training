using System;
using System.Collections.Generic;
using Xamarin.Essentials;

using Xamarin.Forms;

namespace Phoneword
{
    public partial class MainPage : ContentPage
    {
        string translatedNumber;

        public MainPage()
        {
            InitializeComponent();
        }

        async void OnCall(object sender, EventArgs e)
        {
            bool result = await this.DisplayAlert(
                            "Dial a Number",
                            "Would you like to call " + translatedNumber,
                            "Yes",
                            "No");

            if (result)
            {
                try
                {
                    PhoneDialer.Open(translatedNumber);
                }
                catch (ArgumentException)
                {
                    await DisplayAlert("Unable to Dial", "Phone number was not valid.", "OK");
                }
                catch (FeatureNotSupportedException)
                {
                    await DisplayAlert("Unable to Dial", "Phone dialing is not supported.", "OK");
                }
                catch (Exception)
                {
                    //Other error has occurred
                    await DisplayAlert("Unable to dial", "Phone dialing failed.", "OK");
                }

            }
        }

        private void OnTranslate(object sender, EventArgs e)
        {
            string enteredNumber = phoneNumberText.Text;
            translatedNumber = Phoneword.PhonewordTranslator.ToNumber(enteredNumber);
            if (!string.IsNullOrEmpty(translatedNumber))
            {
                callButton.IsEnabled = true;
                callButton.Text = "Call " + translatedNumber;
            }
            else
            {
                callButton.IsEnabled = false;
                callButton.Text = "Call";
            }
        }
    }
}
