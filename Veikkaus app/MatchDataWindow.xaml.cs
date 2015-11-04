using System;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;

namespace Veikkaus_app
{
    public partial class MatchDataWindow : PhoneApplicationPage
    {
        public MatchDataWindow()
        {
            InitializeComponent();
            MainPage.RaiseCustomEvent += new EventHandler<CustomEventArgs>(MainPage_RaiseCustomEvent);
        }

        private void MainPage_RaiseCustomEvent(object sender, CustomEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                HomeLogo.Source = new BitmapImage(e.Match.GetHomeTeamLogoUri());
                AwayLogo.Source = new BitmapImage(e.Match.GetAwayTeamLogoUri());

                HomeTeamName.Text = e.Match.GetHomeTeamName();
                AwayTeamName.Text = e.Match.GetAwayTeamName();

                HomeGoals.Text = e.Match.HomeGoals.ToString();
                AwayGoals.Text = e.Match.AwayGoals.ToString();

                MatchDate.Text = e.Match.GetMatchDate();
            }));
            
        }

        private void BackButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}