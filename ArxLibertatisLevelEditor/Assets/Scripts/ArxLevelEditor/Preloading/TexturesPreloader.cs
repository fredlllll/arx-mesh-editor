namespace Assets.Scripts.ArxLevelEditor.Preloading
{
    /*public static class TexturesPreloader
    {
        public enum Status
        {
            Idle,
            Calculating,
            Loading,
            Done
        }

        public static Status LoaderStatus { get; private set; }
        public static int Loaded { get; private set; } = 0;
        public static int Total { get; private set; } = 0;

        public static void Reset()
        {
            LoaderStatus = Status.Idle;
            Loaded = 0;
            Total = 0;
        }

        public static IEnumerator PreloadTextures()
        {
            LoaderStatus = Status.Calculating;

            yield return null;

            var filesEnum = Directory.EnumerateFiles(EditorSettings.DataDir, "*.jpg", SearchOption.AllDirectories);
            filesEnum = filesEnum.Concat(Directory.EnumerateFiles(EditorSettings.DataDir, "*.bmp", SearchOption.AllDirectories));

            var files = filesEnum.ToArray();

            Total = files.Length;
            LoaderStatus = Status.Loading;

            var start = DateTime.Now;

            for (int i = 0; i < files.Length; i++)
            {
                var f = files[i];
                var path = f.Replace(EditorSettings.DataDir, "");
                LevelEditor.TextureDatabase.GetTexture(path);

                Loaded = i;

                if((DateTime.Now-start).TotalSeconds > 0.1)
                {
                    start = DateTime.Now;
                    yield return null;
                }
            }

            LoaderStatus = Status.Done;
        }
    }*/
}
