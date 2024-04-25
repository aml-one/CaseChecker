using CaseChecker.MVVM.Core;
using CaseChecker.MVVM.Model;
using CaseChecker.MVVM.View;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using static CaseChecker.MVVM.ViewModel.LoginViewModel;

namespace CaseChecker.MVVM.ViewModel;

public partial class MainViewModel : ObservableObject
{
    public System.Timers.Timer _timer;
    public System.Timers.Timer _orderTimer;
    public System.Timers.Timer _countdownClock;
    private int Counter = 10;
    Dictionary<string, double> UnitDoubles = [];

    #region Properties

    private static MainViewModel? instance;
    public static MainViewModel Instance
    {
        get => instance;
        set
        {
            instance = value;
            RaisePropertyChangedStatic(nameof(Instance));
        }
    }

    private static ResourceDictionary lang = [];
    public static ResourceDictionary Lang
    {
        get => lang;
        set
        {
            lang = value;
            RaisePropertyChangedStatic(nameof(Lang));
        }
    }

    private string appVersion = "v1.0";
    public string AppVersion
    {
        get => appVersion;
        set
        {
            appVersion = value;
            RaisePropertyChanged(nameof(AppVersion));
        }
    }

    private double appVersionDouble;
    public double AppVersionDouble
    {
        get => appVersionDouble;
        set
        {
            appVersionDouble = value;
            RaisePropertyChanged(nameof(AppVersionDouble));
        }
    }

    private bool updateAvailable = false;
    public bool UpdateAvailable
    {
        get => updateAvailable;
        set
        {
            updateAvailable = value;
            RaisePropertyChanged(nameof(UpdateAvailable));
        }
    }

    private string accessLevel = "";
    public string AccessLevel
    {
        get => accessLevel;
        set
        {
            accessLevel = value;
            RaisePropertyChanged(nameof(AccessLevel));
        }
    }

    private string siteID = "";
    public string SiteID
    {
        get => siteID;
        set
        {
            siteID = value;
            RaisePropertyChanged(nameof(SiteID));
        }
    }

    private string deviceId = "";
    public string DeviceId
    {
        get => deviceId;
        set
        {
            deviceId = value;
            RaisePropertyChanged(nameof(DeviceId));
        }
    }

    private string serverAddress = "";
    public string ServerAddress
    {
        get => serverAddress;
        set
        {
            serverAddress = value;
            RaisePropertyChanged(nameof(ServerAddress));
        }
    }

    private string exportClockCountDown = "0:00";
    public string ExportClockCountDown
    {
        get => exportClockCountDown;
        set
        {
            exportClockCountDown = value;
            RaisePropertyChanged(nameof(ExportClockCountDown));
        }
    }

    private List<string> debugMessages = [];
    public List<string> DebugMessages
    {
        get => debugMessages;
        set
        {
            debugMessages = value;
            RaisePropertyChanged(nameof(DebugMessages));
        }
    }
    
    private Visibility debugShows = Visibility.Collapsed;
    public Visibility DebugShows
    {
        get => debugShows;
        set
        {
            debugShows = value;
            RaisePropertyChanged(nameof(DebugShows));
        }
    }

    private StatsDBSettingsModel? serverInfoModel;
    public StatsDBSettingsModel ServerInfoModel
    {
        get => serverInfoModel!;
        set
        {
            serverInfoModel = value;
            RaisePropertyChanged(nameof(ServerInfoModel));
        }
    }

    private List<SentOutCasesModel> sentOutCasesModelLeftSide = [];
    public List<SentOutCasesModel> SentOutCasesModelLeftSide
    {
        get => sentOutCasesModelLeftSide;
        set
        {
            sentOutCasesModelLeftSide = value;
            RaisePropertyChanged(nameof(SentOutCasesModelLeftSide));
        }
    }
    
    private List<SentOutCasesModel> sentOutCasesModelRightSide = [];
    public List<SentOutCasesModel> SentOutCasesModelRightSide
    {
        get => sentOutCasesModelRightSide;
        set
        {
            sentOutCasesModelRightSide = value;
            RaisePropertyChanged(nameof(SentOutCasesModelRightSide));
        }
    }


    private int columnAccess = 0;
    public int ColumnAccess
    {
        get => columnAccess;
        set
        {
            columnAccess = value;
            RaisePropertyChanged(nameof(ColumnAccess));
        }
    }
    
    private string lastDBUpdate = "";
    public string LastDBUpdate
    {
        get => lastDBUpdate;
        set
        {
            lastDBUpdate = value;
            RaisePropertyChanged(nameof(LastDBUpdate));
        }
    }
    
    private string lastDBUpdateLocalTime = "Fetching data..";
    public string LastDBUpdateLocalTime
    {
        get => lastDBUpdateLocalTime;
        set
        {
            lastDBUpdateLocalTime = value;
            RaisePropertyChanged(nameof(LastDBUpdateLocalTime));
        }
    }
    
    private bool serverIsOnline = true;
    public bool ServerIsOnline
    {
        get => serverIsOnline;
        set
        {
            serverIsOnline = value;
            RaisePropertyChanged(nameof(ServerIsOnline));
        }
    }
    
    private bool autoSend0 = false;
    public bool AutoSend0
    {
        get => autoSend0;
        set
        {
            autoSend0 = value;
            RaisePropertyChanged(nameof(AutoSend0));
        }
    }
    
    private bool autoSend15 = false;
    public bool AutoSend15
    {
        get => autoSend15;
        set
        {
            autoSend15 = value;
            RaisePropertyChanged(nameof(AutoSend15));
        }
    }
    
    private bool autoSend30 = false;
    public bool AutoSend30
    {
        get => autoSend30;
        set
        {
            autoSend30 = value;
            RaisePropertyChanged(nameof(AutoSend30));
        }
    }
    
    private bool autoSend45 = false;
    public bool AutoSend45
    {
        get => autoSend45;
        set
        {
            autoSend45 = value;
            RaisePropertyChanged(nameof(AutoSend45));
        }
    }
    
    private bool canResetCounter = false;
    public bool CanResetCounter
    {
        get => canResetCounter;
        set
        {
            canResetCounter = value;
            RaisePropertyChanged(nameof(CanResetCounter));
        }
    }
    
    private double updateTimeOpacity = 1;
    public double UpdateTimeOpacity
    {
        get => updateTimeOpacity;
        set
        {
            updateTimeOpacity = value;
            RaisePropertyChanged(nameof(UpdateTimeOpacity));
        }
    }
    
    private string language = "English";
    public string Language
    {
        get => language;
        set
        {
            language = value;
            RaisePropertyChanged(nameof(Language));
        }
    }
    
    private string progressBarColor = "LightGreen";
    public string ProgressBarColor
    {
        get => progressBarColor;
        set
        {
            progressBarColor = value;
            RaisePropertyChanged(nameof(ProgressBarColor));
        }
    }
    
    private string statusColor = "LightGreen";
    public string StatusColor
    {
        get => statusColor;
        set
        {
            statusColor = value;
            RaisePropertyChanged(nameof(StatusColor));
        }
    }
    
    private string updateTimeColor = "LightGreen";
    public string UpdateTimeColor
    {
        get => updateTimeColor;
        set
        {
            updateTimeColor = value;
            RaisePropertyChanged(nameof(UpdateTimeColor));
        }
    }

    #region units

    private double totalUnitsLeftSide = 0;
    public double TotalUnitsLeftSide
    {
        get => totalUnitsLeftSide;
        set
        {
            totalUnitsLeftSide = value;
            RaisePropertyChanged(nameof(TotalUnitsLeftSide));
        }
    }

    private double totalUnitsTodayLeftSide = 0;
    public double TotalUnitsTodayLeftSide
    {
        get => totalUnitsTodayLeftSide;
        set
        {
            totalUnitsTodayLeftSide = value;
            RaisePropertyChanged(nameof(TotalUnitsTodayLeftSide));
        }
    }
    
