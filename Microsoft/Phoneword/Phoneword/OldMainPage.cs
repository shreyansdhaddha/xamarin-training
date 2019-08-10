using System;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Phoneword
{
    public class OldMainPage : ContentPage
    {
        Entry phoneNumberText;
        Button translateButton;
        Button callButton;
        string translatedNumber;

        public OldMainPage()
        {
            this.Padding = new Thickness(20, 20, 20, 20);
            StackLayout panel = new StackLayout { Spacing = 15 };

            panel.Children.Add(new Label
            {
                Text = "Enter a Password:",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });
            panel.Children.Add(phoneNumberText = new Entry { Text = "1-855-XAMARIN" });
            panel.Children.Add(translateButton = new Button { Text = "Translate" });
            panel.Children.Add(callButton = new Button { Text = "Call", IsEnabled = false });

            translateButton.Clicked += OnTranslate;
            callButton.Clicked += OnCall;
            this.Content = panel;
        }

        async void OnCall(object sender, EventArgs e)
        {
            bool result = await this.DisplayAlert(
                            "Dial a Number",
                            "Would you like to call " + translatedNumber,
                            "Yes",
                            "No");

            if(result)
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
            if(!string.IsNullOrEmpty(translatedNumber))
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
