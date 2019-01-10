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

        //public void ReplaceMaterialFilesWithEditRequestFiles(int materialId, int editRequestId, ICollection<BLL.Models.File> allNewFiles)
        //{
        //    if (materialId < 1 || editRequestId < 1)
        //    {
        //        throw new ArgumentOutOfRangeException("Provided materialId or editRequestId is not valid");
        //    }

        //    var materialFolderAbsolutePath = Path.Combine(
        //           GetUploadFolderAbsolutePath(),
        //           SharedResourcesLibrary.Properties.Resources.MaterialFilesUploadFolderName,
        //           materialId.ToString());

        //    var editRequestsFolder = Path.Combine(
        //        materialFolderAbsolutePath,
        //        SharedResourcesLibrary.Properties.Resources.EditRequestFilesUploadFolderName);

        //    RemoveOldFiles(materialFolderAbsolutePath, allNewFiles);

        //    if (allNewFiles?.Count > 0)
        //    {
        //        MoveNewFilesFromEditRequestToMaterial(materialFolderAbsolutePath, editRequestsFolder, editRequestId);
        //    }

        //    // delete all edit requests files connected with this material
        //    if (Directory.Exists(editRequestsFolder))
        //    {
        //        Directory.Delete(editRequestsFolder, true);
        //    }
        //}

        //private void RemoveOldFiles(string materialFolderAbsolutePath, ICollection<BLL.Models.File> newFiles)
        //{
        //    var existingFilesPaths = Directory.GetFiles(materialFolderAbsolutePath);

        //    foreach (var existingFilePath in existingFilesPaths)
        //    {
        //        var existingFile = newFiles.FirstOrDefault(ef => existingFilePath.EndsWith(ef.RelativePath));

        //        if (existingFile == null)
        //        {
        //            System.IO.File.Delete(existingFilePath);
        //        }
        //    }
        //}
        
        //private void MoveNewFilesFromEditRequestToMaterial(string materialFolderAbsolutePath, string editRequestsFolder, int editRequestId)
        //{
        //    var editRequestFolderAbsolutePath = Path.Combine(editRequestsFolder, editRequestId.ToString());

        //    if (Directory.Exists(editRequestFolderAbsolutePath))
        //    {
        //        var editRequestFilesPaths = Directory.GetFiles(editRequestFolderAbsolutePath);

        //        foreach (var filePath in editRequestFilesPaths)
        //        {
        //            var destination = Path.Combine(materialFolderAbsolutePath, Path.GetFileName(filePath));
        //            System.IO.File.Move(filePath, destination);
        //        }
        //    }
        //}

        //public bool ExistsInMaterial(IFormFile file, int materialId)
        //{
        //    var relativePath = Path.Combine(
        //        SharedResourcesLibrary.Properties.Resources.MaterialFilesUploadFolderName,
        //        materialId.ToString(),
        //        file.FileName);

        //    var fileAbsolutePath = Path.Combine(GetUploadFolderAbsolutePath(), relativePath);

        //    return System.IO.File.Exists(fileAbsolutePath);
        //}
    }
}
