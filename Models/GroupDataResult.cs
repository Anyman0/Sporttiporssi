using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Models
{
    public class GroupDataResult
    {
        public string GroupName { get; set; }
        public int? Standing { get; set; }
        public int TeamsInGroup { get; set; }
        public Guid FantasyTeamId { get; set; }
        public int TotalFTP { get; set; }
        public int TradesPerPhase { get; set; }
    }
}
