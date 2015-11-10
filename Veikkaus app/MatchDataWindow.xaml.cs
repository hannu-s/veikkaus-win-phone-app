using System;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;
using Veikkaus_app.Common;
using Veikkaus_app.JsonObjects;
using Microsoft.Phone.Shell;

namespace Veikkaus_app
{
    public partial class MatchDataWindow : PhoneApplicationPage
    {
        public MatchDataWindow()
        {
            InitializeComponent();

            PopulateMainPage();
        }

        private void PopulateMainPage()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var matchDataObj = PhoneApplicationService.Current.State["MatchData"] as MatchData;
                var match = matchDataObj.GetMatch();

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