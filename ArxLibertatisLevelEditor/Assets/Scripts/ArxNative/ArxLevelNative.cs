using Assets.Scripts.ArxNative.IO.DLF;
using Assets.Scripts.ArxNative.IO.FTS;
using Assets.Scripts.ArxNative.IO.LLF;
using System.IO;

namespace Assets.Scripts.ArxNative
{
    public class ArxLevelNative
    {
        private DLF_IO dlf;
        public DLF_IO DLF
        {
            get { return dlf; }
        }
        private LLF_IO llf;
        public LLF_IO LLF
        {
            get { return llf; }
        }
        private FTS_IO fts;
        public FTS_IO FTS
        {
            get { return fts; }
        }

        public string LastLoadName
        {
            get;
            private set;
        }

        public string LastSaveName
        {
            get;
            private set;
        }

        public void LoadLevel(string name)
        {
            LastLoadName = name;

            dlf = new DLF_IO();
            using (FileStream fs = new FileStream(ArxPaths.GetDlfPath(name), FileMode.Open, FileAccess.Read))
            {
                dlf.LoadFrom(DLF_IO.EnsureUnpacked(fs));
            }

            llf = new LLF_IO();
            using (FileStream fs = new FileStream(ArxPaths.GetLlfPath(name), FileMode.Open, FileAccess.Read))
            {
                llf.LoadFrom(LLF_IO.EnsureUnpacked(fs));
            }

            fts = new FTS_IO();
            using (FileStream fs = new FileStream(ArxPaths.GetFtsPath(name), FileMode.Open, FileAccess.Read))
            {
                fts.LoadFrom(FTS_IO.EnsureUnpacked(fs));
            }
        }

        public void SaveLevel(string name)
        {
            LastSaveName = name;

            using (MemoryStream ms = new MemoryStream())
            {
                dlf.WriteTo(ms);
                using (var packedStream = DLF_IO.EnsurePacked(ms))
                using (FileStream fs = new FileStream(ArxPaths.GetDlfPath(name), FileMode.Create, FileAccess.Write))
                {
                    packedStream.CopyTo(fs);
                }
            }

            using (MemoryStream ms = new MemoryStream())
            {
                llf.WriteTo(ms);
                using (var packedStream = LLF_IO.EnsurePacked(ms))
                using (FileStream fs = new FileStream(ArxPaths.GetLlfPath(name), FileMode.Create, FileAccess.Write))
                {
                    packedStream.CopyTo(fs);
                }
            }

            using (MemoryStream ms = new MemoryStream())
            {
                fts.WriteTo(ms);
                using (var packedStream = FTS_IO.EnsurePacked(ms))
                using (FileStream fs = new FileStream(ArxPaths.GetFtsPath(name), FileMode.Create, FileAccess.Write))
                {
                    packedStream.CopyTo(fs);
                }
            }
        }
    }
}
