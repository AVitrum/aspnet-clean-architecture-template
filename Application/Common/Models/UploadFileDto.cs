using Microsoft.AspNetCore.Http;

namespace Application.Common.Models;

public class UploadFileDto
{
    public IFormFile? File { get; set; }
}