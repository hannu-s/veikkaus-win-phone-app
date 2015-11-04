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
                var match = e.MatchData.GetMatch();

                HomeLogo.Source = new BitmapImage(match.GetHomeTeamLogoUri());
                AwayLogo.Source = new BitmapImage(match.GetAwayTeamLogoUri());

                HomeTeamName.Text = match.GetHomeTeamName();
                AwayTeamName.Text = match.GetAwayTeamName();

                HomeGoals.Text = match.HomeGoals.ToString();
                AwayGoals.Text = match.AwayGoals.ToString();

                MatchDate.Text = match.GetMatchDate();
            }));
            
        }

        private void BackButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}