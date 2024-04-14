﻿using System.Security.Cryptography;
using System.Text;
using System.Management;
using System.Windows;

namespace CaseChecker.MVVM.Core;

public class GetDeviceId
{
    public static string GetDeviceID()
    {
        string? id = null;
        
        try
        {
            id = MotherboardInfo.SerialNumber;
        }
        catch (Exception) 
        { 
        }

        id ??= Environment.MachineName;

        return CreateMD5(id);
    }

    public static string CreateMD5(string input)
    {
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = MD5.HashData(inputBytes);

        return Convert.ToHexString(hashBytes).ToLower();
    }

    public static string ReadDeviceInfo()
    {
        StringBuilder sb = new();

        sb.AppendLine("{");
        sb.AppendLine("\"Model\" : \"" + MotherboardInfo.Product + "\", ");
        sb.AppendLine("\"Manufacturer\" : \"" + MotherboardInfo.Manufacturer + "\", ");
        sb.AppendLine("\"Name\" : \"" + Environment.MachineName + "\", ");
        sb.AppendLine("\"OSVersion\" : \"" + OSVersion() + "\", ");
        sb.AppendLine("\"Idiom\" : \"" + "Desktop" + "\", ");
        sb.AppendLine("\"Platform\" : \"" + "WPF" + "\", ");
        sb.AppendLine("\"VirtualDevice\" : \"False\"");
        sb.AppendLine("}");

        return sb.ToString();
    }

    public static string OSVersion()
    {
        try
        {
            string r = "";
            using ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectCollection information = searcher.Get();
            if (information != null)
            {
                foreach (ManagementObject obj in information.Cast<ManagementObject>())
                {
                    r = obj["Caption"].ToString() + " - " + obj["OSArchitecture"].ToString();
                }
            }
            r = r.Replace("NT 5.1.2600", "XP");
            r = r.Replace("NT 5.2.3790", "Server 2003");
            return r.Replace("Microsoft", "").Trim();
        }
        catch (Exception)
        {
        }
        return "Windows";
    }
}


static public class MotherboardInfo
{
    private static ManagementObjectSearcher baseboardSearcher = new("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
       
    static public string Product
    {
        get
        {
            try
            {
                foreach (ManagementObject queryObj in baseboardSearcher.Get().Cast<ManagementObject>())
                {
                    return queryObj["Product"].ToString();
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }
    }

    static public string Manufacturer
    {
        get
        {
            try
            {
                foreach (ManagementObject queryObj in baseboardSearcher.Get().Cast<ManagementObject>())
                {
                    return queryObj["Manufacturer"].ToString();
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }
    }

    static public string SerialNumber
    {
        get
        {
            try
            {
                foreach (ManagementObject queryObj in baseboardSearcher.Get().Cast<ManagementObject>())
                {
                    return queryObj["SerialNumber"].ToString();
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }
    }

    
}