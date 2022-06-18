using System.Reflection;

namespace FileTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var executingFilePath = Assembly.GetExecutingAssembly().Location;
            var currentDir = System.IO.Path.GetDirectoryName(executingFilePath);
            if (currentDir == null)
            {
                return;
            }

            while (true)
            {
                PrintStartTip();

                var key = Console.ReadLine();
                Console.WriteLine("正在操作，请稍候...");
                if (key == "1")
                {
                    FileManager.MoveAllSubFileToDir(currentDir, currentDir);
                }
                else if (key == "2")
                {
                    FileManager.MoveAllReapetFileToDir(currentDir);
                }
                else if (key == "0")
                {
                    break;
                }

                PrintEndTip();
            }
        }

        private static void PrintStartTip()
        {
            Console.WriteLine("本程序是一个简单的文件工具");
            Console.WriteLine("1、将【当前目录以及所有的子目录】的所有文件，移动到当前目录下");
            Console.WriteLine("2、将【当前目录以及所有的子目录】的重复文件，移动到ReapetFile目录下（按照文件大小判断）");
            Console.WriteLine("0、退出程序");
            Console.Write("输入对应的数字：");
        }

        private static void PrintEndTip()
        {
            Console.WriteLine("操作完成!");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
        }
    }
}