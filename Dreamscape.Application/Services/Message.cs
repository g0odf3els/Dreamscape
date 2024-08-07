﻿using MimeKit;

namespace Dreamscape.Application.Services;

public class Message
{
    public List<MailboxAddress> To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }

    public Message(IEnumerable<EmailAddress> to, string subject, string content)
    {
        To = new List<MailboxAddress>();

        To.AddRange(to.Select(x => new MailboxAddress(x.DisplayName, x.Address)));
        Subject = subject;
        Content = content;
    }
}

