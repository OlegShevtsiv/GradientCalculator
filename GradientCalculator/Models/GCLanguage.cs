using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GradientCalculator.Models
{
    public class GCLanguage
    {
        public string nativeLang { get; }

        public string publicLang { get; }

        public GCLanguage(string nat, string pub)
        {
            this.nativeLang = nat;
            this.publicLang = pub;

        }
    }
}
