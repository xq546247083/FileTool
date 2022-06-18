using System;

namespace FileTool
{
    public static class FileManager
    {
        // 移动所有的下级文件到当前目录
        public static void MoveAllSubFileToDir(string moveDir, string currentDir)
        {
            var suiDirList = Directory.GetDirectories(currentDir);
            foreach (var dir in suiDirList)
            {
                MoveAllSubFileToDir(moveDir, dir);
            }

            if (moveDir == currentDir)
            {
                return;
            }

            var fileList = Directory.GetFiles(currentDir);
            foreach (var file in fileList)
            {
                FileHelper.MoveToFile(file, moveDir);
            }
        }

        // 移动所有的相同文件到ReapetFile目录
        public static void MoveAllReapetFileToDir(string dir)
        {
            var reapetDir = Path.Combine(dir, "ReapetFile");
            var sameFileList = GetAllSameFile(dir);
            foreach (var sameFile in sameFileList)
            {
                FileHelper.MoveToFile(sameFile, reapetDir);
            }
        }

        // 获取所有的相同文件
        public static List<string> GetAllSameFile(string dir)
        {
            var subFileList = FileHelper.GetAllSubFile(dir);
            var fileDic = FileHelper.ToDicByFileSize(subFileList);

            return fileDic.Values.Where(r => r.Count > 1).SelectMany(r => r).ToList();
        }
    }
}