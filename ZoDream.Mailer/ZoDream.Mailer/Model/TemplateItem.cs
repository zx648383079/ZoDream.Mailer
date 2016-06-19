using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Mailer.Model
{
    public class TemplateItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsHtml { get; set; } = false;

        public string Content { get; set; }

        public List<string> Attachment { get; set; }


        public TemplateItem()
        {

        }

        public TemplateItem(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public TemplateItem(string title, string content, bool isHtml)
        {
            Title = title;
            Content = content;
            IsHtml = isHtml;
        }

        public string Make(string param)
        {
            var paramers = param.Split('|');
            var content = Content;
            for (int i = 0, length = paramers.Length; i < length; i++)
            {
                var index = $"{{${i + 1}}}";
                if (content.IndexOf(index) < 0)
                {
                    break;
                }
                content = content.Replace(index, paramers[i]);
            }

            return content;
        }
    }
}