    private double totalUnitsLeftOverLeftSide = 0;
    public double TotalUnitsLeftOverLeftSide
    {
        get => totalUnitsLeftOverLeftSide;
        set
        {
            totalUnitsLeftOverLeftSide = value;
            RaisePropertyChanged(nameof(TotalUnitsLeftOverLeftSide));
        }
    }

    

    private double totalCrownsLeftSide = 0;
    public double TotalCrownsLeftSide
    {
        get => totalCrownsLeftSide;
        set
        {
            totalCrownsLeftSide = value;
            RaisePropertyChanged(nameof(TotalCrownsLeftSide));
        }
    }
    
    private double totalAbutmentsLeftSide = 0;
    public double TotalAbutmentsLeftSide
    {
        get => totalAbutmentsLeftSide;
        set
        {
            totalAbutmentsLeftSide = value;
            RaisePropertyChanged(nameof(TotalAbutmentsLeftSide));
        }
    }
    
    private double totalOrdersLeftSide = 0;
    public double TotalOrdersLeftSide
    {
        get => totalOrdersLeftSide;
        set
        {
            totalOrdersLeftSide = value;
            RaisePropertyChanged(nameof(TotalOrdersLeftSide));
        }
    }
    
    private double totalOrdersTodayLeftSide = 0;
    public double TotalOrdersTodayLeftSide
    {
        get => totalOrdersTodayLeftSide;
        set
        {
            totalOrdersTodayLeftSide = value;
            RaisePropertyChanged(nameof(TotalOrdersTodayLeftSide));
        }
    }
    
    private double totalOrdersLeftOversLeftSide = 0;
    public double TotalOrdersLeftOversLeftSide
    {
        get => totalOrdersLeftOversLeftSide;
        set
        {
            totalOrdersLeftOversLeftSide = value;
            RaisePropertyChanged(nameof(TotalOrdersLeftOversLeftSide));
        }
    }

    private double totalUnitsRightSide = 0;
    public double TotalUnitsRightSide
    {
        get => totalUnitsRightSide;
        set
        {
            totalUnitsRightSide = value;
            RaisePropertyChanged(nameof(TotalUnitsRightSide));
        }
    }

    private double totalUnitsTodayRightSide = 0;
    public double TotalUnitsTodayRightSide
    {
        get => totalUnitsTodayRightSide;
        set
        {
            totalUnitsTodayRightSide = value;
            RaisePropertyChanged(nameof(TotalUnitsTodayRightSide));
        }
    }
    
    private double totalUnitsLeftOverRightSide = 0;
    public double TotalUnitsLeftOverRightSide
    {
        get => totalUnitsLeftOverRightSide;
        set
        {
            totalUnitsLeftOverRightSide = value;
            RaisePropertyChanged(nameof(TotalUnitsLeftOverRightSide));
        }
    }

    private double totalCrownsRightSide = 0;
    public double TotalCrownsRightSide
    {
        get => totalCrownsRightSide;
        set
        {
            totalCrownsRightSide = value;
            RaisePropertyChanged(nameof(TotalCrownsRightSide));
        }
    }

    private double totalAbutmentsRightSide = 0;
    public double TotalAbutmentsRightSide
    {
        get => totalAbutmentsRightSide;
        set
        {
            totalAbutmentsRightSide = value;
            RaisePropertyChanged(nameof(TotalAbutmentsRightSide));
        }
    }

    private double totalOrdersRightSide = 0;
    public double TotalOrdersRightSide
    {
        get => totalOrdersRightSide;
        set
        {
            totalOrdersRightSide = value;
            RaisePropertyChanged(nameof(TotalOrdersRightSide));
        }
    }

    private double totalOrdersTodayRightSide = 0;
    public double TotalOrdersTodayRightSide
    {
        get => totalOrdersTodayRightSide;
        set
        {
            totalOrdersTodayRightSide = value;
            RaisePropertyChanged(nameof(TotalOrdersTodayRightSide));
        }
    }

    private double totalOrdersLeftOversRightSide = 0;
    public double TotalOrdersLeftOversRightSide
    {
        get => totalOrdersLeftOversRightSide;
        set
        {
            totalOrdersLeftOversRightSide = value;
            RaisePropertyChanged(nameof(TotalOrdersLeftOversRightSide));
        }
    }

    private double totalUnitsLeftSideFinal = 0;
    public double TotalUnitsLeftSideFinal
    {
        get => totalUnitsLeftSideFinal;
        set
        {
            totalUnitsLeftSideFinal = value;
            RaisePropertyChanged(nameof(TotalUnitsLeftSideFinal));
        }
    }

    private double totalUnitsTodayLeftSideFinal = 0;
    public double TotalUnitsTodayLeftSideFinal
    {
        get => totalUnitsTodayLeftSideFinal;
        set
        {
            totalUnitsTodayLeftSideFinal = value;
            RaisePropertyChanged(nameof(TotalUnitsTodayLeftSideFinal));
        }
    }

    private double totalUnitsLeftOverLeftSideFinal = 0;
    public double TotalUnitsLeftOverLeftSideFinal
    {
        get => totalUnitsLeftOverLeftSideFinal;
        set
        {
            totalUnitsLeftOverLeftSideFinal = value;
            RaisePropertyChanged(nameof(TotalUnitsLeftOverLeftSideFinal));
        }
    }



    private double totalCrownsLeftSideFinal = 0;
    public double TotalCrownsLeftSideFinal
    {
        get => totalCrownsLeftSideFinal;
        set
        {
            totalCrownsLeftSideFinal = value;
            RaisePropertyChanged(nameof(TotalCrownsLeftSideFinal));
        }
    }

    private double totalAbutmentsLeftSideFinal = 0;
    public double TotalAbutmentsLeftSideFinal
    {
        get => totalAbutmentsLeftSideFinal;
        set
        {
            totalAbutmentsLeftSideFinal = value;
            RaisePropertyChanged(nameof(TotalAbutmentsLeftSideFinal));
        }
    }

    private double totalOrdersLeftSideFinal = 0;
    public double TotalOrdersLeftSideFinal
    {
        get => totalOrdersLeftSideFinal;
        set
        {
            totalOrdersLeftSideFinal = value;
            RaisePropertyChanged(nameof(TotalOrdersLeftSideFinal));
        }
    }

    private double totalOrdersTodayLeftSideFinal = 0;
    public double TotalOrdersTodayLeftSideFinal
    {
        get => totalOrdersTodayLeftSideFinal;
        set
        {
            totalOrdersTodayLeftSideFinal = value;
            RaisePropertyChanged(nameof(TotalOrdersTodayLeftSideFinal));
        }
    }

    private double totalOrdersLeftOversLeftSideFinal = 0;
    public double TotalOrdersLeftOversLeftSideFinal
    {
        get => totalOrdersLeftOversLeftSideFinal;
        set
        {
            totalOrdersLeftOversLeftSideFinal = value;
            RaisePropertyChanged(nameof(TotalOrdersLeftOversLeftSideFinal));
        }
    }

    private double totalUnitsRightSideFinal = 0;
    public double TotalUnitsRightSideFinal
    {
        get => totalUnitsRightSideFinal;
        set
        {
            totalUnitsRightSideFinal = value;
            RaisePropertyChanged(nameof(TotalUnitsRightSideFinal));
        }
    }

    private double totalUnitsTodayRightSideFinal = 0;
    public double TotalUnitsTodayRightSideFinal
    {
        get => totalUnitsTodayRightSideFinal;
        set
        {
            totalUnitsTodayRightSideFinal = value;
            RaisePropertyChanged(nameof(TotalUnitsTodayRightSideFinal));
        }
    }

