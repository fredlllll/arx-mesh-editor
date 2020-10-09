using Assets.Scripts.ArxNative.IO;
using Assets.Scripts.ArxNative.IO.FTS;
using System.Collections.Generic;

namespace Assets.Scripts.ArxLevel.Mesh
{
    public class TexturePathLibrary
    {
        //private string[] textureArxPaths;
        //private readonly Dictionary<int, int> textureContainerToIndex = new Dictionary<int, int>();
        private int maxKey = int.MinValue;
        private readonly Dictionary<int, string> texturePaths = new Dictionary<int, string>();
        private readonly FTS_IO fts;

        public TexturePathLibrary(FTS_IO fts)
        {
            this.fts = fts;
        }

        public void LoadTexturePaths()
        {
            for (int i = 0; i < fts.textureContainers.Length; i++)
            {
                var tc = fts.textureContainers[i];
                texturePaths[tc.tc] = ArxIOHelper.GetString(tc.fic);
                if (tc.tc > maxKey)
                {
                    maxKey = tc.tc;
                }
            }
        }

        public void SaveTexturePaths()
        {
            fts.textureContainers = new FTS_IO_TEXTURE_CONTAINER[texturePaths.Count];
            int i = 0;
            foreach (var kv in texturePaths)
            {
                var tc = new FTS_IO_TEXTURE_CONTAINER();
                tc.tc = kv.Key;
                tc.fic = ArxIOHelper.GetBytes(kv.Value, 256);
                tc.temp = 0;//tc.temp isnt used ingame so we dont have to set it to a meaningful value
                fts.textureContainers[i] = tc;
                i++;
            }
        }

        public bool TryGetTexturePath(int textureContainer, out string texturePath)
        {
            return texturePaths.TryGetValue(textureContainer, out texturePath);
        }

        public int AddTexturePath(string texturePath)
        {
            int index = maxKey++;

            texturePaths[index] = texturePath;

            return index;
        }

        public void SetTexturePath(int textureContainer, string texturePath)
        {
            texturePaths[textureContainer] = texturePath;
        }

        public void RemoveTexturePath(int textureContainer)
        {
            texturePaths.Remove(textureContainer);
        }
    }
}
