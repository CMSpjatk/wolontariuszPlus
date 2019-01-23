using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Common
{
    public class FormFilesManagement : IFormFilesManagement
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private static readonly Random rand = new Random();

        public FormFilesManagement(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string GetPathToRandomStockImage()
        {
            var eventsImagesFolder = Path.Combine(GetUploadFolderAbsolutePath(), "stock-event-images");

            var files = Directory.EnumerateFiles(eventsImagesFolder).ToList();
            var relativePath = files[rand.Next(files.Count)];

            relativePath = relativePath.Replace(GetUploadFolderAbsolutePath(), "");
            return relativePath;
        }

        public void RemoveFileFromFileSystem(string relativePath)
        { 
            var path = Path.Combine(GetUploadFolderAbsolutePath(), relativePath);
            System.IO.File.Delete(path);
        }

        public void DeleteWholeEventFolder(int eventId)
        {
            var path = Path.Combine(
                GetUploadFolderAbsolutePath(),
                Properties.Resources.EventFilesUploadFolderName,
                eventId.ToString());

            Directory.Delete(path, true);
        }

        public string SaveFileToFileSystem(IFormFile formFile, int eventId)
        {
            if (eventId < 1)
            {
                throw new ArgumentOutOfRangeException("Provided eventId is not valid");
            }

            string relativePath = string.Empty;

            if (formFile != null)
            {
                if (formFile.Length > 0)
                {
                    Guid guid = Guid.NewGuid();

                    relativePath = Path.Combine(
                        Properties.Resources.EventFilesUploadFolderName,
                        eventId.ToString(),
                        $"{guid}_{formFile.FileName}");

                    var fileAbsolutePath = Path.Combine(GetUploadFolderAbsolutePath(), relativePath);

                    Directory.CreateDirectory(Path.GetDirectoryName(fileAbsolutePath));

                    using (var stream = new FileStream(fileAbsolutePath, FileMode.Create))
                    {
                        formFile.CopyTo(stream);
                    }
                }
            }

            return relativePath;
        }
        
        public string GetUploadFolderAbsolutePath()
        {
            return Path.Combine(_hostingEnvironment.ContentRootPath, Properties.Resources.UploadsFolderName);
        }
    }
}
