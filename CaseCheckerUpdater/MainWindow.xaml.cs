using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace CaseCheckerUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public System.Timers.Timer _timer;

        private ResourceDictionary lang = [];
        public ResourceDictionary Lang
        {
            get => lang;
            set
            {
                lang = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            SetLanguageDictionary();

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += Timer_Elapsed;
        }

        public void SetLanguageDictionary(string language = "")
        {
            if (language.Equals(""))
            {
                Lang.Source = Thread.CurrentThread.CurrentCulture.ToString() switch
                {
                    "en-US" => new Uri("\\Lang\\StringResources_English.xaml", UriKind.Relative),
                    "zh-Hans" => new Uri("\\Lang\\StringResources_Chinese.xaml", UriKind.Relative),
                    _ => new Uri("\\Lang\\StringResources_English.xaml", UriKind.Relative),
                };
            }
            else
            {
                try
                {
                    Lang.Source = new Uri("\\Lang\\StringResources_" + language + ".xaml", UriKind.Relative);
                }
                catch (IOException)
                {
                    Lang.Source = new Uri("\\Lang\\StringResources_English.xaml", UriKind.Relative);
                }
            }

            this.Resources.MergedDictionaries.Add(lang);
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            StopMainApp();
        }

        private void StopMainApp()
        {
            var Processes = Process.GetProcesses()
                               .Where(pr => pr.ProcessName == "CaseChecker");
            foreach (var process in Processes)
            {
                process.Kill();
            }
            
            

            Task.Run(DownloadUpdate).Wait();
            
            StartCaseApp();
        }

        private async void DownloadUpdate()
        {
            Thread.Sleep(2000);

            string appPath = Path.GetDirectoryName(AppContext.BaseDirectory);
            try
            {
                Thread.Sleep(500);
                if (File.Exists($@"{appPath}\CaseChecker_old.exe"))
                    File.Delete($@"{appPath}\CaseChecker_old.exe");
                Thread.Sleep(500);
                if (File.Exists($@"{appPath}\CaseChecker.exe"))
                    File.Move($@"{appPath}\CaseChecker.exe", $@"{appPath}\CaseChecker_old.exe");
                Thread.Sleep(2000);
                using var client = new HttpClient();
                using var s = await client.GetStreamAsync("https://raw.githubusercontent.com/aml-one/CaseChecker/master/CaseChecker/Executable/CaseChecker.exe");
                using var fs = new FileStream($@"{appPath}\CaseChecker.exe", FileMode.OpenOrCreate);
                await s.CopyToAsync(fs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                if (File.Exists($@"{appPath}\CaseChecker_old.exe"))
                    File.Move($@"{appPath}\CaseChecker_old.exe", $@"{appPath}\CaseChecker.exe");
            }

            Thread.Sleep(3000);
        }

        private void StartCaseApp()
        {
            Thread.Sleep(3000);
            try
            {
                string appPath = Path.GetDirectoryName(AppContext.BaseDirectory);

                var p = new Process();

                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = $"/c \"{appPath}\\CaseChecker.exe\" updated";
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();

                Thread.Sleep(2000);
                CloseThisApp();
            }
            catch (Exception)
            {
                MessageBox.Show((string)Lang["couldNotStart"], (string)Lang["error"], MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void CloseThisApp()
        {
            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        private void Label_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show((string)Lang["closeMessage"], (string)Lang["caseCheckerUpdater"], MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }
    }
}