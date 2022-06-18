using System;

namespace FileTool
{
    public static class FileManager
    {
        public static void MoveAllFileToCurrentDir(string moveDir, string currentDir)
        {
            var suiDirList = Directory.GetDirectories(currentDir);
            foreach (var dir in suiDirList)
            {
                MoveAllFileToCurrentDir(moveDir, dir);
            }

            if (moveDir == currentDir)
            {
                return;
            }
            
            var fileList = Directory.GetFiles(currentDir);
            foreach (var file in fileList)
            {
                FileHelper.CopyToFile(file, moveDir);
            }
        }
    }
}