using CaseChecker.MVVM.Core;
using CaseChecker.MVVM.Model;
using CaseChecker.MVVM.View;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Reflection;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CaseChecker.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{
    public System.Timers.Timer _timer;
    public System.Timers.Timer _orderTimer;
    public System.Timers.Timer _countdownClock;
    private int Counter = 10;

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

    private string appVersion = "v1.2";
    public string AppVersion
    {
        get => appVersion;
        set
        {
            appVersion = value;
            RaisePropertyChanged(nameof(AppVersion));
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
    
    private List<SentOutCasesModel> sentOutCasesModelLeftSide;
    public List<SentOutCasesModel> SentOutCasesModelLeftSide
    {
        get => sentOutCasesModelLeftSide;
        set
        {
            sentOutCasesModelLeftSide = value;
            RaisePropertyChanged(nameof(SentOutCasesModelLeftSide));
        }
    }
    
    private List<SentOutCasesModel> sentOutCasesModelRightSide;
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
    
    private string progressBarColor = "Green";
    public string ProgressBarColor
    {
        get => progressBarColor;
        set
        {
            progressBarColor = value;
            RaisePropertyChanged(nameof(ProgressBarColor));
        }
    }
    
    private string updateTimeColor = "Crimson";
    public string UpdateTimeColor
    {
        get => updateTimeColor;
        set
        {
            updateTimeColor = value;
            RaisePropertyChanged(nameof(UpdateTimeColor));
        }
    }

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

#endregion

    public RelayCommand UpdateRequestCommand { get; set; }

    public MainViewModel()
    {
        Instance = this;
        AccessLevel = LoginViewModel.Instance.AccessLevel;
        DeviceId = LoginViewModel.Instance.DeviceId;
        ServerAddress = LoginViewModel.Instance.ServerAddress;

        AppVersion = "Made by AmL - v1.22 - 041424a";

        ColumnAccess = AccessLevel switch
        {
            "Left" => 1,
            "Right" => 2,
            "Both" => 3,
            _ => 0,
        };

        UpdateRequestCommand = new RelayCommand(o => UpdateRequest());

        _countdownClock = new System.Timers.Timer(1000);
        _countdownClock.Elapsed += CountDownCLock_Elapsed;
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

    private async void UpdateRequest()
    {
        if (!ServerInfoModel.ServerIsWritingDatabase && ServerIsOnline)
        {
            _timer.Interval = 2000;
            _ = RequestServerUpdate();
        }
    }

    private async void CountDownCLock_Elapsed(object? sender, ElapsedEventArgs e)
    {
        if (DateTime.Now.Second % 10 == 1)
            ServerIsOnline = await CheckIfServerIsAlive();

        
        if (!ServerIsOnline)
        {
            StatsDBSettingsModel infoModl = new StatsDBSettingsModel();
            infoModl.StatsServerStatus = "Offline";
            infoModl.ServerIsWritingDatabase = true;
            ServerInfoModel = infoModl;
            ProgressBarColor = "Crimson";
            ExportClockCountDown = "-";
            LastDBUpdateLocalTime = "Waiting for server to come online..";
            return;
        }
        else
        {
            ProgressBarColor = "Green";
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
                UpdateTimeColor = "Crimson";
                UpdateTimeOpacity -= 0.02;
            }
            else
                UpdateTimeColor = "Black";
        }
        else
            ExportClockCountDown = "Due";


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
            UpdateTimeColor = "Crimson";
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
                
                TotalAbutmentsRightSide = 0;
                TotalCrownsRightSide = 0;
                TotalOrdersRightSide = 0;
                TotalOrdersTodayRightSide = 0;
                TotalUnitsRightSide = 0;
                TotalUnitsTodayRightSide = 0;

                foreach (var model in modelList)
                {
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

                    if (model.SentOn == DateTime.Now.ToString("MM-dd-yyyy"))
                        model.SentOn = "zToday";
                    if (model.SentOn == DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy"))
                        model.SentOn = "9Yesterday";

                    if (model.SentOn != "zToday" || model.SentOn != "9Yesterday")
                    {
                        if (DateTime.TryParse(model.SentOn, out DateTime sentOn))
                        {
                            string dayName = sentOn.ToString("dddd");

                            dayName = dayName switch
                            {
                                "星期一" => "2Monday",
                                "星期二" => "3Tuesday",
                                "星期三" => "4Wednesday",
                                "星期四" => "5Thursday",
                                "星期五" => "6Friday",
                                "星期六" => "7Saturday",
                                "星期日" => "8Sunday",
                                "Monday" => "2Monday",
                                "Tuesday" => "3Tuesday",
                                "Wednesday" => "4Wednesday",
                                "Thursday" => "5Thursday",
                                "Friday" => "6Friday",
                                "Saturday" => "7Saturday",
                                "Sunday" => "8Sunday",
                                _ => dayName,
                            };
                            model.SentOn = dayName;
                        }
                    }

                    model.IconImage = GetIcon(model.ScanSource!, model.CommentIcon!);

                    model.Items = model.Items!.Replace("Unsectioned model, Antagonist model", "Model")
                                              .Replace("Unsectioned model", "Model")
                                              .Replace("Antagonist model", "Model");


                    if (model.CommentIcon == "7")
                    {
                        model.Crowns = "";
                        model.Abutments = "";
                        model.Models = "";
                        model.TotalUnits = "0";
                        model.TotalUnitsWithPrefixZero = "00";
                        model.SentOn = "Design ready";
                    }

                    if (!string.IsNullOrEmpty(model.Crowns)) 
                    {
                        _ = int.TryParse(model.Crowns, out int crowns);
                        if (model.Side!.Equals("left", StringComparison.CurrentCultureIgnoreCase))
                        {
                            TotalCrownsLeftSide += crowns;
                            TotalUnitsLeftSide += crowns;

                            if (model.SentOn!.Equals("zToday"))
                                TotalUnitsTodayLeftSide += crowns;
                            
                            if (model.SentOn!.Equals("9Yesterday") && DateTime.Now.Hour < 5)
                                TotalUnitsTodayLeftSide += crowns;
                        }
                        
                        if (model.Side!.Equals("right", StringComparison.CurrentCultureIgnoreCase))
                        {
                            TotalCrownsRightSide += crowns;
                            TotalUnitsRightSide += crowns;

                            if (model.SentOn!.Equals("zToday"))
                                TotalUnitsTodayRightSide += crowns;
                            if (model.SentOn!.Equals("9Yesterday") && DateTime.Now.Hour < 5)
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

                            if (model.SentOn!.Equals("zToday"))
                                TotalUnitsTodayLeftSide += abutments;
                            if (model.SentOn!.Equals("9Yesterday") && DateTime.Now.Hour < 5)
                                TotalUnitsTodayLeftSide += abutments;
                        }
                        
                        if (model.Side!.Equals("right", StringComparison.CurrentCultureIgnoreCase))
                        {
                            TotalAbutmentsRightSide += abutments;
                            TotalUnitsRightSide += abutments;

                            if (model.SentOn!.Equals("zToday"))
                                TotalUnitsTodayRightSide += abutments;
                            if (model.SentOn!.Equals("9Yesterday") && DateTime.Now.Hour < 5)
                                TotalUnitsTodayRightSide += abutments;
                        }
                    }

                    if (model.Side!.Equals("left", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (model.SentOn!.Equals("zToday"))
                            TotalOrdersTodayLeftSide++;
                        if (model.SentOn!.Equals("9Yesterday") && DateTime.Now.Hour < 5)
                            TotalOrdersTodayLeftSide++;

                        TotalOrdersLeftSide++;
                    }
                    
                    if (model.Side!.Equals("right", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (model.SentOn!.Equals("zToday"))
                            TotalOrdersTodayRightSide++;
                        if (model.SentOn!.Equals("9Yesterday") && DateTime.Now.Hour < 5)
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

                    if (model.Rush == "1")
                    {
                        model.CommentColor = "Crimson";
                        model.SentOn = "0RUSH";
                    }

                    if (model.Comment is not null)
                    {
                        if (model.Comment.StartsWith("This case is NOT in"))
                        {
                            model.CommentColor = "Gray";
                            model.SentOn = "No scan file";
                        }
                    }
                }

            }).Wait();

            TotalOrdersLeftOversLeftSide = TotalOrdersLeftSide - TotalOrdersTodayLeftSide;
            TotalOrdersLeftOversRightSide = TotalOrdersRightSide - TotalOrdersTodayRightSide;

            TotalUnitsLeftOverLeftSide = TotalUnitsLeftSide - TotalUnitsTodayLeftSide;
            TotalUnitsLeftOverRightSide = TotalUnitsRightSide - TotalUnitsTodayRightSide;

            sortedModelList = [.. modelList.OrderBy(x => x.SentOn).ThenByDescending(x => x.Rush).ThenBy(x => x.CommentIcon).ThenByDescending(x => x.TotalUnitsWithPrefixZero)];


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
            Debug.WriteLine($"MVM {ex.Message}");
        }
        http.Dispose();
        handler.Dispose();
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
                }

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"MVM {ex.Message}");
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
            Debug.WriteLine($"MVM {ex.Message}");
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
            Debug.WriteLine($"MVM {ex.Message}");
        }
        http.Dispose();
        handler.Dispose();
        return false;
    }
}
