using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
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

        public static Task<DialogResult> OpenDialog()
        {
            return new Task<DialogResult>(() =>
            {
                var ofn = new OpenFileName();
                ofn.lStructSize = Marshal.SizeOf(ofn);
                ofn.lpstrFilter = Filter + "\0\0";

                var strFile = new char[1024];
                Array.Copy(FileName.ToCharArray(), strFile, FileName.Length);

                ofn.lpstrFile = new string(strFile);
                ofn.nMaxFile = ofn.lpstrFile.Length;
                ofn.lpstrFileTitle = new string(new char[256]);
                ofn.nMaxFileTitle = ofn.lpstrFileTitle.Length;
                ofn.lpstrTitle = Title;
                if (GetOpenFileName(ofn))
                {
                    FileName = ofn.lpstrFile;
                    return DialogResult.OK;
                }

                return DialogResult.Cancel;
            });
        }
    }
}
