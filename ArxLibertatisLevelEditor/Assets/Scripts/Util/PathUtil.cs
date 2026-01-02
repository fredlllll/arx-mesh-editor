using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class PathUtil
    {
        public static void SetCurrentWorkingDirectoryToProjectRoot()
        {
            var projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            Directory.SetCurrentDirectory(projectRoot);
        }
    }
}
