namespace CaseChecker.MVVM.Model;

public class SentOutCasesModel
{
    public string? OrderID { get; set; }
    public string? Crowns { get; set; } = "0";
    public string? Abutments { get; set; } = "0";
    public string? Models { get; set; } = "0";
    public string? TotalUnits { get; set; } = "0";
    public string? Comment { get; set; }
    public string? SentOn { get; set; }
    public string? Items { get; set; }
    public string? Manufacturer { get; set; }
    public string? Rush { get; set; } = "False";
    public string? Side { get; set; }
    public string? Directory { get; set; }
    public string? MaxProcessStatusID { get; set; }
    public string? ProcessLockID { get; set; }
    public string? ScanSource { get; set; }
    public string? CommentIcon { get; set; } = "-1";
    public string? CommentColor { get; set; }
    public string? CommentIn3Shape { get; set; }
    public string? IconImage { get; set; }

    public string? TotalUnitsWithPrefixZero { get; set; } = "0";

    public string? RushCaseComment { get; set; } = "(RUSH CASE)";
    public string? RushForMorningComment { get; set; } = "RUSH case for the morning!";
    public string? OrderDesignedComment { get; set; } = "(Order received by the lab, but auto import failed)";
}