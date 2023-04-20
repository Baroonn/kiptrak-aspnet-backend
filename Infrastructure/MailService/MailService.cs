//using System.Net.Mail;
using Domain.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Service.ServiceContracts;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System.Diagnostics;

namespace Infrastructure.MailService;

public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;

    public MailService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }

    public async System.Threading.Tasks.Task SendEmailAsync(MailRequest mailRequest)
    {
        await MightSendEmail(mailRequest);
    }

    private async System.Threading.Tasks.Task MightSendEmail(MailRequest mailRequest)
    {
        Configuration.Default.ApiKey.TryAdd("api-key", "");

        var apiInstance = new TransactionalEmailsApi();
        string SenderName = _mailSettings.DisplayName;
        string SenderEmail = _mailSettings.Mail;
        SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
        string ToEmail = mailRequest.ToEmail;
        string ToName = "John Doe";
        SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
        List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
        To.Add(smtpEmailTo);
        string HtmlContent = "<html><body><h1>Please provide this token:  {{params.token}}</h1></body></html>";
        string? TextContent = null;
        string Subject = "KipTrak Email Verification";
        string ReplyToName = "KipTrak";
        string ReplyToEmail = "replyto@domain.com";
        SendSmtpEmailReplyTo ReplyTo = new SendSmtpEmailReplyTo(ReplyToEmail, ReplyToName);
        string? AttachmentUrl = null;
        string stringInBase64 = "aGVsbG8gdGhpcyBpcyB0ZXN0";
        byte[] Content = System.Convert.FromBase64String(stringInBase64);
        string AttachmentName = "test.txt";
        SendSmtpEmailAttachment AttachmentContent = new SendSmtpEmailAttachment(AttachmentUrl, Content, AttachmentName);
        List<SendSmtpEmailAttachment> Attachment = new List<SendSmtpEmailAttachment>();
        Attachment.Add(AttachmentContent);
        JObject Headers = new JObject();
        Headers.Add("Some-Custom-Name", "unique-id-1234");
        long? TemplateId = null;
        JObject Params = new JObject();
        Params.Add("token", mailRequest.Body);
        List<string> Tags = new List<string>();
        Tags.Add("mytag");
        SendSmtpEmailTo1 smtpEmailTo1 = new SendSmtpEmailTo1(ToEmail, ToName);
        List<SendSmtpEmailTo1> To1 = new List<SendSmtpEmailTo1>();
        To1.Add(smtpEmailTo1);
        Dictionary<string, object> _parmas = new Dictionary<string, object>();
        _parmas.Add("params", Params);
        SendSmtpEmailReplyTo1 ReplyTo1 = new SendSmtpEmailReplyTo1(ReplyToEmail, ReplyToName);
        SendSmtpEmailMessageVersions messageVersion = new SendSmtpEmailMessageVersions(To1, _parmas, null, null, ReplyTo1, Subject);
        List<SendSmtpEmailMessageVersions> messageVersiopns = new List<SendSmtpEmailMessageVersions>();
        messageVersiopns.Add(messageVersion);
        try
        {
            var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, HtmlContent, TextContent, Subject, ReplyTo, Attachment, Headers, TemplateId, Params, messageVersiopns, Tags);
            CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
            Debug.WriteLine(result.ToJson());
            Console.WriteLine(result.ToJson());
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            Console.WriteLine(e.Message);
        }
    }
}