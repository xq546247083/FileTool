using System.Text.RegularExpressions;

namespace FileTool
{
    public static class StringHelper
    {
        public static bool HasChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        public static bool HasEnCh(string str)
        {
            return Regex.IsMatch(str, @"[A-Za-z]");
        }
    }
}