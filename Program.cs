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
                    if (key == "100")
                    {
                        FileManager.MoveAllSubFileToDir(currentDir, currentDir);
                    }
                    else if (key == "101")
                    {
                        FileManager.MoveAllReapetFileToDir(currentDir);
                    }
                    else if (key == "102")
                    {
                        FileManager.MoveAllSubFileToTypeDir(currentDir);
                    }
                    else if (key == "200")
                    {
                        FileManager.MoveAllSubFileToDir(currentDir, currentDir);
                        FileManager.ReaNameAllSubFileByFileSize(currentDir);
                    }
                    else if (key == "201")
                    {
                        FileManager.MoveAllSubChineseFileToDir(currentDir);
                    }
                    else if (key == "202")
                    {
                        FileManager.MoveAllSubEnFileToDir(currentDir);
                    }
                    else if (key == "203")
                    {
                        FileManager.RenameAllFileWithDirName(currentDir);
                    }
                    else if (key == "300")
                    {
                        FileManager.MoveAllWallpaperFileToDir(currentDir, currentDir);
                    }
                    else if (key == "900")
                    {
                        Console.Write("请输入要包含的字符:");
                        var containStr = Console.ReadLine();
                        FileManager.MoveAllSubSomeFileToDir(currentDir, containStr);
                    }
                    else if (key == "901")
                    {
                        Console.Write("请输入最小文件大小(kb):");
                        var minFileSize = Console.ReadLine();
                        Console.Write("请输入最大文件大小(kb):");
                        var maxFileSize = Console.ReadLine();

                        FileManager.MoveAllSuitFileToDir(currentDir, minFileSize, maxFileSize);
                    }
                    else if (key == "902")
                    {
                        Console.Write("请输入多少个文件一组:");
                        var containStr = Console.ReadLine();
                        FileManager.MoveFileToDirByGroup(currentDir, containStr);
                    }
                    else if (key == "903")
                    {
                        Console.Write("请输入【当前目录】的第一个文件夹名:");
                        var firstDirStr = Console.ReadLine();
                        Console.Write("请输入【当前目录】的第而个文件夹名:");
                        var secondDirStr = Console.ReadLine();
                        FileManager.MoveCompareFileToDir(currentDir, firstDirStr, secondDirStr);
                    }
                    else if (key == "904")
                    {
                        Console.Write("请输入【当前目录】的第一个文件夹名:");
                        var firstDirStr = Console.ReadLine();
                        Console.Write("请输入【当前目录】的第而个文件夹名:");
                        var secondDirStr = Console.ReadLine();
                        FileManager.MoveFullCompareFileToDir(currentDir, firstDirStr, secondDirStr);
                    }
                    else
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
            Console.WriteLine("本程序是一个简单的文件工具,输入数字，开始整理。");
            Console.WriteLine("注意：1、如果名字相同，整理后一般只会保留一个文件 2、所有操作都不可以恢复，注意备份。");
            Console.WriteLine("100、将【当前目录以及所有的子目录】的所有文件，移动到当前目录下");
            Console.WriteLine("101、将【当前目录以及所有的子目录】的重复文件（按照文件大小判断），移动到ReapetFile目录下（）");
            Console.WriteLine("102、将【当前目录以及所有的子目录】的所有文件，按照文件类型分类，并移动到分类目录下");
            Console.WriteLine("200、将【当前目录以及所有的子目录】的所有文件，移动到当前目录下，并按照大小顺序重命名文件");
            Console.WriteLine("201、将【当前目录以及所有的子目录】的所有文件，把名字包含中文的文件，移动到ChineseFile目录下");
            Console.WriteLine("202、将【当前目录以及所有的子目录】的所有文件，把名字包含英文的文件，移动到EnFile目录下");
            Console.WriteLine("203、将【当前目录以及所有的子目录】的所有文件，重命名为：“当前文件夹名”+“ 文件名”");
            Console.WriteLine("300、将【当前目录以及所有的子目录】的所有Wallpaper Engine文件，移动到当前目录下（针对Wallpaper Engine,文件名为自动重名为壁纸名字）");
            Console.WriteLine("900、将【当前目录以及所有的子目录】的所有文件，包含【特定字符】的文件，移动到【特定字符】目录下");
            Console.WriteLine("901、将【当前目录以及所有的子目录】的符合大小条件的文件，移动到SuitFile目录下");
            Console.WriteLine("902、将【当前目录以及所有的子目录】的所有文件，按照大小快速分组到文件夹");
            Console.WriteLine("903、将【当前目录】的两个文件夹的所有文件按照名字对比，将第二个文件夹不相同的文件移动到NotCompare目录");
            Console.WriteLine("904、将【当前目录】的两个文件夹的所有文件按照名字和大小对比，将第二个文件夹不相同的文件移动到FullNotCompare目录");
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