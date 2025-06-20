using CourseDip.Application.Interfaces.Repository;
using CourseDip.Application.Interfaces.Service;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class CertificateService : ICertificateService
{
    private readonly IUserRepository _userRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IConfiguration _configuration;

    public CertificateService(
        IUserRepository userRepository,
        ICourseRepository courseRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _courseRepository = courseRepository;
        _configuration = configuration;
    }

    public async Task<byte[]> GenerateCertificatePdfAsync(int userId, int courseId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        var course = await _courseRepository.GetByIdAsync(courseId);

        var userName = $"{user.FirstName} {user.LastName}";
        var courseName = course.Title;
        var date = DateTime.Now.ToString("dd.MM.yyyy");

        var pdf = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);

                page.Content().Border(2).Padding(40).Column(column =>
                {
                    column.Spacing(25);

                    column.Item().AlignCenter().Text("СЕРТИФИКАТ")
                        .FontSize(34)
                        .Bold()
                        .FontColor("#2E86C1");
                    column.Item().AlignCenter().Text($"ID сертификата: C{userId:D4}-{courseId:D4}")
                        .FontSize(10)
                        .FontColor(Colors.Grey.Medium);

                    column.Item().AlignCenter().Text("Настоящим удостоверяется, что")
                        .FontSize(18);

                    column.Item().AlignCenter().Text(userName)
                        .FontSize(26)
                        .Bold()
                        .Underline()
                        .FontColor("#1B2631");

                    column.Item().AlignCenter().Text($"успешно завершил(а) обучение по курсу")
                        .FontSize(18);

                    column.Item().AlignCenter().Text($"«{courseName}»")
                        .FontSize(20)
                        .Italic()
                        .FontColor("#2E4053");

                    column.Item().PaddingVertical(20).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                    column.Item().AlignRight().Text($"Дата: {date}")
                        .FontSize(14)
                        .FontColor(Colors.Grey.Darken2);

                    column.Item()
                             .Width(100)
                             .PaddingTop(10)
                             .AlignRight()
                             .Image("wwwroot/images/print.png");

                });
            });
        });

        using var stream = new MemoryStream();
        pdf.GeneratePdf(stream);
        return stream.ToArray();
    }


    public async Task SendCertificateByEmailAsync(int userId, int courseId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        var pdfBytes = await GenerateCertificatePdfAsync(userId, courseId);

        var emailConfig = _configuration.GetSection("EmailSettings");
        var fromEmail = emailConfig["From"];
        var password = emailConfig["Password"];
        var smtpHost = emailConfig["SmtpHost"];
        var smtpPort = int.Parse(emailConfig["SmtpPort"]);

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Course Platform", fromEmail));
        message.To.Add(MailboxAddress.Parse(user.Email));
        message.Subject = "Ваш сертификат о завершении курса";

        var builder = new BodyBuilder
        {
            HtmlBody = $"<p>Здравствуйте, {user.FirstName}!<br/>Во вложении — ваш сертификат о завершении курса.</p>"
        };

        builder.Attachments.Add("certificate.pdf", pdfBytes, ContentType.Parse("application/pdf"));
        message.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(fromEmail, password);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}
