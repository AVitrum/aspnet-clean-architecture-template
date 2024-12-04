namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }
    
    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] UploadFileDto fileDto)
    {
        var result = await _fileService.UploadAsync(fileDto);
        
        if (fileDto.File is null || fileDto.File.Length == 0) return BadRequest("File is empty");
        
        return Ok(result);
    }
    
    [HttpGet("download/{id}")]
    public async Task<IActionResult> Download(string id)
    {
        try
        {
            var file = await _fileService.DownloadAsync(Guid.Parse(id));
            return File(file.OpenReadStream(), file.ContentType, file.FileName);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("File not found");
        }
    }
}