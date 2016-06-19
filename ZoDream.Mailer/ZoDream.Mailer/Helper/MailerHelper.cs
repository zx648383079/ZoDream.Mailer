using Limilabs.Client.IMAP;
using Limilabs.Client.POP3;
using Limilabs.Client.SMTP;
using Limilabs.Mail;
using Limilabs.Mail.Fluent;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ZoDream.Mailer.Model;

namespace ZoDream.Mailer.Helper
{
    public class MailerHelper
    {
        /// <summary>
        /// IMAP
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static List<IMail> GetByImap(ServerItem item)
        {
            var mails = new List<IMail>();
            using (Imap imap = new Imap())
            {
                imap.Connect(item.Server, item.Port);  // or ConnectSSL for SSL
                imap.UseBestLogin(item.User, item.Password);

                imap.SelectInbox();
                List<long> uids = imap.Search(Flag.Unseen);
                
                foreach (long uid in uids)
                {
                    mails.Add(new MailBuilder()
                        .CreateFromEml(imap.GetMessageByUID(uid)));
                }
            }
            return mails;
        }

        /// <summary>
        /// IMAP
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static List<IMail> GetByPop3(ServerItem item)
        {
            var mails = new List<IMail>();
            using (Pop3 pop3 = new Pop3())
            {
                pop3.Connect(item.Server, item.Port);  // or ConnectSSL for SSL      
                pop3.UseBestLogin(item.User, item.Password);

                List<string> uids = pop3.GetAll();
                foreach (string uid in uids)
                {
                    mails.Add(new MailBuilder()
                        .CreateFromEml(pop3.GetMessageByUID(uid)));
                }
            }
            return mails;
        }

        public static bool Send(ServerItem item, string title, string body)
        {
            var email = Mail.Html(@"Html with an image: <img src=""cid:lena"" />")
            .AddVisual(@"c:\lena.jpeg").SetContentId("lena")
            .AddAttachment(@"c:\tmp.doc").SetFileName("document.doc")
            .To("to@example.com")
            .From("from@example.com")
            .Subject(title)
            .Create();
            return Send(item, email);
        }

        public static IMail MakeMail(EmailItem email, TemplateItem template, string from)
        {
            var body = template.Make(email.Value);
            IFluentMail mail;
            if (template.IsHtml)
            {
                mail = Mail.Html("");
                var matches = Regex.Matches(body, @"<img[^<>]+src=""?([^""\s<>]+)", RegexOptions.IgnoreCase);
                for (int i = 0, length = matches.Count; i < length; i++)
                {
                    var file = matches[i].Groups[1].Value;
                    if (file.IndexOf("//") >= 0 || !File.Exists(file))
                    {
                        continue;
                    }
                    body = body.Replace(file, "cid:img" + i);
                    mail.AddVisual(file).SetContentId("img" + i);
                }
                mail.Html(body);
            } else
            {
                mail = Mail.Text(body);
            }
            foreach (var file in template.Attachment)
            {
                mail.AddAttachment(file).SetFileName(Path.GetFileName(file));
            }
            return mail.To(email.Email)
                .From(from)
                .Subject(template.Title)
                .Create();
        }

        public static bool Send(ServerItem item, EmailItem email, TemplateItem template)
        {
            return Send(item, MakeMail(email, template, item.Email));
        }

        public static bool Send(ServerItem item, IMail email)
        {
            var result = true;
            using (Smtp smtp = new Smtp())
            {
                if (item.IsSSL)
                {
                    smtp.ConnectSSL(item.Server, item.Port);  // or ConnectSSL for SSL
                }
                else
                {
                    smtp.Connect(item.Server, item.Port);  // or ConnectSSL for SSL
                }
                
                smtp.UseBestLogin(item.User, item.Password);

                var message = smtp.SendMessage(email);
                result = message.Status == SendMessageStatus.Success;
            }
            return result;
        }
    }
}
