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
    }
}