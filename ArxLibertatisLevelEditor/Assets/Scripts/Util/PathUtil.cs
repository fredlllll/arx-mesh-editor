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

        // Helper compatible with .NET Framework 4.7.1
        public static string GetRelativePath(string basePath, string path)
        {
            if (string.IsNullOrEmpty(basePath)) return path;
            // Ensure directory separators and trailing slash on base
            var baseFull = Path.GetFullPath(basePath);
            if (!baseFull.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                baseFull += Path.DirectorySeparatorChar;
            }

            var pathFull = Path.GetFullPath(path);

            try
            {
                var baseUri = new Uri(baseFull);
                var pathUri = new Uri(pathFull);
                var relative = baseUri.MakeRelativeUri(pathUri).ToString();
                relative = Uri.UnescapeDataString(relative).Replace('/', Path.DirectorySeparatorChar);
                return relative;
            }
            catch
            {
                // Fallback: if URI conversion fails, attempt simple substring replacement (best-effort)
                if (pathFull.StartsWith(baseFull, StringComparison.OrdinalIgnoreCase))
                {
                    return pathFull.Substring(baseFull.Length);
                }
                return pathFull;
            }
        }
    }
}
