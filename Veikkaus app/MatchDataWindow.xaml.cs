using System;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;
using Veikkaus_app.Common;
using Veikkaus_app.JsonObjects;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;

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
            var client = new AppHttpClient();
            var matchId = PhoneApplicationService.Current.State["MatchId"].ToString();

            Task.Factory.StartNew(new Action(() =>
            {
                var currentMatchData = GetCurrentMatchData(client, matchId);
                UpdatePageUI(currentMatchData);
            }));
        }

        private static MatchData GetCurrentMatchData(AppHttpClient client, string matchId)
        {
            var fetchMatchDataTask = client.GetMatchDataAsync(matchId);
            fetchMatchDataTask.Wait();
            var currentMatchData = JsonMatchDeserializer.GetMatchDataFromJsonString(fetchMatchDataTask.Result);
            return currentMatchData;
        }

        private void UpdatePageUI(MatchData currentMatchData)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var match = currentMatchData.GetMatch();

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