using System;
using System.IO;
using System.Text.RegularExpressions;

public class Test
{
    public static string getDirForOutputlog(string game)
    {
        var logPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"Low\miHoYo\" + game + @"\output_log.txt";
        if (!File.Exists(logPath)) { Console.WriteLine("目录异常."); return null; }
        using (var sr = new StreamReader(new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
        {
            string target = null;
            var regex = new Regex(@"[A-Z]:\\[^:]+\\SDKCaches", RegexOptions.Compiled);
            while ((target = sr.ReadLine()) != null)
            {
                if (target.Contains("SDKCaches"))
                {
                    var match = regex.Match(target);
                    if (match.Success) return Path.GetDirectoryName(match.Value.Trim());
                    else Console.WriteLine("正则未匹配到路径");
                    return null;
                }
            }
            Console.WriteLine("未找到匹配行");
            return null;
        }
    }
    public static string getDirForPlayerlog(string game)
    {
        var logPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"Low\miHoYo\" + game+ @"\Player.log";
        if (!File.Exists(logPath)) { Console.WriteLine("目录异常."); return null; }
        using (var sr = new StreamReader(new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
        {
            string target = null;
            var regex = new Regex(@"Loading player data from\s*(.+)", RegexOptions.Compiled);
            while ((target = sr.ReadLine()) != null)
            {
                if (target.Contains("Loading player data from"))
                {
                    var match = regex.Match(target);
                    if (match.Success) return Path.GetDirectoryName(match.Groups[1].Value.Trim());
                    else Console.WriteLine("正则未匹配到路径");
                    return null;
                }
            }
            Console.WriteLine("未找到匹配行");
            return null;
        }
    }
    public static string getUrlFordata_2(string path)
    {
        if (string.IsNullOrEmpty(path))return "未能获取到数据目录.";
        {
            var webCachesPath = Path.Combine(path, "webCaches");
            var versionDirs = Directory.GetDirectories(webCachesPath);
            if (versionDirs.Length == 0) return "未找到版本文件夹";
            Array.Sort(versionDirs);
            var latestVersionDir = versionDirs[versionDirs.Length - 1];
            var dataFile = Path.Combine(latestVersionDir, "Cache", "Cache_Data", "data_2");
            var regex = new Regex(@"https://[^\s""':]*getGachaLog[^\s""':]*&end_id", RegexOptions.Compiled);
            string lastUrl = null;
            using (var sr = new StreamReader(new FileStream(dataFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var match = regex.Match(line);
                    if (match.Success) lastUrl = match.Value;
                }
            }
            return lastUrl ?? "未找到 getGachaLog 网页链接。";
        }
    }
    public static string getGachaLogUrl()
    {
        Console.WriteLine("请选择游戏：\n\t1. 原神  \n\t2. 崩坏：星穹铁道  \n\t3. 绝区零\n");
        string gameDir = "";
        while (true)
        {
            var key = Console.ReadKey(true);
            switch (key.KeyChar)
            {
                case '1': gameDir = getDirForOutputlog("原神"); break;
                case '2': gameDir = getDirForPlayerlog("崩坏：星穹铁道"); break;
                case '3': gameDir = getDirForOutputlog("绝区零"); break;
                default:
                    Console.WriteLine("无效输入，请按 1 或 2 或 3 选择游戏.\n");
                    continue;
            }
            break;
        }
        var result = getUrlFordata_2(gameDir);
        Console.WriteLine(result);
        return result;
    }
    public static void Main()
    {
        Console.WriteLine(getGachaLogUrl());
    }
}