    private double totalUnitsLeftOverRightSideFinal = 0;
    public double TotalUnitsLeftOverRightSideFinal
    {
        get => totalUnitsLeftOverRightSideFinal;
        set
        {
            totalUnitsLeftOverRightSideFinal = value;
            RaisePropertyChanged(nameof(TotalUnitsLeftOverRightSideFinal));
        }
    }

    private double totalCrownsRightSideFinal = 0;
    public double TotalCrownsRightSideFinal
    {
        get => totalCrownsRightSideFinal;
        set
        {
            totalCrownsRightSideFinal = value;
            RaisePropertyChanged(nameof(TotalCrownsRightSideFinal));
        }
    }

    private double totalAbutmentsRightSideFinal = 0;
    public double TotalAbutmentsRightSideFinal
    {
        get => totalAbutmentsRightSideFinal;
        set
        {
            totalAbutmentsRightSideFinal = value;
            RaisePropertyChanged(nameof(TotalAbutmentsRightSideFinal));
        }
    }

    private double totalOrdersRightSideFinal = 0;
    public double TotalOrdersRightSideFinal
    {
        get => totalOrdersRightSideFinal;
        set
        {
            totalOrdersRightSideFinal = value;
            RaisePropertyChanged(nameof(TotalOrdersRightSideFinal));
        }
    }

    private double totalOrdersTodayRightSideFinal = 0;
    public double TotalOrdersTodayRightSideFinal
    {
        get => totalOrdersTodayRightSideFinal;
        set
        {
            totalOrdersTodayRightSideFinal = value;
            RaisePropertyChanged(nameof(TotalOrdersTodayRightSideFinal));
        }
    }

    private double totalOrdersLeftOversRightSideFinal = 0;
    public double TotalOrdersLeftOversRightSideFinal
    {
        get => totalOrdersLeftOversRightSideFinal;
        set
        {
            totalOrdersLeftOversRightSideFinal = value;
            RaisePropertyChanged(nameof(TotalOrdersLeftOversRightSideFinal));
        }
    }

    #endregion units

    private Visibility totalUnitsTodaySameAsAllTimeTotalLeftSide = Visibility.Visible;
    public Visibility TotalUnitsTodaySameAsAllTimeTotalLeftSide
    {
        get => totalUnitsTodaySameAsAllTimeTotalLeftSide;
        set
        {
            totalUnitsTodaySameAsAllTimeTotalLeftSide = value;
            RaisePropertyChanged(nameof(TotalUnitsTodaySameAsAllTimeTotalLeftSide));
        }
    }
    
    private Visibility totalOrdersTodaySameAsAllTimeTotalLeftSide = Visibility.Visible;
    public Visibility TotalOrdersTodaySameAsAllTimeTotalLeftSide
    {
        get => totalOrdersTodaySameAsAllTimeTotalLeftSide;
        set
        {
            totalOrdersTodaySameAsAllTimeTotalLeftSide = value;
            RaisePropertyChanged(nameof(TotalOrdersTodaySameAsAllTimeTotalLeftSide));
        }
    }
    
    private Visibility totalUnitsTodaySameAsAllTimeTotalRightSide = Visibility.Visible;
    public Visibility TotalUnitsTodaySameAsAllTimeTotalRightSide
    {
        get => totalUnitsTodaySameAsAllTimeTotalRightSide;
        set
        {
            totalUnitsTodaySameAsAllTimeTotalRightSide = value;
            RaisePropertyChanged(nameof(TotalUnitsTodaySameAsAllTimeTotalRightSide));
        }
    }
    
    private Visibility totalOrdersTodaySameAsAllTimeTotalRightSide = Visibility.Visible;
    public Visibility TotalOrdersTodaySameAsAllTimeTotalRightSide
    {
        get => totalOrdersTodaySameAsAllTimeTotalRightSide;
        set
        {
            totalOrdersTodaySameAsAllTimeTotalRightSide = value;
            RaisePropertyChanged(nameof(TotalOrdersTodaySameAsAllTimeTotalRightSide));
        }
    }

    private int orderByIndex = 0;
    public int OrderByIndex
    {
        get => orderByIndex;
        set
        {
            orderByIndex = value;
            RaisePropertyChanged(nameof(OrderByIndex));
            SortOrders();
        }
    }

    #endregion

    public RelayCommand UpdateRequestCommand { get; set; }
    public RelayCommand StartProgramUpdateCommand { get; set; }
    public RelayCommand SwitchLanguageCommand { get; set; }
    public RelayCommand OpenUpAdminWindowCommand { get; set; }

    public MainViewModel()
    {
        Instance = this;
        AccessLevel = LoginViewModel.Instance.AccessLevel;
        DeviceId = LoginViewModel.Instance.DeviceId;
        ServerAddress = LoginViewModel.Instance.ServerAddress;
        Lang = LoginViewModel.Instance.Lang;

        AddToDebug("#0: App started");

        Language = (string)Lang["language"];

        LastDBUpdateLocalTime = (string)Lang["fetchingData"];

        #region UnitDoubles fillup
        UnitDoubles.Add("TotalAbutmentsLeftSide", TotalAbutmentsLeftSideFinal);
        UnitDoubles.Add("TotalCrownsLeftSide", TotalCrownsLeftSideFinal);
        UnitDoubles.Add("TotalOrdersLeftSide", TotalOrdersLeftSideFinal);
        UnitDoubles.Add("TotalOrdersTodayLeftSide", TotalOrdersTodayLeftSideFinal);
        UnitDoubles.Add("TotalUnitsLeftSide", TotalUnitsLeftSideFinal);
        UnitDoubles.Add("TotalUnitsTodayLeftSide", TotalUnitsTodayLeftSideFinal);
        UnitDoubles.Add("TotalOrdersLeftOversLeftSide", TotalOrdersLeftOversLeftSideFinal);
        UnitDoubles.Add("TotalUnitsLeftOverLeftSide", TotalUnitsLeftOverLeftSideFinal);

        UnitDoubles.Add("TotalAbutmentsRightSide", TotalAbutmentsRightSideFinal);
        UnitDoubles.Add("TotalCrownsRightSide", TotalCrownsRightSideFinal);
        UnitDoubles.Add("TotalOrdersRightSide", TotalOrdersRightSideFinal);
        UnitDoubles.Add("TotalOrdersTodayRightSide", TotalOrdersTodayRightSideFinal);
        UnitDoubles.Add("TotalUnitsRightSide", TotalUnitsRightSideFinal);
        UnitDoubles.Add("TotalUnitsTodayRightSide", TotalUnitsTodayRightSideFinal);
        UnitDoubles.Add("TotalOrdersLeftOversRightSide", TotalOrdersLeftOversRightSideFinal);
        UnitDoubles.Add("TotalUnitsLeftOverRightSide", TotalUnitsLeftOverRightSideFinal);
        #endregion UnitDoubles fillup

        var assembly = Assembly.GetExecutingAssembly();
        string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("version.txt"));
        string versionResult = "";
        using (Stream stream = assembly.GetManifestResourceStream(resourceName)!)
        using (StreamReader reader = new(stream))
        {
            versionResult = reader.ReadToEnd();
            if (double.TryParse(versionResult[..versionResult.IndexOf('-')].Trim(), out var dbl))
                AppVersionDouble = dbl;
        }

        AppVersion = $"Made by AmL - v{versionResult}";

        if (File.Exists($"{LocalConfigFolderHelper}settings.cf"))
        {
            if (int.TryParse(File.ReadAllText($"{LocalConfigFolderHelper}settings.cf"), out int OrderByIdx))
            OrderByIndex = OrderByIdx;
        }

        ColumnAccess = AccessLevel switch
        {
            "Left" => 1,
            "Right" => 2,
            "Both" => 3,
            _ => 0,
        };

