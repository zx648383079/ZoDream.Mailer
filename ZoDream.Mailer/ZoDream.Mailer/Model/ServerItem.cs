using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Mailer.Model
{
    public class ServerItem
    {
        public Int64 Id { get; set; }

        public string Server { get; set; }

        public int Port { get; set; } = 25;

        public ServerKind Kind { get; set; } = ServerKind.SMTP;

        public string User { get; set; }

        public bool IsSSL { get; set; } = false;

        public string Password { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public ServerItem()
        {

        }

        public ServerItem(string server, string user, string password)
        {
            Server = server;
            Email = Name = User = user;
            Password = password;
        }

        public ServerItem(string server, int port, string user, string password)
        {
            Server = server;
            Port = port;
            Email = Name = User = user;
            Password = password;
        }

        public ServerItem(string server, int port, string user, string password, string email, string name)
        {
            Server = server;
            Port = port;
            Email = email;
            Name = name;
            User = user;
            Password = password;
        }

        public ServerItem(string server, int port, ServerKind kind, string user, string password, string email, string name)
        {
            Server = server;
            Kind = kind;
            Port = port;
            Email = email;
            Name = name;
            User = user;
            Password = password;
        }

        public ServerItem(string server, int port, bool isSSL, string user, string password, string email, string name)
        {
            Server = server;
            if (server.IndexOf("smtp") >= 0)
            {
                Kind = ServerKind.SMTP;
            }else if(server.IndexOf("pop") >= 0) {
                Kind = ServerKind.POP3;
            } else {
                Kind = ServerKind.IMAP;
            }
            Port = port;
            IsSSL = isSSL;
            Email = email;
            Name = name;
            User = user;
            Password = password;
        }

        public ServerItem(string server, int port, bool isSSL, ServerKind kind, string user, string password, string email, string name)
        {
            Server = server;
            Kind = kind;
            Port = port;
            IsSSL = isSSL;
            Email = email;
            Name = name;
            User = user;
            Password = password;
        }
    }
}
