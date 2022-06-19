using System.Text.RegularExpressions;

namespace FileTool
{
    public static class StringHelper
    {
        public static bool HasChinese(this string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }
    }
}