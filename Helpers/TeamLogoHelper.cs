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
            { "Sport", "sport.webp" },
            { "Kärpät", "karpat.webp" },
            { "KalPa", "kalpa.webp" },
            { "SaiPa", "saipa.webp" },
            { "KooKoo", "kookoo.webp" },
            { "JYP", "jyp.webp" },
            { "HIFK", "hifk.webp" },
            { "Jukurit", "jukurit.webp" },
            { "HPK", "hpk.webp" },
            { "Lukko", "lukko.webp" },
            { "Tappara", "tappara.webp" },
            { "Ilves", "ilves.webp" },
            { "TPS", "tps.webp" },
            { "Ässät", "assat.webp" },
            { "Pelicans", "pelicans.webp" },
            { "K-Espoo", "kespoo.webp" }
        };
        public static string GetTeamLogo(String teamName)
        {
            return TeamLogos.TryGetValue(teamName, out var logo) ? logo: "liigalogo.jpg";
        }
    }
}
