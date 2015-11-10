using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Threading.Tasks;
using Veikkaus_app.JsonObjects;
using Veikkaus_app.Common;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;

namespace Veikkaus_app
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ObservableCollection<Match> matches;
        
        public MainPage()
        {
            InitializeComponent();
            var client = new AppHttpClient();

            Task.Factory.StartNew(new Action(() =>
            {
                var fetchMatchesTask = client.GetMatchesAsync();
                fetchMatchesTask.Wait();
                matches = JsonMatchDeserializer.GetMatchListFromJsonString(fetchMatchesTask.Result);
                PopulateMatchItemsControl(matches);
            }));
        }

        private void PopulateMatchItemsControl(ObservableCollection<Match> matches)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                IC1.ItemsSource = matches;
                SwapLoadingTextToMatchItemsControl();
            }));
        }

        private void SwapLoadingTextToMatchItemsControl()
        {
            LoadingText.Visibility = Visibility.Collapsed;
            IC1.Visibility = Visibility.Visible;
        }

        private void SwapItemsControlToLoadingText()
        {
            LoadingText.Visibility = Visibility.Visible;
            IC1.Visibility = Visibility.Collapsed;
        }

        private void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Match match = null;
            Task<MatchData> matchDataTask = null;
            var btnTag = (sender as Button).Tag.ToString();

            Dispatcher.BeginInvoke(new Action(() => SwapItemsControlToLoadingText()));

            foreach (var iter in matches)
            {
                var matchId = iter.GetMatchId();
                if (matchId.Equals(btnTag))
                {
                    match = iter;
                    break;
                }
            }

            Task.Factory.StartNew(new Action(() =>
            {
                matchDataTask = match.GetMatchDataAsync();
                matchDataTask.Wait();

            })).ContinueWith(task =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    SwapLoadingTextToMatchItemsControl();
                    PhoneApplicationService.Current.State["MatchData"] = matchDataTask.Result;
                    NavigationService.Navigate(new Uri("/MatchDataWindow.xaml", UriKind.Relative));
                }));
            });
        }
    }
}