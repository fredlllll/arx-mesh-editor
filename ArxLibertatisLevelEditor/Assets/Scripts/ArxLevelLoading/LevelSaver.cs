using Assets.Scripts.ArxLevel;
using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxNative.IO.Shared_IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ArxLevelLoading
{
    public static class LevelSaver
    {
        public static void SaveLevel(Level level, string name)
        {
            var lvln = level.ArxLevelNative;

            lvln.DLF.header.positionEdit = new SavedVec3(LevelEditor.EditorCamera.transform.position);
            lvln.DLF.header.angleEdit = new SavedAnglef(LevelEditor.EditorCamera.transform.eulerAngles);
            lvln.DLF.header.offset = new SavedVec3(level.LevelOffset);

            SaveMesh(level);

            lvln.SaveLevel(name);
        }

        static void SaveMesh(Level level)
        {
            //TODO: update all info for mesh
        }
    }
}
