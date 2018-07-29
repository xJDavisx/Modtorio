using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Modtorio
{
    public class ModList
    {
        /// <summary>
        /// Initializes a class representation of the "mod-list.json" file in the mod directory.
        /// </summary>
        public ModList()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public ModListEntry[] mods { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public class ModListEntry
        {
            public string name { get; set; }
            public bool enabled { get; set; }
        }

    }
}
