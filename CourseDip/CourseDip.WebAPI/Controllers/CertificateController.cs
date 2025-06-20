using CourseDip.Application.Interfaces.Service;
using CourseDip.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/certificates")]
public class CertificateController : ControllerBase
{
    private readonly ICertificateService _certificateService;

    public CertificateController(ICertificateService certificateService)
    {
        _certificateService = certificateService;
    }

    [HttpGet("{courseId}/download")]
    public async Task<IActionResult> Download(int courseId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var pdfBytes = await _certificateService.GenerateCertificatePdfAsync(userId, courseId);
        return File(pdfBytes, "application/pdf", "certificate.pdf");
    }

    [HttpPost("{courseId}/send")]
    public async Task<IActionResult> Send(int courseId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _certificateService.SendCertificateByEmailAsync(userId, courseId);
        return Ok(new { message = "Сертификат отправлен на почту" });
    }
}
