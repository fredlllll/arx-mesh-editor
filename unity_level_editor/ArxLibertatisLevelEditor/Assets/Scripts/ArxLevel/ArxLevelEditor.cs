using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.ArxLevel
{
    public class ArxLevelEditor : MonoBehaviour
    {
        public ArxLevel CurrentLevel { get; private set; }

        public void OpenLevel(string name)
        {
            if(CurrentLevel != null)
            {
                Destroy(CurrentLevel.LevelObject);
            }

            CurrentLevel = new ArxLevel();
            CurrentLevel.Load(name);
        }
        public void UnpackFiles()
        {
            var ftsDir = new DirectoryInfo(@"F:\Program Files\Arx Libertatis\paks\game\graph\levels");
            var dlfLlfDir = new DirectoryInfo(@"F:\Program Files\Arx Libertatis\paks\graph\levels");

            var levels = new int[]
            {
            0,1,2,3,4,5,6,7,8,10,11,12,13,14,15,16,17,18,19,20,21,22,23
            };

            foreach (var l in levels)
            {
                var fts = Path.Combine(ftsDir.FullName, "level" + l, "fast.fts");
                var dlf = Path.Combine(dlfLlfDir.FullName, "level" + l, "level" + l + ".dlf");
                var llf = Path.Combine(dlfLlfDir.FullName, "level" + l, "level" + l + ".llf");

                //fts
                using (var unpacked = FTS_IO.FTS_IO.EnsureUnpacked(new FileStream(fts, FileMode.Open, FileAccess.Read)))
                {
                    using (var outFs = new FileStream(fts + ".unpacked", FileMode.Create, FileAccess.Write))
                    {
                        unpacked.CopyTo(outFs);
                    }
                }

                using (var unpacked = DLF_IO.DLF_IO.EnsureUnpacked(new FileStream(dlf, FileMode.Open, FileAccess.Read)))
                {
                    using (var outFs = new FileStream(dlf + ".unpacked", FileMode.Create, FileAccess.Write))
                    {
                        unpacked.CopyTo(outFs);
                    }
                }

                using (var unpacked = LLF_IO.LLF_IO.EnsureUnpacked(new FileStream(llf, FileMode.Open, FileAccess.Read)))
                {
                    using (var outFs = new FileStream(llf + ".unpacked", FileMode.Create, FileAccess.Write))
                    {
                        unpacked.CopyTo(outFs);
                    }
                }
            }
        }
    }
}