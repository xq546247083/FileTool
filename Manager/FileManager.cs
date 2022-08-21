using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace FileTool
{
    public static class FileManager
    {
        // 移动所有的下级文件到当前目录
        public static void MoveAllSubFileToDir(string currentDir, string moveDir)
        {
            var suiDirList = Directory.GetDirectories(currentDir);
            foreach (var dir in suiDirList)
            {
                MoveAllSubFileToDir(dir, moveDir);
            }

            if (moveDir == currentDir)
            {
                return;
            }

            var fileList = Directory.GetFiles(currentDir);
            foreach (var file in fileList)
            {
                FileHelper.MoveToDir(file, moveDir);
            }
        }

        // 按照大小顺序重命名文件
        public static void ReaNameAllSubFileByFileSize(string currentDir)
        {
            var allSubFileList = FileHelper.GetAllSubFile(currentDir);
            var allSubFileInfoList = allSubFileList.Select(r => new FileInfo(r)).OrderBy(r => r.Length);

            var i = 1;
            foreach (var subFileInfo in allSubFileInfoList)
            {
                if (subFileInfo.Name.Contains("FileTool"))
                {
                    continue;
                }

                string subFileInfoExName = subFileInfo.Name.Substring(subFileInfo.Name.LastIndexOf(".") + 1);
                FileHelper.MoveToDir(subFileInfo.FullName, currentDir, $"{i}.{subFileInfoExName}");
                i++;
            }
        }

        // 将【当前目录以及所有的子目录】的所有文件，把名字包含中文的文件，移动到ChineseFile目录下
        public static void MoveAllSubChineseFileToDir(string dir)
        {
            var chineseDir = Path.Combine(dir, "ChineseFile");

            var subFileList = FileHelper.GetAllSubFile(dir);
            foreach (var subFileStr in subFileList)
            {
                var subFile = new FileInfo(subFileStr);
                if (subFile.Name.Contains("FileTool"))
                {
                    continue;
                }

                if (StringHelper.HasChinese(subFile.Name))
                {
                    FileHelper.MoveToDir(subFileStr, chineseDir);
                }
            }
        }

        // 将【当前目录以及所有的子目录】的所有文件，包含【特定字符】的文件，移动到【特定字符】目录下
        public static void MoveAllSubSomeFileToDir(string dir, string containStr)
        {
            var containDir = Path.Combine(dir, containStr);
            if (containDir.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                Console.WriteLine("输入的支付无效，不能有特殊字符");
                return;
            }

            containStr = containStr.ToLower();
            var subFileList = FileHelper.GetAllSubFile(dir);
            foreach (var subFileStr in subFileList)
            {
                var subFile = new FileInfo(subFileStr);
                if (subFile.Name.Contains("FileTool"))
                {
                    continue;
                }

                if (subFile.Name.ToLower().Contains(containStr))
                {
                    FileHelper.MoveToDir(subFileStr, containDir);
                }
            }
        }

        // 将【当前目录以及所有的子目录】的所有文件，把名字包含中文的文件，移动到EnFile目录下
        public static void MoveAllSubEnFileToDir(string dir)
        {
            var enDir = Path.Combine(dir, "EnFile");

            var subFileList = FileHelper.GetAllSubFile(dir);
            foreach (var subFileStr in subFileList)
            {
                var subFile = new FileInfo(subFileStr);
                if (subFile.Name.Contains("FileTool"))
                {
                    continue;
                }

                var subFileNameStr = subFile.Name.Substring(0, subFile.Name.LastIndexOf("."));
                if (StringHelper.HasEnCh(subFileNameStr))
                {
                    FileHelper.MoveToDir(subFileStr, enDir);
                }
            }
        }

        // 移动所有的wallpaper文件到当前目录
        public static void MoveAllWallpaperFileToDir(string currentDir, string moveDir)
        {
            var subFileList = FileHelper.GetAllSubFile(currentDir);
            var projectFileList = subFileList.Where(r =>
            {
                var subFile = new FileInfo(r);
                return subFile.Name.Contains("project");
            }).ToList();

            foreach (var projectFileStr in projectFileList)
            {
                try
                {
                    var projectFile = new FileInfo(projectFileStr);

                    var projectTextLines = File.ReadLines(projectFileStr).Select(r => r.Replace("\t", ""));
                    var projectTextStr = string.Join("", projectTextLines);
                    var projectObj = JsonConvert.DeserializeObject<dynamic>(projectTextStr);

                    string wallpaperFileName = projectObj.file;
                    string wallpaperExName = wallpaperFileName.Substring(wallpaperFileName.LastIndexOf(".") + 1);
                    string wallpapeTitle = $"{projectObj.title}.{wallpaperExName}";
                    FileHelper.MoveToDir(Path.Combine(projectFile.DirectoryName, wallpaperFileName), moveDir, wallpapeTitle);

                    string wallpaperPreviewFileName = projectObj.preview;
                    string wallpaperPreviewExName = wallpaperPreviewFileName.Substring(wallpaperPreviewFileName.LastIndexOf(".") + 1);
                    string wallpapePreviewTitle = $"{projectObj.title}_Preview.{wallpaperPreviewExName}";
                    FileHelper.MoveToDir(Path.Combine(projectFile.DirectoryName, wallpaperPreviewFileName), moveDir, wallpapePreviewTitle);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"序列化project失败，错误信息为:{ex}");
                }
            }
        }

        // 将【当前目录以及所有的子目录】的所有文件，按照文件类型分类，并移动到分类目录
        public static void MoveAllSubFileToTypeDir(string dir)
        {
            var subFileList = FileHelper.GetAllSubFile(dir);
            foreach (var subFileStr in subFileList)
            {
                var subFile = new FileInfo(subFileStr);
                if (subFile.Name.Contains("FileTool"))
                {
                    continue;
                }

                var type = subFile.Name.Substring(subFile.Name.LastIndexOf(".") + 1);
                FileHelper.MoveToDir(subFileStr, Path.Combine(dir, type));
            }
        }

        // 移动所有的相同文件到ReapetFile目录
        public static void MoveAllReapetFileToDir(string dir)
        {
            var reapetDir = Path.Combine(dir, "ReapetFile");
            var sameFileList = GetAllSameFile(dir);
            foreach (var sameFile in sameFileList)
            {
                FileHelper.MoveToDir(sameFile, reapetDir);
            }
        }

        // 获取所有的相同文件
        public static List<string> GetAllSameFile(string dir)
        {
            var subFileList = FileHelper.GetAllSubFile(dir);
            var fileDic = FileHelper.ToDicByFileSize(subFileList);

            return fileDic.Values.Where(r => r.Count > 1).SelectMany(r => r).ToList();
        }

        // 移动所有的合适大小的文件到SuitFile目录
        public static void MoveAllSuitFileToDir(string dir, string minFileSizeStr, string maxFileSizeStr)
        {
            if (!long.TryParse(minFileSizeStr, out var minFileSizeNum))
            {
                Console.WriteLine("输入的最小值有误");
                return;
            }

            if (!long.TryParse(maxFileSizeStr, out var maxFileSizeNum))
            {
                Console.WriteLine("输入的最大值有误");
                return;
            }

            var reapetDir = Path.Combine(dir, "SuitFile");
            var subFileList = FileHelper.GetAllSubFile(dir);
            foreach (var subFileStr in subFileList)
            {
                var subFile = new FileInfo(subFileStr);
                if (subFile.Name.Contains("FileTool"))
                {
                    continue;
                }

                var fileLenght = subFile.Length / 1024;
                if (fileLenght >= minFileSizeNum && fileLenght <= maxFileSizeNum)
                {
                    FileHelper.MoveToDir(subFileStr, reapetDir);
                }
            }
        }

        // 按照大小快速分组到文件夹
        public static void MoveFileToDirByGroup(string dir, string fileCount)
        {
            if (!long.TryParse(fileCount, out var fileCountNum))
            {
                Console.WriteLine("输入有误");
                return;
            }

            // 获取所有文件，并排序
            var subFileList = FileHelper.GetAllSubFile(dir);
            var subFileInfoList = new List<FileInfo>();
            foreach (var subFileStr in subFileList)
            {
                var subFile = new FileInfo(subFileStr);
                if (subFile.Name.Contains("FileTool"))
                {
                    continue;
                }

                subFileInfoList.Add(subFile);
            }

            // 按顺序移动到文件夹
            subFileInfoList = subFileInfoList.OrderBy(r => r.Length).ToList();
            var i = 0;
            foreach (var subFile in subFileInfoList)
            {
                var currenPage = (int)i / fileCountNum + 1;

                var currentDir = Path.Combine(dir, currenPage.ToString());
                FileHelper.MoveToDir(subFile.FullName, currentDir);

                i++;
            }
        }
    }
}