using System;
using System.Reflection;

namespace FileTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            var executingFilePath = Assembly.GetExecutingAssembly().Location;
            var currentDir = System.IO.Path.GetDirectoryName(executingFilePath);
            if (currentDir == null)
            {
                return;
            }

            while (true)
            {
                try
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
                    else if (key == "3")
                    {
                        Console.Write("请输入最小文件大小(kb):");
                        var minFileSize = Console.ReadLine();
                        Console.Write("请输入最大文件大小(kb):");
                        var maxFileSize = Console.ReadLine();

                        FileManager.MoveAllSuitFileToDir(currentDir, minFileSize, maxFileSize);
                    }
                    else if (key == "4")
                    {
                        FileManager.MoveAllWallpaperFileToDir(currentDir, currentDir);
                    }
                    else if (key == "5")
                    {
                        FileManager.MoveAllSubFileToTypeDir(currentDir);
                    }
                    else if (key == "6")
                    {
                        FileManager.MoveAllSubChineseFileToDir(currentDir);
                    }
                    else if (key == "7")
                    {
                        FileManager.MoveAllSubEnFileToDir(currentDir);
                    }
                    else if (key == "8")
                    {
                        FileManager.MoveAllSubFileToDir(currentDir, currentDir);
                        FileManager.ReaNameAllSubFileByFileSize(currentDir);
                    }
                    else if (key == "9")
                    {
                        Console.Write("请输入要包含的字符:");
                        var containStr = Console.ReadLine();
                        FileManager.MoveAllSubSomeFileToDir(currentDir, containStr);
                    }
                    else if (key == "0")
                    {
                        Console.WriteLine("输入错误的数字");
                        break;
                    }

                    PrintEndTip();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"出现错误:{ex}");
                }
            }
        }

        private static void PrintStartTip()
        {
            Console.WriteLine("本程序是一个简单的文件工具");
            Console.WriteLine("1、将【当前目录以及所有的子目录】的所有文件，移动到当前目录下");
            Console.WriteLine("2、将【当前目录以及所有的子目录】的重复文件，移动到ReapetFile目录下（按照文件大小判断）");
            Console.WriteLine("3、将【当前目录以及所有的子目录】的符合大小条件的文件，移动到SuitFile目录下");
            Console.WriteLine("4、将【当前目录以及所有的子目录】的所有Wallpaper Engine文件，移动到当前目录下（针对Wallpaper Engine,文件名为自动重名为壁纸名字）");
            Console.WriteLine("5、将【当前目录以及所有的子目录】的所有文件，按照文件类型分类，并移动到分类目录下");
            Console.WriteLine("6、将【当前目录以及所有的子目录】的所有文件，把名字包含中文的文件，移动到ChineseFile目录下");
            Console.WriteLine("7、将【当前目录以及所有的子目录】的所有文件，把名字包含英文的文件，移动到EnFile目录下");
            Console.WriteLine("8、将【当前目录以及所有的子目录】的所有文件，移动到当前目录下，并按照大小顺序重命名文件");
            Console.WriteLine("9、将【当前目录以及所有的子目录】的所有文件，包含【特定字符】的文件，移动到【特定字符】目录下");
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

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs e)
        {
            var assemblyName = new AssemblyName(e.Name).Name;
            string resourceName = "FileTool.DLL." + assemblyName + ".dll";
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return null;
                }

                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }
    }
}