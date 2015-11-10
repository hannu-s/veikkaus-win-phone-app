using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Threading.Tasks;
using System.Diagnostics;
using Veikkaus_app.JsonObjects;
using Veikkaus_app.Common;
using Microsoft.Phone.Shell;

namespace Veikkaus_app
{
    public partial class MainPage : PhoneApplicationPage
    {
        private List<Match> matches;

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

        private void PopulateMatchItemsControl(List<Match> matches)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                SwapLoadingTextToMatchItemsControl();

                matches.ForEach(match =>
                {
                    Button btn = CreateItemsControlButton(match);
                    Grid contentGrid = CreateButtonsContentGrid();
                    TextBlock matchName, matchResult, matchDate;

                    CreateMatchDataTextBlocks(match, out matchName, out matchResult, out matchDate);
                    ApplyTextBlocksToContentGrid(contentGrid, matchName, matchResult, matchDate);

                    btn.Content = contentGrid;
                    MatchItemsControl.Items.Add(btn);
                });
            }));
        }

        private void SwapLoadingTextToMatchItemsControl()
        {
            LoadingText.Visibility = Visibility.Collapsed;
            MatchItemsControl.Visibility = Visibility.Visible;
        }

        private void SwapItemsControlToLoadingText()
        {
            LoadingText.Visibility = Visibility.Visible;
            MatchItemsControl.Visibility = Visibility.Collapsed;
        }

        private Button CreateItemsControlButton(Match match)
        {
            var btn = new Button();
            btn.Name = match.GetMatchId();
            btn.Tap += Btn_Tap;
            return btn;
        }

        private static void CreateMatchDataTextBlocks(Match match, out TextBlock matchName, out TextBlock matchResult, out TextBlock matchDate)
        {
            matchName = new TextBlock();
            matchName.Text = match.GetMatchName();
            matchName.HorizontalAlignment = HorizontalAlignment.Center;

            matchResult = new TextBlock();
            matchResult.Text = match.GetMatchResult();
            matchResult.HorizontalAlignment = HorizontalAlignment.Center;

            matchDate = new TextBlock();
            matchDate.Text = match.GetMatchDate();
            matchDate.HorizontalAlignment = HorizontalAlignment.Center;
        }

        private static void ApplyTextBlocksToContentGrid(Grid contentGrid, TextBlock matchName, TextBlock matchResult, TextBlock matchDate)
        {
            Grid.SetRow(matchName, 0);
            Grid.SetRow(matchResult, 1);
            Grid.SetRow(matchDate, 2);

            Grid.SetColumn(matchName, 0);
            Grid.SetColumn(matchResult, 0);
            Grid.SetColumn(matchDate, 0);

            contentGrid.Children.Add(matchName);
            contentGrid.Children.Add(matchResult);
            contentGrid.Children.Add(matchDate);
        }

        private static Grid CreateButtonsContentGrid()
        {
            var contentGrid = new Grid();
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition());

            contentGrid.RowDefinitions.Add(new RowDefinition());
            contentGrid.RowDefinitions.Add(new RowDefinition());
            contentGrid.RowDefinitions.Add(new RowDefinition());
            return contentGrid;
        }

        private void Btn_Tap(object sender, RoutedEventArgs e)
        {
            SwapItemsControlToLoadingText();
            var match = matches.Find(obj => obj.GetMatchId().Equals((sender as Button).Name));
            Task<MatchData> matchDataTask = null;

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