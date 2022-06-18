using System;

namespace FileTool
{
    public static class FileHelper
    {
        // 移动文件到目录
        public static void MoveToFile(string sourceFileName, string targetFolderPath)
        {
            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
            }

            string fileName = Path.GetFileName(sourceFileName);
            string targetPath = Path.Combine(targetFolderPath, fileName);

            FileInfo file = new FileInfo(sourceFileName);
            if (file.Exists)
            {
                file.MoveTo(targetPath, true);
            }
        }

        // 获取所有的下级文件
        public static List<string> GetAllSubFile(string parentDir)
        {
            var result = new List<string>();

            var suiDirList = Directory.GetDirectories(parentDir);
            foreach (var dir in suiDirList)
            {
                result.AddRange(GetAllSubFile(dir));
            }

            var fileList = Directory.GetFiles(parentDir);
            foreach (var file in fileList)
            {
                result.Add(file);
            }

            return result;
        }

        // 文件列表，根据文件大小组成字典
        public static Dictionary<long, List<string>> ToDicByFileSize(List<string> fileList)
        {
            var fileDic = new Dictionary<long, List<string>>();
            foreach (var subFileStr in fileList)
            {
                var subFlie = new FileInfo(subFileStr);
                var subFileLength = subFlie.Length;
                if (!fileDic.ContainsKey(subFileLength))
                {
                    fileDic[subFileLength] = new List<string>();
                }

                fileDic[subFileLength].Add(subFileStr);
            }

            return fileDic;
        }
    }
}