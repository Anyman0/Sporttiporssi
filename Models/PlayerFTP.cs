using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Models
{
    public class PlayerFTP
    {
        public Guid PlayerFTPId { get; set; }
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public string Serie { get; set; }
        public string PlayerName { get; set; }
        public int FTP { get; set; }
        public DateTime GameDate { get; set; }
    }
}
