using Domain.Common;

namespace Domain.Entities;

public class FileEntity : BaseEntity
{
    public required string Name { get; set; }
    public required string ContentType { get; set; }
    public required byte[] Data { get; set; }
}