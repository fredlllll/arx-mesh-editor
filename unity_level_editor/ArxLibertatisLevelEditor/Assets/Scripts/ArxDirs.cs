using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class ArxDirs
    {
        static string dataDir = @"F:\Program Files\Arx Libertatis\paks\"; //for testing

        public static string DataDir
        {
            get { return dataDir; }
            set
            {
                dataDir = value;
            }
        }

        public static string DLFDir
        {
            get
            {
                return Path.Combine(dataDir, "graph", "levels");
            }
        }

        public static string LLFDir
        {
            get
            {
                return DLFDir; //same as dlf
            }
        }

        public static string FTSDir
        {
            get
            {
                return Path.Combine(dataDir, "game", "graph", "levels");
            }
        }
    }
}
