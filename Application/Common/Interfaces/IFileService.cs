using Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces;

public interface IFileService
{
    Task<Guid> UploadAsync(UploadFileDto fileDto);
    Task<IFormFile> DownloadAsync(Guid fileId);
}