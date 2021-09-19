namespace Assets.Scripts.ArxLevelEditor
{
    public static class EditorSettings
    {
        public static string DataDir;
        public static string FtsDir;
        public static string DlfLlfDir;

        static EditorSettings() {
            // DataDir = @"F:\Program Files\Arx Libertatis\";
            // FtsDir = DataDir + @"paks\game\graph\levels";
            // DlfLlfDir = DataDir + @"paks\graph\levels";

            DataDir = @"C:\Program Files\Arx Libertatis\";
            FtsDir = DataDir + @"game\graph\levels";
            DlfLlfDir = DataDir + @"graph\levels";

            // TODO: load stuff from ArxLibertatisLevelEditor/config.json
        }
    }
}
