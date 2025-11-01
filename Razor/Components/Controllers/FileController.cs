using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Razor.Components.Services;

[Authorize] 
[ApiController]
[Route("api/files")]
public class FileController : ControllerBase
{
    private readonly RequestFileService _requestFileService;

    public FileController(RequestFileService requestFileService)
    {
        _requestFileService = requestFileService;
    }

    [HttpGet("download")]
    public IActionResult DownloadFile([FromQuery] string requestedFile, [FromQuery] string filePath)
    {
        try
        {
            var stream = _requestFileService.GetStream(requestedFile, filePath);
            var fileName = Path.GetFileName(requestedFile.Replace("|", "\\"));

            if (requestedFile.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                return File(stream, "application/pdf"); 
            }

            return File(stream, "application/octet-stream", fileName);
        }
        catch (FileNotFoundException)
        {
            return NotFound("File not found on server.");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
