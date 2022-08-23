using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IFileService
    {
        string SaveFileToServer(IFormFile file,string filePath);
        string SaveFileToFtp(IFormFile file);
        byte[] SaveConvertByteArrayToDatabase(IFormFile file);
    }
}
