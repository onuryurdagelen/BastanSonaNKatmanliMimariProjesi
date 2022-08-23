using Business.Abstract;
using Core.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Business.Concrete
{
    public class FileManager : IFileService
    {
        public byte[] SaveConvertByteArrayToDatabase(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                var fileBytes = memoryStream.ToArray();
                string fileString = Convert.ToBase64String(fileBytes);
                return fileBytes;
            }
        }

        public string SaveFileToFtp(IFormFile file)
        {
            var fileFormat = file.FileName.Substring(file.FileName.LastIndexOf("."));
            fileFormat = fileFormat.ToLower();
            string fileName = Guid.NewGuid().ToString() + "_" + DateTime.Now.FullDateAndTimeStringWithUnderScore() + fileFormat;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://192.168.1.104/Images/" + fileName);
            request.Credentials = new NetworkCredential("onuryurdagelen", "159951eslem");
            request.Method = WebRequestMethods.Ftp.UploadFile;

            using (Stream ftpStream = request.GetRequestStream())
            {
                file.CopyTo(ftpStream);
            }
            return fileName;
        }

        public string SaveFileToServer(IFormFile file, string filePath)
        {
            var fileFormat = file.FileName.Substring(file.FileName.LastIndexOf("."));
            fileFormat = fileFormat.ToLower();
            string fileName = Guid.NewGuid().ToString() + "_" + DateTime.Now.FullDateAndTimeStringWithUnderScore() + fileFormat;

            filePath = "./Assets/images/" + fileName;
            using (var stream = System.IO.File.Create(filePath))
            {
                file.CopyTo(stream);
            }
            return fileName;
        }
    }
}
