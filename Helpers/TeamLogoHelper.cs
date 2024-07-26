using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Helpers
{
    public static class TeamLogoHelper
    {
        public static readonly Dictionary<string, string> TeamLogos = new Dictionary<string, string>
        {
            { "Sport", "sport_logo.jpg" },
            { "Kärpät", "karppa_logo.png" }
        };
        public static string GetTeamLogo(String teamName)
        {
            return TeamLogos.TryGetValue(teamName, out var logo) ? logo: "liigalogo.jpg";
        }
    }
}