        UpdateRequestCommand = new RelayCommand(o => UpdateRequest());
        StartProgramUpdateCommand = new RelayCommand(o => StartProgramUpdate());
        SwitchLanguageCommand = new RelayCommand(o => SwitchLanguage());
        OpenUpAdminWindowCommand = new RelayCommand(o => OpenUpAdminWindow());
        
        _countdownClock = new System.Timers.Timer(1000);
        _countdownClock.Elapsed += CountDownClock_Elapsed;
        _countdownClock.Start();
        
        _timer = new System.Timers.Timer(10000);
        _timer.Elapsed += Timer_Elapsed;
        _timer.Start();
        
        _orderTimer = new System.Timers.Timer(60000);
        _orderTimer.Elapsed += OrderTimer_Elapsed;
        _orderTimer.Start();

        _ = GetServerInfo();

        ResetCountDownCounter();
        _ = GetTheOrderInfos();
    }

    private static void OpenUpAdminWindow()
    {
        PassCodeWindow passcodeWindow = new()
        {
            Owner = MainWindow.Instance
        };
        passcodeWindow.ShowDialog();
    }

    private void SwitchLanguage()
    {
        if (Language == "English")
        {
            MainWindow.Instance.SetLanguageDictionary("Chinese");
            SetLanguageDictionary("Chinese");
            Language = "Chinese";
        }
        else
        {
            MainWindow.Instance.SetLanguageDictionary("English");
            SetLanguageDictionary("English");
            Language = "English";
        }
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
            catch (IOException ex)
            {
                AddToDebug("#1: " + ex.Message);
                Lang.Source = new Uri("..\\..\\Lang\\StringResources_English.xaml", UriKind.Relative);
            }
        }

        MainWindow.Instance.Resources.MergedDictionaries.Add(lang);
    }

    private async void UpdateRequest()
    {
        if (!ServerInfoModel.ServerIsWritingDatabase && ServerIsOnline)
        {
            _timer.Interval = 2000;
            _ = RequestServerUpdate();
        }
    }

    public void StartProgramUpdate()
    {
        var Processes = Process.GetProcesses()
                           .Where(pr => pr.ProcessName == "CaseCheckerUpdater");
        foreach (var process in Processes)
        {
            process.Kill();
        }

        Task.Run(DownloadUpdater).Wait();

        StartUpdaterApp();
    }

    private async void DownloadUpdater()
    {
        Thread.Sleep(500);

        string appPath = Path.GetDirectoryName(AppContext.BaseDirectory);
        try
        {
            if (File.Exists($@"{appPath}\CaseCheckerUpdater.exe"))
                File.Delete($@"{appPath}\CaseCheckerUpdater.exe");

            Thread.Sleep(500);
            if (!File.Exists($@"{appPath}\CaseCheckerUpdater.exe"))
            {
                using var client = new HttpClient();
                using var s = await client.GetStreamAsync("https://raw.githubusercontent.com/aml-one/CaseChecker/master/CaseChecker/Executable/CaseCheckerUpdater.exe");
                using var fs = new FileStream($@"{appPath}\CaseCheckerUpdater.exe", FileMode.OpenOrCreate);
                await s.CopyToAsync(fs);
            }
        }
        catch (Exception)
        {
            try
            {
                if (File.Exists($@"{appPath}\CaseCheckerUpdater.exe"))
                    File.Delete($@"{appPath}\CaseCheckerUpdater.exe");

                if (!File.Exists($@"{appPath}\CaseCheckerUpdater.exe"))
                {
                    using var client = new HttpClient();
                    using var s = await client.GetStreamAsync("https://aml.one/CaseChecker/CaseCheckerUpdater.exe");
                    using var fs = new FileStream($@"{appPath}\CaseCheckerUpdater.exe", FileMode.OpenOrCreate);
                    await s.CopyToAsync(fs);
                }
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    AddToDebug("#2e: " + ex.Message);
                });
            }
        }

        Thread.Sleep(3000);
    }

    private void StartUpdaterApp()
    {
        Thread.Sleep(3000);
        try
        {
            string appPath = Path.GetDirectoryName(AppContext.BaseDirectory);

            var p = new Process();

            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = $"/c \"{appPath}\\CaseCheckerUpdater.exe\"";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            Thread.Sleep(2000);
        }
        catch (Exception ex)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                AddToDebug("#3e: " + ex.Message);
                MessageBox.Show(MainWindow.Instance, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            );
        }
    }

    private async void CountDownClock_Elapsed(object? sender, ElapsedEventArgs e)
    {
        if (DateTime.Now.Second % 10 == 1)
            ServerIsOnline = await CheckIfServerIsAlive();

        
        if (!ServerIsOnline)
        {
            StatsDBSettingsModel infoModl = new()
            {
                StatsServerStatus = (string)Lang["offline"],
                ServerIsWritingDatabase = true
            };
            ServerInfoModel = infoModl;
            ProgressBarColor = "LightSalmon";
            StatusColor = "LightSalmon";
            ExportClockCountDown = "-";
            LastDBUpdateLocalTime = (string)Lang["waitingForServer"]; 
            return;
        }
        else
        {
            ProgressBarColor = "#a1fa93";
            StatusColor = "LightGreen";
        }
        

        if (ServerInfoModel is null)
            return;

        if (Counter > 0)
        {
            Counter--;
            int minutes = Counter / 60;
            int seconds = Counter % 60;

            if (seconds < 10)
                ExportClockCountDown = minutes.ToString() + ":0" + seconds.ToString();
            else
                ExportClockCountDown = minutes.ToString() + ":" + seconds.ToString();

            if (UpdateTimeOpacity > 0.5)
            {
                UpdateTimeColor = "LightGreen";
                UpdateTimeOpacity -= 0.02;
            }
            else
                UpdateTimeColor = "#DDD";
        }
        else
            ExportClockCountDown = (string)Lang["due"]; ;


        if (Counter < 5)
        {
            if (_timer.Interval != 2000)
                _timer.Interval = 2000;
        }
        else
        {
            if (_timer.Interval != 10000)
                _timer.Interval = 10000;
        }

        if (CanResetCounter)
        {
            if (!ServerInfoModel.ServerIsWritingDatabase)
            {
                CanResetCounter = false;
                ResetCountDownCounter();
                if (ServerIsOnline)
                    _ = GetTheOrderInfos();
            }
        }
    }

    private void OrderTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        if (!ServerInfoModel.ServerIsWritingDatabase & ServerIsOnline)
        {
            _ = GetTheOrderInfos();
        }
    }

    private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        if (LastDBUpdate != ServerInfoModel.LastDBUpdate && !ServerInfoModel.ServerIsWritingDatabase && ServerIsOnline)
        {
            LastDBUpdateLocalTime = DateTime.Now.ToString("MMM d - h:mm:ss tt");
            _ = GetTheOrderInfos();
            UpdateTimeColor = "LightGreen";
            UpdateTimeOpacity = 1;
            CanResetCounter = true;
        }

        if (ServerIsOnline)
            _ = GetServerInfo();
        LastDBUpdate = ServerInfoModel.LastDBUpdate!;
    }

    private async Task GetTheOrderInfos(string side = "both")
    {
        var handler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        };

        var http = new HttpClient(handler);
        http.DefaultRequestHeaders.Add("DeviceId", DeviceId);

        List<SentOutCasesModel> modelList = [];
        List<SentOutCasesModel> sortedModelList = [];

        try
        {
            string result = await http.GetStringAsync($"https://{ServerAddress}:10113/api/statssentoutcases/{side}");
            modelList = JsonConvert.DeserializeObject<List<SentOutCasesModel>>(result)!;

            Task.Run(() =>
            {
                TotalAbutmentsLeftSide = 0;
                TotalCrownsLeftSide = 0;
                TotalOrdersLeftSide = 0;
                TotalOrdersTodayLeftSide = 0;
                TotalUnitsLeftSide = 0;
                TotalUnitsTodayLeftSide = 0;
                TotalOrdersLeftOversLeftSide = 0;
                TotalUnitsLeftOverLeftSide = 0;

                TotalAbutmentsRightSide = 0;
                TotalCrownsRightSide = 0;
                TotalOrdersRightSide = 0;
                TotalOrdersTodayRightSide = 0;
                TotalUnitsRightSide = 0;
                TotalUnitsTodayRightSide = 0;
                TotalOrdersLeftOversRightSide = 0;
                TotalUnitsLeftOverRightSide = 0;

                foreach (var model in modelList)
                {
                    model.RedoCaseComment = (string)Lang["redoCaseComment"];
                    model.RushCaseComment = (string)Lang["rushCaseComment"];
                    model.RushForMorningComment = (string)Lang["rushForMorningComment"];
                    model.OrderDesignedComment = (string)Lang["orderDesignedComment"];

                    if (model.TotalUnits!.Length == 1)
                        model.TotalUnitsWithPrefixZero = "0" + model.TotalUnits;
                    else
                        model.TotalUnitsWithPrefixZero = model.TotalUnits;

                    if (model.Crowns == "0")
                        model.Crowns = "";
                    if (model.Abutments == "0")
                        model.Abutments = "";
                    if (model.Models == "0")
                        model.Models = "";
                    else
                        model.Models = "🗸";

                    if (model.SentOn == DateTime.Now.ToString("MM-dd-yyyy"))
                        model.SentOn = $"z{(string)Lang["today"]}";
                    if (model.SentOn == DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy"))
                        model.SentOn = $"9{(string)Lang["yesterday"]}";

                    if (model.SentOn != $"z{(string)Lang["today"]}" || model.SentOn != $"9{(string)Lang["yesterday"]}")
                    {
                        if (DateTime.TryParse(model.SentOn, out DateTime sentOn))
                        {
                            string dayName = sentOn.ToString("dddd");

                            dayName = dayName switch
                            {
                                "星期一" => $"2{(string)Lang["monday"]}",
                                "星期二" => $"3{(string)Lang["tuesday"]}",
                                "星期三" => $"4{(string)Lang["wednesday"]}",
                                "星期四" => $"5{(string)Lang["thursday"]}",
                                "星期五" => $"6{(string)Lang["friday"]}",
                                "星期六" => $"7{(string)Lang["saturday"]}",
                                "星期日" => $"8{(string)Lang["sunday"]}",
                                "Monday" => $"2{(string)Lang["monday"]}",
                                "Tuesday" => $"3{(string)Lang["tuesday"]}",
                                "Wednesday" => $"4{(string)Lang["wednesday"]}",
                                "Thursday" => $"5{(string)Lang["thursday"]}",
                                "Friday" => $"6{(string)Lang["friday"]}",
                                "Saturday" => $"7{(string)Lang["saturday"]}",
                                "Sunday" => $"8{(string)Lang["sunday"]}",
                                _ => dayName,
                            };
                            model.SentOn = dayName;
                        }
                    }

                    model.IconImage = GetIcon(model.ScanSource!, model.CommentIcon!);

                    if (Language == "Chinese")
                        model.Items = Translate(model.Items!);

                    model.Items = model.Items!.Replace($"{(string)Lang["unsectionedModel"]}, {(string)Lang["antagonistModel"]}", (string)Lang["model"])
                                              .Replace((string)Lang["unsectionedModel"], (string)Lang["model"])
                                              .Replace((string)Lang["antagonistModel"], (string)Lang["model"]);

                    model.CommentIn3Shape = model.CommentIn3Shape!.Trim()
                                                                 .Replace("!", "")
                                                                 .Replace("Thanks","")
                                                                 .Replace("Thank you","")
                                                                 .Replace("Thank You","")
                                                                 .Replace("[Converted From FDI]", "")
                                                                 .Trim();

                    model.CommentIn3Shape = LineBreaksRegEx().Replace(model.CommentIn3Shape, string.Empty);

                    if (model.CommentIn3Shape!.Contains(" redo", StringComparison.CurrentCultureIgnoreCase) ||
                        model.CommentIn3Shape!.Contains("redo ", StringComparison.CurrentCultureIgnoreCase) ||
                        model.CommentIn3Shape!.Equals("redo", StringComparison.CurrentCultureIgnoreCase) ||
                        model.CommentIn3Shape!.Contains("return to lab", StringComparison.CurrentCultureIgnoreCase) ||
                        model.CommentIn3Shape!.Contains("returned to lab", StringComparison.CurrentCultureIgnoreCase) ||
                        model.CommentIn3Shape!.Contains("open margin", StringComparison.CurrentCultureIgnoreCase))
                    {
                        model.CommentColor = "Maroon";
                        model.Redo = "1";
                    }

                    // clearing comment, if it's a standard iTero comment
                    if (model.CommentIn3Shape.Contains("Exported from iTero system"))
                        model.CommentIn3Shape = "";
                    else if (!string.IsNullOrEmpty(model.CommentIn3Shape))
                        model.CommentIn3Shape = char.ToUpper(model.CommentIn3Shape[0]) + model.CommentIn3Shape.Substring(1);



                    if (model.CommentIcon == "7")
                    {
                        model.Crowns = "";
                        model.Abutments = "";
                        model.Models = "";
                        model.TotalUnits = "0";
                        model.TotalUnitsWithPrefixZero = "00";
                        model.SentOn = $"0{(string)Lang["designReady"]}";
                    }

                    if (model.Comment is not null)
                    {
                        if (model.Comment.StartsWith("This case is NOT in"))
                        {
                            model.Crowns = "";
                            model.Abutments = "";
                            model.Models = "";
                            model.TotalUnits = "0";
                            model.TotalUnitsWithPrefixZero = "00";
                            model.CommentColor = "Gray";
                            model.SentOn = (string)Lang["noScanFile"];
                        }    
                    }

                    if (model.CommentIcon == "8")
                    {
                        model.CommentColor = "Blue";
                        model.SentOn = $"1{(string)Lang["change"]}";
                    }

                    #region Counting units

                    if (!string.IsNullOrEmpty(model.Crowns)) 
                    {
                        _ = int.TryParse(model.Crowns, out int crowns);
                        if (model.Side!.Equals("left", StringComparison.CurrentCultureIgnoreCase))
                        {
                            TotalCrownsLeftSide += crowns;
                            TotalUnitsLeftSide += crowns;

                            if (model.SentOn!.Equals($"z{(string)Lang["today"]}"))
                                TotalUnitsTodayLeftSide += crowns;
                            if (model.SentOn!.Equals($"1{(string)Lang["change"]}"))
                                TotalUnitsTodayLeftSide += crowns;
                            if (model.SentOn!.Equals($"9{(string)Lang["yesterday"]}") && DateTime.Now.Hour < 5)
                                TotalUnitsTodayLeftSide += crowns;
                        }
                        
                        if (model.Side!.Equals("right", StringComparison.CurrentCultureIgnoreCase))
                        {
                            TotalCrownsRightSide += crowns;
                            TotalUnitsRightSide += crowns;

                            if (model.SentOn!.Equals($"z{(string)Lang["today"]}"))
                                TotalUnitsTodayRightSide += crowns;
                            if (model.SentOn!.Equals($"1{(string)Lang["change"]}"))
                                TotalUnitsTodayRightSide += crowns;
                            if (model.SentOn!.Equals($"9{(string)Lang["yesterday"]}") && DateTime.Now.Hour < 5)
                                TotalUnitsTodayRightSide += crowns;
                        }
                    }

                    if (!string.IsNullOrEmpty(model.Abutments))
                    {
                        _ = int.TryParse(model.Abutments, out int abutments);
                        if (model.Side!.Equals("left", StringComparison.CurrentCultureIgnoreCase))
                        {
                            TotalAbutmentsLeftSide += abutments;
                            TotalUnitsLeftSide += abutments;

                            if (model.SentOn!.Equals($"z{(string)Lang["today"]}"))
                                TotalUnitsTodayLeftSide += abutments;
                            if (model.SentOn!.Equals($"1{(string)Lang["change"]}"))
                                TotalUnitsTodayLeftSide += abutments;
                            if (model.SentOn!.Equals($"9{(string)Lang["yesterday"]}") && DateTime.Now.Hour < 5)
                                TotalUnitsTodayLeftSide += abutments;
                        }
                        
                        if (model.Side!.Equals("right", StringComparison.CurrentCultureIgnoreCase))
                        {
                            TotalAbutmentsRightSide += abutments;
                            TotalUnitsRightSide += abutments;

                            if (model.SentOn!.Equals($"z{(string)Lang["today"]}"))
                                TotalUnitsTodayRightSide += abutments;
                            if (model.SentOn!.Equals($"1{(string)Lang["change"]}"))
                                TotalUnitsTodayRightSide += abutments;
                            if (model.SentOn!.Equals($"9{(string)Lang["yesterday"]}") && DateTime.Now.Hour < 5)
                                TotalUnitsTodayRightSide += abutments;
                        }
                    }

                    if (model.Side!.Equals("left", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (model.SentOn!.Equals($"z{(string)Lang["today"]}"))
                            TotalOrdersTodayLeftSide++;
                        if (model.SentOn!.Equals($"1{(string)Lang["change"]}"))
                            TotalOrdersTodayLeftSide++;
                        if (model.SentOn!.Equals($"9{(string)Lang["yesterday"]}") && DateTime.Now.Hour < 5)
                            TotalOrdersTodayLeftSide++;

                        TotalOrdersLeftSide++;
                    }
                    
                    if (model.Side!.Equals("right", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (model.SentOn!.Equals($"z{(string)Lang["today"]}"))
                            TotalOrdersTodayRightSide++;
                        if (model.SentOn!.Equals($"1{(string)Lang["change"]}"))
                            TotalOrdersTodayRightSide++;
                        if (model.SentOn!.Equals($"9{(string)Lang["yesterday"]}") && DateTime.Now.Hour < 5)
                            TotalOrdersTodayRightSide++;

                        TotalOrdersRightSide++;
                    }

                    if (TotalOrdersLeftSide == TotalOrdersTodayLeftSide || TotalOrdersTodayLeftSide == 0)
                        TotalOrdersTodaySameAsAllTimeTotalLeftSide = Visibility.Hidden;
                    else
                        TotalOrdersTodaySameAsAllTimeTotalLeftSide = Visibility.Visible;
                    
                    if (TotalUnitsLeftSide == TotalUnitsTodayLeftSide || TotalUnitsTodayLeftSide == 0)
                        TotalUnitsTodaySameAsAllTimeTotalLeftSide = Visibility.Hidden;
                    else
                        TotalUnitsTodaySameAsAllTimeTotalLeftSide = Visibility.Visible;
                    
                    if (TotalOrdersRightSide == TotalOrdersTodayRightSide || TotalOrdersTodayRightSide == 0)
                        TotalOrdersTodaySameAsAllTimeTotalRightSide = Visibility.Hidden;
                    else
                        TotalOrdersTodaySameAsAllTimeTotalRightSide = Visibility.Visible;
                    
                    if (TotalUnitsRightSide == TotalUnitsTodayRightSide || TotalUnitsTodayRightSide == 0)
                        TotalUnitsTodaySameAsAllTimeTotalRightSide = Visibility.Hidden;
                    else
                        TotalUnitsTodaySameAsAllTimeTotalRightSide = Visibility.Visible;

                    #endregion Counting units

                    if (model.Rush == "1")
                    {
                        model.CommentColor = "Crimson";
                        model.SentOn = $"0{(string)Lang["rush"]}";
                    }

                    

                    if (Language == "Chinese")
                        model.Comment = TranslateComment(model.Comment!);
                }

            }).Wait();

            TotalOrdersLeftOversLeftSide = TotalOrdersLeftSide - TotalOrdersTodayLeftSide;
            TotalOrdersLeftOversRightSide = TotalOrdersRightSide - TotalOrdersTodayRightSide;

            TotalUnitsLeftOverLeftSide = TotalUnitsLeftSide - TotalUnitsTodayLeftSide;
            TotalUnitsLeftOverRightSide = TotalUnitsRightSide - TotalUnitsTodayRightSide;

            _ = Task.Run(StartPresentingUnitNumbers);

            if (OrderByIndex == 0)
            {
                sortedModelList = [.. modelList.OrderBy(x => x.SentOn).ThenByDescending(x => x.Rush).ThenBy(x => x.CommentIcon).ThenByDescending(x => x.TotalUnitsWithPrefixZero)];
            }
            else if (OrderByIndex == 1)
            {
                sortedModelList = [.. modelList.OrderBy(x => x.SentOn).ThenByDescending(x => x.Rush).ThenBy(x => x.CommentIcon).ThenBy(x => x.OrderID)];
            }



            if (side.Equals("left", StringComparison.CurrentCultureIgnoreCase))
            {
                SentOutCasesModelLeftSide = sortedModelList;
            }
            
            if (side.Equals("right", StringComparison.CurrentCultureIgnoreCase))
                SentOutCasesModelRightSide = sortedModelList;

            if (side.Equals("both", StringComparison.CurrentCultureIgnoreCase))
            {
                SentOutCasesModelLeftSide = sortedModelList.Where(x => x.Side == "Left").ToList();
                SentOutCasesModelRightSide = sortedModelList.Where(x => x.Side == "Right").ToList();
            }
        }
        catch (Exception ex)
        {
            AddToDebug("#4: " + ex.Message);
        }
        http.Dispose();
        handler.Dispose();
    }

    
    private async void StartPresentingUnitNumbers()
    {
        if (TotalUnitsLeftSideFinal != TotalUnitsLeftSide && !AccessLevel.Equals("Right"))
            await CountUp_TotalUnitsLeftSide(TotalUnitsLeftSide);
        if (TotalCrownsLeftSideFinal != TotalCrownsLeftSide && !AccessLevel.Equals("Right"))
            await CountUp_TotalCrownsLeftSide(TotalCrownsLeftSide);
        if (TotalAbutmentsLeftSideFinal != TotalAbutmentsLeftSide && !AccessLevel.Equals("Right"))
            await CountUp_TotalAbutmentsLeftSide(TotalAbutmentsLeftSide);
        if (TotalOrdersLeftSideFinal != TotalOrdersLeftSide && !AccessLevel.Equals("Right"))
            await CountUp_TotalOrdersLeftSide(TotalOrdersLeftSide);
        if (TotalUnitsLeftOverLeftSideFinal != TotalUnitsLeftOverLeftSide && !AccessLevel.Equals("Right"))
            await CountUp_TotalUnitsLeftOverLeftSide(TotalUnitsLeftOverLeftSide);
        if (TotalOrdersLeftOversLeftSideFinal != TotalOrdersLeftOversLeftSide && !AccessLevel.Equals("Right"))
            await CountUp_TotalOrdersLeftOversLeftSide(TotalOrdersLeftOversLeftSide);
        if (TotalUnitsTodayLeftSideFinal != TotalUnitsTodayLeftSide && !AccessLevel.Equals("Right"))
            await CountUp_TotalUnitsTodayLeftSide(TotalUnitsTodayLeftSide);
        if (TotalOrdersTodayLeftSideFinal != TotalOrdersTodayLeftSide && !AccessLevel.Equals("Right"))
            await CountUp_TotalOrdersTodayLeftSide(TotalOrdersTodayLeftSide);

        if (TotalUnitsRightSideFinal != TotalUnitsRightSide && !AccessLevel.Equals("Left"))
            await CountUp_TotalUnitsRightSide(TotalUnitsRightSide);
        if (TotalCrownsRightSideFinal != TotalCrownsRightSide && !AccessLevel.Equals("Left"))
            await CountUp_TotalCrownsRightSide(TotalCrownsRightSide);
        if (TotalAbutmentsRightSideFinal != TotalAbutmentsRightSide && !AccessLevel.Equals("Left"))
            await CountUp_TotalAbutmentsRightSide(TotalAbutmentsRightSide);
        if (TotalOrdersRightSideFinal != TotalOrdersRightSide && !AccessLevel.Equals("Left"))
            await CountUp_TotalOrdersRightSide(TotalOrdersRightSide);
        if (TotalUnitsLeftOverRightSideFinal != TotalUnitsLeftOverRightSide && !AccessLevel.Equals("Left"))
            await CountUp_TotalUnitsLeftOverRightSide(TotalUnitsLeftOverRightSide);
        if (TotalOrdersLeftOversRightSideFinal != TotalOrdersLeftOversRightSide && !AccessLevel.Equals("Left"))
            await CountUp_TotalOrdersLeftOversRightSide(TotalOrdersLeftOversRightSide);
        if (TotalUnitsTodayRightSideFinal != TotalUnitsTodayRightSide && !AccessLevel.Equals("Left"))
            await CountUp_TotalUnitsTodayRightSide(TotalUnitsTodayRightSide);
        if (TotalOrdersTodayRightSideFinal != TotalOrdersTodayRightSide && !AccessLevel.Equals("Left"))
            await CountUp_TotalOrdersTodayRightSide(TotalOrdersTodayRightSide);
    }

    #region CoutUp functions
    private async Task CountUp_TotalUnitsLeftSide(double Max)
    {
        for(int i = 0; i <= Max; i++)
        {
            TotalUnitsLeftSideFinal = i;
            Thread.Sleep(10);
        }
    }
    
    private async Task CountUp_TotalCrownsLeftSide(double Max)
    {
        for(int i = 0; i <= Max; i++)
        {
            TotalCrownsLeftSideFinal = i;
            Thread.Sleep(10);
        }
    }
    
    private async Task CountUp_TotalAbutmentsLeftSide(double Max)
    {
        for(int i = 0; i <= Max; i++)
        {
            TotalAbutmentsLeftSideFinal = i;
            Thread.Sleep(10);
        }
    }
    
    private async Task CountUp_TotalOrdersLeftSide(double Max)
    {
        for(int i = 0; i <= Max; i++)
        {
            TotalOrdersLeftSideFinal = i;
            Thread.Sleep(10);
        }
    }
    
    private async Task CountUp_TotalUnitsLeftOverLeftSide(double Max)
    {
        for(int i = 0; i <= Max; i++)
        {
            TotalUnitsLeftOverLeftSideFinal = i;
            Thread.Sleep(10);
        }
    }
    
    private async Task CountUp_TotalOrdersLeftOversLeftSide(double Max)
    {
        for(int i = 0; i <= Max; i++)
        {
            TotalOrdersLeftOversLeftSideFinal = i;
            Thread.Sleep(10);
        }
    }
    
    private async Task CountUp_TotalUnitsTodayLeftSide(double Max)
    {
        for(int i = 0; i <= Max; i++)
        {
            TotalUnitsTodayLeftSideFinal = i;
            Thread.Sleep(10);
        }
    }
    
    private async Task CountUp_TotalOrdersTodayLeftSide(double Max)
    {
        for(int i = 0; i <= Max; i++)
        {
            TotalOrdersTodayLeftSideFinal = i;
            Thread.Sleep(10);
        }
    }


    private async Task CountUp_TotalUnitsRightSide(double Max)
    {
        for (int i = 0; i <= Max; i++)
        {
            TotalUnitsRightSideFinal = i;
            Thread.Sleep(10);
        }
    }

    private async Task CountUp_TotalCrownsRightSide(double Max)
    {
        for (int i = 0; i <= Max; i++)
        {
            TotalCrownsRightSideFinal = i;
            Thread.Sleep(10);
        }
    }

    private async Task CountUp_TotalAbutmentsRightSide(double Max)
    {
        for (int i = 0; i <= Max; i++)
        {
            TotalAbutmentsRightSideFinal = i;
            Thread.Sleep(10);
        }
    }

    private async Task CountUp_TotalOrdersRightSide(double Max)
    {
        for (int i = 0; i <= Max; i++)
        {
            TotalOrdersRightSideFinal = i;
            Thread.Sleep(10);
        }
    }

    private async Task CountUp_TotalUnitsLeftOverRightSide(double Max)
    {
        for (int i = 0; i <= Max; i++)
        {
            TotalUnitsLeftOverRightSideFinal = i;
            Thread.Sleep(10);
        }
    }

    private async Task CountUp_TotalOrdersLeftOversRightSide(double Max)
    {
        for (int i = 0; i <= Max; i++)
        {
            TotalOrdersLeftOversRightSideFinal = i;
            Thread.Sleep(10);
        }
    }

    private async Task CountUp_TotalUnitsTodayRightSide(double Max)
    {
        for (int i = 0; i <= Max; i++)
        {
            TotalUnitsTodayRightSideFinal = i;
            Thread.Sleep(10);
        }
    }

    private async Task CountUp_TotalOrdersTodayRightSide(double Max)
    {
        for (int i = 0; i <= Max; i++)
        {
            TotalOrdersTodayRightSideFinal = i;
            Thread.Sleep(10);
        }
    }

    #endregion CoutUp functions

    private static string TranslateComment(string text)
    {
        text = text.Replace("Someone moved this case manually here.", "有人手动将此案例移至此处");
        text = text.Replace("This case is NOT in the export folder!!!", "此案例不在导出文件夹中！ 忽略这个案例");
        text = text.Replace("This case were sent to both designer!", "这个案子发给了两位设计师");
        text = text.Replace("The design needs to change!", "设计需要改变！");        
        return text;
    }


    private static string Translate(string text)
    {
        text = text.Replace("Unsectioned model", "未分割模型");
        text = text.Replace("Antagonist model", "对合模型");
        text = text.Replace("Sectioned (die ditched) model", "分割模型");
        text = text.Replace("Die", "代型");

        text = text.Replace("Temporary on prepared model", "已制备模型上的临时冠");
        text = text.Replace("Anatomy bridge with gingiva", "解剖牙桥 含牙龈");
        text = text.Replace("Crown with gingiva", "牙冠 含牙龈");
        text = text.Replace("Anatomical coping", "解剖型内冠");


        text = text.Replace("Anatomy bridge", "解剖牙桥");
        text = text.Replace("Frame bridge", "框架桥");

        text = text.Replace("Temporary Crown", "临时冠");

        text = text.Replace("Anatomical Abutment", "解剖型基台");
        text = text.Replace("Post and Core", "桩核");
        text = text.Replace("Inlay", "嵌体");
        text = text.Replace("Onlay", "高嵌体");
        text = text.Replace("Abutment", "基台");
        text = text.Replace("Screw Retained Crown", "螺丝固位冠");
        text = text.Replace("Veneer", "贴面");

        text = text.Replace("Coping", "内冠");
        text = text.Replace("Crown", "牙冠");

        return text;
    }

    private void SortOrders()
    {
        if (SentOutCasesModelLeftSide is not null || SentOutCasesModelRightSide is not null)
        {
            List<SentOutCasesModel>? modelListLeftSide = SentOutCasesModelLeftSide;
            List<SentOutCasesModel>? modelListRightSide = SentOutCasesModelRightSide;
            List<SentOutCasesModel>? sortedModelListLeftSide = [];
            List<SentOutCasesModel>? sortedModelListRightSide = [];

            if (OrderByIndex == 0)
            {
                if (modelListLeftSide is not null)
                {
                    sortedModelListLeftSide = [.. modelListLeftSide.OrderBy(x => x.SentOn).ThenByDescending(x => x.Rush).ThenBy(x => x.CommentIcon).ThenByDescending(x => x.TotalUnitsWithPrefixZero)];
                    SentOutCasesModelLeftSide = sortedModelListLeftSide;
                }

                if (modelListRightSide is not null)
                {
                    sortedModelListRightSide = [.. modelListRightSide.OrderBy(x => x.SentOn).ThenByDescending(x => x.Rush).ThenBy(x => x.CommentIcon).ThenByDescending(x => x.TotalUnitsWithPrefixZero)];
                    SentOutCasesModelRightSide = sortedModelListRightSide;
                }
            }
            else if (OrderByIndex == 1)
            {
                if (modelListLeftSide is not null)
                {
                    sortedModelListLeftSide = [.. modelListLeftSide.OrderBy(x => x.SentOn).ThenByDescending(x => x.Rush).ThenBy(x => x.CommentIcon).ThenByDescending(x => x.OrderID)];
                    SentOutCasesModelLeftSide = sortedModelListLeftSide;
                }

                if (modelListRightSide is not null)
                {
                    sortedModelListRightSide = [.. modelListRightSide.OrderBy(x => x.SentOn).ThenByDescending(x => x.Rush).ThenBy(x => x.CommentIcon).ThenByDescending(x => x.OrderID)];
                    SentOutCasesModelRightSide = sortedModelListRightSide;
                }
            }

        }

        File.WriteAllText($"{LocalConfigFolderHelper}settings.cf", OrderByIndex.ToString());
    }

    private static string GetIcon(string ScanSource, string commentIcon)
    {
        if (commentIcon == "7") return "/Images/crown.png";

        return ScanSource switch
        {
            "ss3ShapeDesktopScanner" => "/Images/i10.png",
            "ss3SE4" => "/Images/i33.png",
            "ss3SE3" => "/Images/i33.png",
            "ss3SE2" => "/Images/i33.png",
            "ss3SD2000" => "/Images/i20.png",
            "ss3SD1000" => "/Images/i20.png",
            "ss3SD900" => "/Images/i20.png",
            "ss3SD810" => "/Images/i20.png",
            "ss3SD800" => "/Images/i20.png",
            "ss3SD700" => "/Images/i20.png",
            _ => "/Images/trios_new.png",
        };
    }

    private async Task GetServerInfo()
    {
        var handler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        };

        var http = new HttpClient(handler);
        http.DefaultRequestHeaders.Add("DeviceId", DeviceId);

        try
        {
            string result = await http.GetStringAsync($"https://{ServerAddress}:10113/api/statsserverinfo");
            ServerInfoModel = JsonConvert.DeserializeObject<StatsDBSettingsModel>(result)!;

            AutoSend0 = ServerInfoModel.AutoSend0;
            AutoSend15 = ServerInfoModel.AutoSend15;
            AutoSend30 = ServerInfoModel.AutoSend30;
            AutoSend45 = ServerInfoModel.AutoSend45;

            if (!AccessLevel.Equals("both", StringComparison.CurrentCultureIgnoreCase))
            {
                string siteId = ServerInfoModel.SiteID;
                if (siteId is not null)
                {
                    ServerInfoModel.DesignerNameAnteriors = siteId[..siteId.IndexOf('-')];
                    ServerInfoModel.DesignerNamePosteriors = siteId[..siteId.IndexOf('-')];
                    SiteID = siteId[..siteId.IndexOf('-')];
                }
            }
            else
            {
                string siteId = ServerInfoModel.SiteID;
                if (siteId is not null)
                    SiteID = siteId[..siteId.IndexOf('-')];
            }

            ServerInfoModel.StatsServerStatus = ServerInfoModel.StatsServerStatus switch
            {
                "Idle" => (string)Lang["srvIdle"],
                "Exporting cases" => (string)Lang["srvExporting"],
                "Collecting cases" => (string)Lang["srvCollecting"],
                "Cleaning database" => (string)Lang["srvCleaning"],
                "Calculating daily statistics" => (string)Lang["srvCalculating"],
                _ => (string)Lang["srvIdle"],
            };
        }
        catch (Exception ex)
        {
            AddToDebug("#5: " + ex.Message);
        }
        http.Dispose();
        handler.Dispose();
    }

    private async Task RequestServerUpdate()
    {
        var handler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        };

        var http = new HttpClient(handler);
        http.DefaultRequestHeaders.Add("DeviceId", DeviceId);

        try
        {
            string result = await http.GetStringAsync($"https://{ServerAddress}:10113/api/statsserverinfo/update");
        }
        catch (Exception ex)
        {
            AddToDebug("#6: " + ex.Message);
        }
        http.Dispose();
        handler.Dispose();
    }

    private void ResetCountDownCounter()
    {
        if (!AutoSend0 && !AutoSend15 && !AutoSend30 && !AutoSend45)
        {
            AutoSend0 = true;
            AutoSend15 = true;
            AutoSend30 = true;
            AutoSend45 = true;
        }

        List<int> Times = new List<int>();
        int TimeNowMinute, TimeNowSecond;

        TimeNowMinute = DateTime.Now.Minute;
        TimeNowSecond = DateTime.Now.Second;


        int SecondsRemain = 60 - TimeNowSecond;

        int TimeRemainTill0 = 60 - TimeNowMinute;
        int TimeRemainTill15 = (60 - TimeNowMinute) + 15;
        int TimeRemainTill30 = (60 - TimeNowMinute) + 30;
        int TimeRemainTill45 = (60 - TimeNowMinute) + 45;

        if (TimeRemainTill15 > 60) TimeRemainTill15 -= 60;
        if (TimeRemainTill30 > 60) TimeRemainTill30 -= 60;
        if (TimeRemainTill45 > 60) TimeRemainTill45 -= 60;

        int TotalRemainingSecondsTill0 = ((TimeRemainTill0 - 1) * 60) + SecondsRemain;
        int TotalRemainingSecondsTill15 = ((TimeRemainTill15 - 1) * 60) + SecondsRemain;
        int TotalRemainingSecondsTill30 = ((TimeRemainTill30 - 1) * 60) + SecondsRemain;
        int TotalRemainingSecondsTill45 = ((TimeRemainTill45 - 1) * 60) + SecondsRemain;


        if (AutoSend0)
            Times.Add(TotalRemainingSecondsTill0);

        if (AutoSend15)
            Times.Add(TotalRemainingSecondsTill15);

        if (AutoSend30)
            Times.Add(TotalRemainingSecondsTill30);

        if (AutoSend45)
            Times.Add(TotalRemainingSecondsTill45);

        Counter = Times.Min();

    }

    public void AddToDebug(string message)
    {
        DebugMessages.Add(message);
        RaisePropertyChanged(nameof(DebugMessages));
    }

    private async Task<bool> CheckIfServerIsAlive()
    {
        var handler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        };

        var http = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(20)
        };

        try
        {
            string result = await http.GetStringAsync($"https://{ServerAddress}:10113/api/checkifserveralive");

            if (result == "true")
                return true;
            else
                return false;

        }
        catch (Exception ex)
        {
            AddToDebug("#7: " + ex.Message);
        }
        http.Dispose();
        handler.Dispose();
        return false;
    }

    [GeneratedRegex(@"^\s+$[\r\n]*", RegexOptions.Multiline)]
    private static partial Regex LineBreaksRegEx();
}
