using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerSide.Models
{
    public class Sender
    {
        public string emailAddress { get; set; }
    }

    public class Recipient
    {
        public string emailAddress { get; set; }
        public string recipientType { get; set; }
    }

    public class EmailAPIModel
    {
        public string subject { get; set; }
        public string content { get; set; }
        public string contentType { get; set; }
        public Sender sender { get; set; }
        public List<Recipient> recipients { get; set; }
    }

    public class EmailModel
    {
        public int UserId { get; set; }
        public string subject { get; set; }
        public string content { get; set; }
        public string sender { get; set; }
        public string RecipientsTO { get; set; }
        public string RecipientsCC { get; set; }
        public string RecipientsBCC { get; set; }
        public List<HttpPostedFileBase> Files { get; set; }
    }
}