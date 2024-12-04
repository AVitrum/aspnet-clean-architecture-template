using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Files;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;

    public FileService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }
    
    public async Task<Guid> UploadAsync(UploadFileDto fileDto)
    {
        using var memoryStream = new MemoryStream();
        await fileDto.File!.CopyToAsync(memoryStream);

        var file = new FileEntity
        {
            Name = fileDto.File.FileName,
            ContentType = fileDto.File.ContentType,
            Data = memoryStream.ToArray()
        };
        
        return await _fileRepository.UploadAsync(file);
    }

    public async Task<IFormFile> DownloadAsync(Guid fileId)
    {
        var file = await _fileRepository.GetByIdAsync(fileId);
        var stream = new MemoryStream(file.Data);

        return new FormFile(stream, 0, stream.Length, file.Name, file.Name)
        {
            Headers = new HeaderDictionary(),
            ContentType = file.ContentType 
        };
    }
}