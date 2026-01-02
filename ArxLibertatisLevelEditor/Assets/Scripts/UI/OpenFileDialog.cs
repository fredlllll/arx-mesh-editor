using SFB;
using System;
using System.Threading.Tasks;

namespace Assets.Scripts.UI
{
    public static class OpenFileDialog
    {
        public enum DialogResult
        {
            OK,
            Cancel
        }

        public static string Title { get; set; } = "Open File Dialog";

        public static ExtensionFilter[] Filters { get; set; } = Array.Empty<ExtensionFilter>();

        public static string FileName { get; set; } = "";

        public static Task<DialogResult> OpenDialogAsync()
        {
            return new Task<DialogResult>(() =>
            {
                var paths = StandaloneFileBrowser.OpenFilePanel("Open Texture", FileName, Filters, false);
                if (paths.Length > 0)
                {
                    FileName = paths[0];
                    return DialogResult.OK;
                }
                return DialogResult.Cancel;
            });
        }
    }
}
