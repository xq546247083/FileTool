using System;

namespace FileTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("文件工具");
            Console.WriteLine("1、本程序会将【当前目录以及所有的子目录】的所有文件，移动到当前目录下");
            Console.Write("输入对应的数字：");

            var key = Console.ReadLine();

            Console.WriteLine("正在操作，请稍候...");
            if (key == "1")
            {
                var currentDir = System.Environment.CurrentDirectory;
                FileManager.MoveAllFileToCurrentDir(currentDir, currentDir);
            }

            Console.WriteLine("操作完成，输入任意字符退出");
            Console.Read();
        }
    }
}