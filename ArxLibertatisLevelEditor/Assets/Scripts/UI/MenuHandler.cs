using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxNative.IO;
using Assets.Scripts.ArxNative.IO.DLF;
using Assets.Scripts.ArxNative.IO.FTS;
using Assets.Scripts.ArxNative.IO.LLF;
using Assets.Scripts.ArxNative.IO.PK;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class MenuHandler : MonoBehaviour
    {
        public InputField levelName;

        public void OpenMenuClicked()
        {
            LevelEditor.OpenLevel(levelName.text);
            gameObject.SetActive(false);
        }

        public void TestImplode()
        {
            System.Random r = new System.Random();
            byte[] bytes = new byte[512];
            r.NextBytes(bytes);

            var imploded = Implode.DoImplode(bytes);
            var exploded = ArxIO.Unpack(imploded);

            compare("imploded exploded", bytes, exploded);

            Debug.Log("test done");
        }

        void compare(string name, byte[] bytesIn, byte[] bytesOut)
        {
            if (bytesIn.Length != bytesOut.Length)
            {
                Debug.Log(name + " in: " + bytesIn.Length + " " + name + " out: " + bytesOut.Length);
            }
            else
            {
                for (int i = 0; i < bytesIn.Length; i++)
                {
                    if (bytesIn[i] != bytesOut[i])
                    {
                        Debug.Log(name + " different at " + i);
                        break;
                    }
                }
            }
        }

        public void TestIOReadWrite()
        {
            DirectoryInfo ftsDir = new DirectoryInfo(@"F:\Program Files\Arx Libertatis\paks\game\graph\levels");
            DirectoryInfo dlfLlfDir = new DirectoryInfo(@"F:\Program Files\Arx Libertatis\paks\graph\levels");

            string name = levelName.text;

            var dlf = Path.Combine(dlfLlfDir.FullName, name, name + ".dlf");
            var llf = Path.Combine(dlfLlfDir.FullName, name, name + ".llf");
            var fts = Path.Combine(ftsDir.FullName, name, "fast.fts");

            //DEBUG: use unpacked versions of files for now
            dlf += ".unpacked";
            llf += ".unpacked";
            fts += ".unpacked";

            var DLF = new DLF_IO();
            byte[] dlfBytesIn;
            using (FileStream fs = new FileStream(dlf, FileMode.Open, FileAccess.Read))
            {
                dlfBytesIn = new byte[fs.Length];
                fs.Read(dlfBytesIn, 0, dlfBytesIn.Length);
                fs.Position = 0;
                DLF.LoadFrom(fs);
            }
            MemoryStream dlfOut = new MemoryStream();
            DLF.WriteTo(dlfOut);
            byte[] dlfBytesOut = dlfOut.ToArray();
            using(FileStream fs = new FileStream(dlf+".out",FileMode.Create, FileAccess.Write))
            {
                dlfOut.Position = 0;
                dlfOut.CopyTo(fs);
            }

            var LLF = new LLF_IO();
            byte[] llfBytesIn;
            using (FileStream fs = new FileStream(llf, FileMode.Open, FileAccess.Read))
            {
                llfBytesIn = new byte[fs.Length];
                fs.Read(llfBytesIn, 0, llfBytesIn.Length);
                fs.Position = 0;
                LLF.LoadFrom(fs);
            }
            MemoryStream llfOut = new MemoryStream();
            LLF.WriteTo(llfOut);
            byte[] llfBytesOut = llfOut.ToArray();

            var FTS = new FTS_IO();
            byte[] ftsBytesIn;
            using (FileStream fs = new FileStream(fts, FileMode.Open, FileAccess.Read))
            {
                ftsBytesIn = new byte[fs.Length];
                fs.Read(ftsBytesIn, 0, ftsBytesIn.Length);
                fs.Position = 0;
                FTS.LoadFrom(fs);
            }
            MemoryStream ftsOut = new MemoryStream();
            FTS.WriteTo(ftsOut);
            byte[] ftsBytesOut = ftsOut.ToArray();
            using (FileStream fs = new FileStream(fts + ".out", FileMode.Create, FileAccess.Write))
            {
                ftsOut.Position = 0;
                ftsOut.CopyTo(fs);
            }

            //compare results
            compare("dlf", dlfBytesIn, dlfBytesOut);
            compare("llf", llfBytesIn, llfBytesOut);
            compare("fts", ftsBytesIn, ftsBytesOut);
        }
    }
}
