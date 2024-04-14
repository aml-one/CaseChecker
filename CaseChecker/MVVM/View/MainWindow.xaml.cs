using CaseChecker.MVVM.Core;
using CaseChecker.MVVM.Model;
using CaseChecker.MVVM.ViewModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CaseChecker.MVVM.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Dictionary<string, bool> expandStatesLeft = [];
        private Dictionary<string, bool> expandStatesRight = [];
        public System.Timers.Timer _timer;

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

            PropertyGroupDescription groupDescription = new("SentOn");
            listViewLeft.Items.GroupDescriptions.Add(groupDescription);
            listViewRight.Items.GroupDescriptions.Add(groupDescription);

            if (LoginViewModel.Instance.AccessLevel.Equals("both", StringComparison.CurrentCultureIgnoreCase))
                this.Width = 1200;
            else
                this.Width = 500;
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
    }
}
