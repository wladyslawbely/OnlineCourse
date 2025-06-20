using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Service
{
    public interface ICertificateService
    {
        Task<byte[]> GenerateCertificatePdfAsync(int userId, int courseId);
        Task SendCertificateByEmailAsync(int userId, int courseId);
    }
}
