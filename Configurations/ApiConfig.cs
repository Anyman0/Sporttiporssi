using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Configurations
{
    public class ApiConfig
    {
        public static readonly string ApiBaseAddress = Preferences.Get("ApiBaseAddress", "default-value");
    }
}
