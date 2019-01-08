using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Architecture.Common.Options;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Razor;
using FluentEmail.SendGrid;
using Microsoft.Extensions.Options;

namespace Architecture.Common.Services.Email
{
    public class EmailProviderService : IEmailProviderService
    {
        private readonly SendGridOptions _sendGridOptions;

        public EmailProviderService(IOptions<SendGridOptions> sendGridOptions)
        {
            _sendGridOptions = sendGridOptions.Value;
            FluentEmail.Core.Email.DefaultSender = new SendGridSender(_sendGridOptions.ApiKey);
            FluentEmail.Core.Email.DefaultRenderer = new RazorRenderer();
        }

        public async Task<SendResponse> SendAsync(string from, string to, string subject, string template, dynamic templateParams)
        {
            return await SendAsync(from, to, null, null, subject, template, templateParams);
        }

        public async Task<SendResponse> SendAsync(string from, string to, string subject, string template, bool isHtml = false)
        {
            return await SendAsync(from, to, null, null, subject, template, isHtml);
        }

        public async Task<SendResponse> SendAsync(string from, string to, IList<string> cc, string subject, string template, dynamic templateParams)
        {
            return await SendAsync(from, to, cc, null, subject, template, templateParams);
        }

        public async Task<SendResponse> SendAsync(string from, string to, IList<string> cc, string subject, string template, bool isHtml = false)
        {
            return await SendAsync(from, to, cc, null, subject, template, isHtml);
        }

        public async Task<SendResponse> SendAsync(string from, string to, IList<string> cc, IList<string> bcc, string subject, string template, dynamic templateParams)
        {
            var sendEmail = FluentEmail.Core.Email.From(from)
                .To(to)
                .Subject(subject)
                .UsingTemplate(template, templateParams);

            if (cc != null)
            {
                var ccList = cc.Select(x => new Address { EmailAddress = x }).ToList();
                sendEmail = sendEmail.CC(ccList);
            }
            if (bcc != null)
            {
                var bccList = bcc.Select(x => new Address { EmailAddress = x }).ToList();
                sendEmail = sendEmail.BCC(bccList);
            }

            return await sendEmail.SendAsync();
        }

        public async Task<SendResponse> SendAsync(string from, string to, IList<string> cc, IList<string> bcc, string subject, string template, bool isHtml = false)
        {
            var sendEmail = FluentEmail.Core.Email.From(from)
                .To(to)
                .Subject(subject)
                .Body(template, isHtml);

            if (cc != null)
            {
                var ccList = cc.Select(x => new Address { EmailAddress = x }).ToList();
                sendEmail = sendEmail.CC(ccList);
            }
            if (bcc != null)
            {
                var bccList = bcc.Select(x => new Address { EmailAddress = x }).ToList();
                sendEmail = sendEmail.BCC(bccList);
            }

            return await sendEmail.SendAsync();
        }
    }
}
