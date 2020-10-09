using System.IO;

namespace Assets.Scripts.ArxNative
{
    public static class ArxPaths
    {
        public static string GetDlfPath(string levelname)
        {
            return Path.Combine(ArxDirs.DLFDir, levelname, levelname + ".dlf");
        }

        public static string GetLlfPath(string levelname)
        {
            return Path.Combine(ArxDirs.LLFDir, levelname, levelname + ".llf");
        }

        public static string GetFtsPath(string levelname)
        {
            return Path.Combine(ArxDirs.FTSDir, levelname, "fast.fts");
        }
    }
}
