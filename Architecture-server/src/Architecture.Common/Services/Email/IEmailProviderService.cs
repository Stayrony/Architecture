using FluentEmail.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Architecture.Common.Services.Email
{
    public interface IEmailProviderService
    {
        Task<SendResponse> SendAsync(string from, string to, string subject, string template, dynamic templateParams);
        Task<SendResponse> SendAsync(string from, string to, string subject, string template, bool isHtml = false);
        Task<SendResponse> SendAsync(string from, string to, IList<string> cc, string subject, string template, dynamic templateParams);
        Task<SendResponse> SendAsync(string from, string to, IList<string> cc, string subject, string template, bool isHtml = false);
        Task<SendResponse> SendAsync(string from, string to, IList<string> cc, IList<string> bcc, string subject, string template, dynamic templateParams);
        Task<SendResponse> SendAsync(string from, string to, IList<string> cc, IList<string> bcc, string subject, string template, bool isHtml = false);
    }
}
