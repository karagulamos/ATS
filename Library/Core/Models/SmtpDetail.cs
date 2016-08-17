using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Models
{
    public partial class SmtpDetail
    {
        [Key]
        public int SmtpDetailId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public int UseSSL { get; set; }
        public int OutboundPort { get; set; }
        public string OutboundHost { get; set; }
    }
}
