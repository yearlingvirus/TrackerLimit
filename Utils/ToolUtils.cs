using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLimit.Utils
{
    public static class ToolUtils
    {
        public static string ExtractSiteName(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            var uri = new Uri(url);
            string host = uri.Host;
            var parts = host.Split('.'); // 分割域名
            if (parts.Length >= 2)
            {
                return parts[parts.Length - 2]; // 获取倒数第二部分
            }

            return string.Empty;
        }

        public static string? ReadConsoleLine()
        {
            return Console.ReadLine();
        }

        public static void PrintConsoleLine(string text)
        {
            Console.WriteLine(text);
        }

        public static void PrintTextWithTopDash(Action action)
        {
            PrintDashLine();

            action?.Invoke();
        }

        public static void PrintTextWithTopDash(string text)
        {
            PrintDashLine();

            Console.WriteLine(text);
        }

        public static void PrintDashLine()
        {
            int consoleWidth = Console.WindowWidth;
            string dashLine = new string('-', consoleWidth - 1); // 减1以避免自动换行
            PrintConsoleLine(dashLine);
        }
    }
}
