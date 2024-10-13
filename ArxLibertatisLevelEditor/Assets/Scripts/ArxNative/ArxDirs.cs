using Assets.Scripts.ArxLevelEditor;
using System.IO;

namespace Assets.Scripts.ArxNative
{
    public static class ArxDirs
    {

        public static string DLFDir
        {
            get
            {
                return Path.Combine(ArxLibertatisEditorIO.ArxPaths.DataDir, "graph", "levels");
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
                return Path.Combine(ArxLibertatisEditorIO.ArxPaths.DataDir, "game", "graph", "levels");
            }
        }
    }
}
