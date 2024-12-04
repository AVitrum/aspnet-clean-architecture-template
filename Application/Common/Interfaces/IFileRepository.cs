using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IFileRepository : IBaseRepository<FileEntity>
{
    Task<Guid> UploadAsync(FileEntity file);
}