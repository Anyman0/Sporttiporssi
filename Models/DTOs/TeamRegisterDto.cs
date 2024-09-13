using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Models.DTOs
{
    public class TeamRegisterDto
    {
        public string Email { get; set; }
        public string Serie { get; set; }
        public string TeamName { get; set; }
        public int TradesPerPhase { get; set; }       
        public int FundsLeft { get; set; }       
    }
}
