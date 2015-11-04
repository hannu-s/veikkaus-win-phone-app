using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Veikkaus_app.Resources;
using System.Threading.Tasks;
using Veikkaus_app;
using System.IO;
using System.Windows.Documents;
using System.Diagnostics;

namespace Veikkaus_app
{
    public partial class MainPage : PhoneApplicationPage
    {
        public static event EventHandler<CustomEventArgs> RaiseCustomEvent;
        private List<Match> matches;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            var client = new AppHttpClient();

            Task.Factory.StartNew(new Action(() =>
            {
                var fetchMatchesTask = client.GetMatchesAsync();

                fetchMatchesTask.Wait();

                matches = JsonMatchDeserializer.GetMatchListFromJsonString(fetchMatchesTask.Result);

                Console.WriteLine(matches);

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

            matchResult = new TextBlock();
            matchResult.Text = match.GetMatchResult();

            matchDate = new TextBlock();
            matchDate.Text = match.GetMatchDate();
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
            NavigationService.Navigate(new Uri("/MatchDataWindow.xaml", UriKind.Relative));

            var match = matches.Find(obj => obj.GetMatchId().Equals((sender as Button).Name));
            Task.Factory.StartNew(new Action(() =>
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                while (stopwatch.Elapsed < TimeSpan.FromSeconds(10))
                {
                    if (RaiseCustomEvent == null)
                        System.Threading.Thread.Sleep(10);
                    else
                    {
                        RaiseCustomEvent(this, new CustomEventArgs(match));
                        break;
                    }
                }
                stopwatch.Reset();
            }));
        }


        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}