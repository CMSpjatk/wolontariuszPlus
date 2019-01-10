using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Common
{
    public interface IFormFilesManagement
    {
        void RemoveFileFromFileSystem(string relativePath);
        void DeleteWholeEventFolder(int eventId);
        string SaveFileToFileSystem(IFormFile formFile, int eventId);
        string GetUploadFolderAbsolutePath();
        string GetPathToRandomStockImage();
    }
}
