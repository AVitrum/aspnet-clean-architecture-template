namespace Infrastructure.Repositories;

public class FileRepository : BaseRepository<FileEntity>, IFileRepository
{
    private readonly ApplicationDbContext _context;
    
    public FileRepository(ApplicationDbContext context) : base(context, context.Files)
    {
        _context = context;
    }


    public async Task<Guid> UploadAsync(FileEntity file)
    {
        await _context.Files.AddAsync(file);
        await _context.SaveChangesAsync();
        return file.Id;
    }
}