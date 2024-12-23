using System.Diagnostics;

namespace SpaceWar
{
    public partial class MainPage : ContentPage
    {
        public static double MyFontSize = 20; // Örnek font boyutu

        public MainPage()
        {
            InitializeComponent();
        }

        private async void StarterBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GamePage());
        }
        private async void SettingBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

    }

    public class GlobalFontSizeExtension : IMarkupExtension
    {
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return MainPage.MyFontSize;
        }
    }
}
