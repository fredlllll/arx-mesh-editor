using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static Assets.Scripts.UI.Windows.ComDlg32;

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

        public static string Filter { get; set; } = "";

        public static string FileName { get; set; } = "";

        private static string GetString(char[] chars)
        {
            int strlen = chars.Length;
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == 0)
                {
                    strlen = i;
                    break;
                }
            }
            string retval = new string(chars, 0, strlen);
            return retval;
        }

        public static Task<DialogResult> OpenDialog()
        {
            return new Task<DialogResult>(() =>
            {
                var ofn = new OpenFileName();
                ofn.lStructSize = Marshal.SizeOf(ofn);
                ofn.lpstrFilter = Filter + "\0\0";
                ofn.Flags = 0x00001000 | 0x00000800 | 0x00000008;//OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST|OFN_NOCHANGEDIR

                var strFile = new char[1024];
                Array.Copy(FileName.ToCharArray(), strFile, FileName.Length);

                ofn.lpstrFile = new string(strFile);
                ofn.nMaxFile = ofn.lpstrFile.Length;
                ofn.lpstrFileTitle = new string(new char[256]);
                ofn.nMaxFileTitle = ofn.lpstrFileTitle.Length;
                ofn.lpstrTitle = Title;
                if (GetOpenFileName(ofn))
                {
                    FileName = GetString(ofn.lpstrFile.ToCharArray());
                    return DialogResult.OK;
                }

                return DialogResult.Cancel;
            });
        }
    }
}
