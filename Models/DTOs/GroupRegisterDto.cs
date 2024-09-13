using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Models.DTOs
{
    public class GroupRegisterDto
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Serie { get; set; }
        public FantasyGroup FantasyGroup { get; set; }
    }
}
