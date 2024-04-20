using CaseChecker.MVVM.Core;
using CaseChecker.MVVM.Model;
using CaseChecker.MVVM.ViewModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CaseChecker.MVVM.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ResourceDictionary lang = [];
        public ResourceDictionary Lang
        {
            get => lang;
            set
            {
                lang = value;
                RaisePropertyChanged(nameof(Lang));
            }
        }

        private double remoteAppVersionDouble;
        public double RemoteAppVersionDouble
        {
            get => remoteAppVersionDouble;
            set
            {
                remoteAppVersionDouble = value;
                RaisePropertyChanged(nameof(RemoteAppVersionDouble));
            }
        }

        private Dictionary<string, bool> expandStatesLeft = [];
        private Dictionary<string, bool> expandStatesRight = [];
        public System.Timers.Timer _timer;
        public System.Timers.Timer _updateTimer;
        private static bool UpdateMessagePresented = false;

        public static event PropertyChangedEventHandler? PropertyChangedStatic;
        public event PropertyChangedEventHandler? PropertyChanged;

        public static void RaisePropertyChangedStatic([CallerMemberName] string? propertyname = null)
        {
            PropertyChangedStatic?.Invoke(typeof(ObservableObject), new PropertyChangedEventArgs(propertyname));
        }
        public void RaisePropertyChanged([CallerMemberName] string? propertyname = null)
        {
            PropertyChanged?.Invoke(typeof(ObservableObject), new PropertyChangedEventArgs(propertyname));
        }

        private static MainWindow? instance;
        public static MainWindow Instance
        {
            get => instance!;
            set
            {
                instance = value;
                RaisePropertyChangedStatic(nameof(Instance));
            }
        }
        
        
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;

            _timer = new System.Timers.Timer(2500);
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();

            _updateTimer = new System.Timers.Timer(10000);
            _updateTimer.Elapsed += UpdateTimer_Elapsed;
            _updateTimer.Start();

            PropertyGroupDescription groupDescription = new("SentOn");
            listViewLeft.Items.GroupDescriptions.Add(groupDescription);
            listViewRight.Items.GroupDescriptions.Add(groupDescription);

            if (LoginViewModel.Instance.AccessLevel.Equals("both", StringComparison.CurrentCultureIgnoreCase))
                this.Width = 1200;
            else
                this.Width = 500;

            SetLanguageDictionary();
        }

        private void UpdateTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            _updateTimer.Interval = (3600 * 1000);
            LookForUpdate();
        }

        private async void LookForUpdate()
        {
            double remoteVersion = 0;
            try
            {
                string result = await new HttpClient().GetStringAsync("https://raw.githubusercontent.com/aml-one/CaseChecker/master/CaseChecker/version.txt");
                _ = double.TryParse(result[..result.IndexOf('-')].Trim(), out remoteVersion);
                versionLabel.ToolTip = $"{(string)Lang["lastAvailableVersion"]}: {remoteVersion}";
            }
            catch (Exception)
            {
            }

            if (remoteVersion > MainViewModel.Instance.AppVersionDouble)
            {
                MainViewModel.Instance.UpdateAvailable = true;
                if (!UpdateMessagePresented)
                {
                    UpdateMessagePresented = true;
                    MessageBoxResult result = MessageBox.Show(this, (string)Lang["updateAvailableMessageBox"], (string)Lang["updateAvailableMessageBoxTitle"], MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        MainViewModel.Instance.StartProgramUpdate();
                    }
                }
            }
            else
                MainViewModel.Instance.UpdateAvailable = false;
        }

        public void SetLanguageDictionary(string language = "")
        {
            if (language.Equals(""))
            {
                Lang.Source = Thread.CurrentThread.CurrentCulture.ToString() switch
                {
                    "en-US" => new Uri("..\\..\\Lang\\StringResources_English.xaml", UriKind.Relative),
                    "zh-Hans" => new Uri("..\\..\\Lang\\StringResources_Chinese.xaml", UriKind.Relative),
                    "zh-Hant" => new Uri("..\\..\\Lang\\StringResources_Chinese.xaml", UriKind.Relative),
                    "zh-CHT" => new Uri("..\\..\\Lang\\StringResources_Chinese.xaml", UriKind.Relative),
                    "zh-CN" => new Uri("..\\..\\Lang\\StringResources_Chinese.xaml", UriKind.Relative),
                    "zh-CHS" => new Uri("..\\..\\Lang\\StringResources_Chinese.xaml", UriKind.Relative),
                    "zh-HK" => new Uri("..\\..\\Lang\\StringResources_Chinese.xaml", UriKind.Relative),
                    _ => new Uri("..\\..\\Lang\\StringResources_English.xaml", UriKind.Relative),
                };
            }
            else
            {
                try
                {
                    Lang.Source = new Uri("..\\..\\Lang\\StringResources_" + language + ".xaml", UriKind.Relative);
                }
                catch (IOException)
                {
                    Lang.Source = new Uri("..\\..\\Lang\\StringResources_English.xaml", UriKind.Relative);
                }
            }

            this.Resources.MergedDictionaries.Add(lang);
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    listViewLeft.Items.Refresh();
                    listViewRight.Items.Refresh();
                }));
            }
            catch (Exception)
            {
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LoginWindow.Instance.Close();
        }

        private void listViewLeft_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth; // take into account vertical scrollbar

           
            gView.Columns[0].Width = 34;
            gView.Columns[1].Width = workingWidth - 238;
            gView.Columns[2].Width = 30;
            gView.Columns[3].Width = 30;
            gView.Columns[4].Width = 30;
            gView.Columns[5].Width = 110;
        }

        private void listViewRight_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth; // take into account vertical scrollbar

            gView.Columns[0].Width = 34;
            gView.Columns[1].Width = workingWidth - 238;
            gView.Columns[2].Width = 30;
            gView.Columns[3].Width = 30;
            gView.Columns[4].Width = 30;
            gView.Columns[5].Width = 110;
        }

        private void ExpanderLeft_Loaded(object sender, RoutedEventArgs e)
        {
            var expander = (Expander)sender;
            var dc = (CollectionViewGroup)expander.DataContext;
            var groupName = dc.Name.ToString();
            if (expandStatesLeft.TryGetValue(groupName, out var value))
                expander.IsExpanded = value;
        }

        private void ExpanderLeft_ExpandedCollapsed(object sender, RoutedEventArgs e)
        {
            var expander = (Expander)sender;
            var dc = (CollectionViewGroup)expander.DataContext;
            var groupName = dc.Name.ToString();
            expandStatesLeft[groupName] = expander.IsExpanded;
        }
        
        private void ExpanderRight_Loaded(object sender, RoutedEventArgs e)
        {
            var expander = (Expander)sender;
            var dc = (CollectionViewGroup)expander.DataContext;
            var groupName = dc.Name.ToString();
            if (expandStatesRight.TryGetValue(groupName, out var value))
                expander.IsExpanded = value;
        }

        private void ExpanderRight_ExpandedCollapsed(object sender, RoutedEventArgs e)
        {
            var expander = (Expander)sender;
            var dc = (CollectionViewGroup)expander.DataContext;
            var groupName = dc.Name.ToString();
            expandStatesRight[groupName] = expander.IsExpanded;
        }

        public void TitleBar_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
                BtnMaximize_Click(sender, e);

            if (e.ChangedButton == MouseButton.Left)
                try
                {
                    this.DragMove();
                }
                catch { }
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                this.BorderThickness = new Thickness(0);
                btnMaximize.Content = "▣";
            }
            else if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                this.BorderThickness = new Thickness(6, 6, 6, 3);
                btnMaximize.Content = "⧉";
            }

        }

        private void BtnCloseApplication_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                this.BorderThickness = new Thickness(6, 6, 6, 0);
                btnMaximize.Content = "⧉";
            }
            else if (WindowState == WindowState.Normal)
            {
                this.BorderThickness = new Thickness(0);
                btnMaximize.Content = "▣";
            }
        }
    }
}
