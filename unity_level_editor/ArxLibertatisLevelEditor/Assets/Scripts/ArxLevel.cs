using Assets.Scripts.Data;
using Assets.Scripts.DataSync;
using Assets.Scripts.Shared_IO;
using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class ArxLevel
    {
        private DLF_IO.DLF_IO dlf;
        public DLF_IO.DLF_IO DLF
        {
            get { return dlf; }
        }

        public Vector3 EditCameraPos
        {
            get { return dlf.header.positionEdit.ToVector3(); }
            set { dlf.header.positionEdit = new SavedVec3(value); }
        }
        public Vector3 EditCameraEuler
        {
            get { return dlf.header.angleEdit.ToEuler(); }
            set { dlf.header.angleEdit = new SavedAnglef(value); }
        }
        public Vector3 LevelOffset
        {
            get { return dlf.header.offset.ToVector3(); }
            set
            {
                dlf.header.offset = new SavedVec3(value);
                LevelObject.transform.localPosition = value;
            }
        }

        public string SceneName
        {
            get { return ArxIOHelper.GetString(dlf.scenes[0].name); }
            set { dlf.scenes[0].name = ArxIOHelper.GetBytes(value, 512); }
        }

        private LLF_IO.LLF_IO llf;
        public LLF_IO.LLF_IO LLF
        {
            get { return llf; }
        }
        private FTS_IO.FTS_IO fts;
        public FTS_IO.FTS_IO FTS
        {
            get { return fts; }
        }

        string name;

        public string Name { get { return name; } }

        public GameObject LevelObject { get; private set; }
        GameObject intersObject;
        GameObject lightsObject;
        GameObject fogsObject;
        GameObject pathsObject;

        ArxLevelMesh levelMesh;

        void LoadFiles()
        {
            var dlfPath = Path.Combine(ArxDirs.DLFDir, name, name + ".dlf");
            var llfPath = Path.Combine(ArxDirs.LLFDir, name, name + ".llf");
            var ftsPath = Path.Combine(ArxDirs.FTSDir, name, "fast.fts");

            //DEBUG: use unpacked versions of files for now
            dlfPath += ".unpacked";
            llfPath += ".unpacked";
            ftsPath += ".unpacked";

            dlf = new DLF_IO.DLF_IO();
            using (FileStream fs = new FileStream(dlfPath, FileMode.Open, FileAccess.Read))
            {
                dlf.LoadFrom(fs);
            }

            llf = new LLF_IO.LLF_IO();
            using (FileStream fs = new FileStream(llfPath, FileMode.Open, FileAccess.Read))
            {
                llf.LoadFrom(fs);
            }

            fts = new FTS_IO.FTS_IO();
            using (FileStream fs = new FileStream(ftsPath, FileMode.Open, FileAccess.Read))
            {
                fts.LoadFrom(fs);
            }
        }

        void ProcessDLF()
        {
            CreateInters();
            CreateLightingDLF();
            CreateLightsDLF();
            CreateFogs();
            CreatePaths();
        }

        void ProcessLLF()
        {

        }

        void ProcessFTS()
        {
            CreateMesh();
        }

        void CreateInters()
        {
            for (int i = 0; i < dlf.inters.Length; i++)
            {
                var interObject = new GameObject();
                interObject.transform.SetParent(intersObject.transform);

                var inter = interObject.AddComponent<Inter>();
                inter.LoadFrom(this, i);
            }
        }

        void CreateLightingDLF()
        {
            //because lighting has been moved to LLF this will most likely never be used
            if (dlf.header.lighting == 0) { return; }

            Debug.Log("if you see this, lighting is present in DLF, write code for it!");
        }

        void CreateLightsDLF()
        {
            for (int i = 0; i < dlf.lights.Length; i++)
            {
                var light = dlf.lights[i];
                var lightObject = new GameObject("light " + i);
                lightObject.transform.SetParent(lightsObject.transform);

                lightObject.transform.localPosition = light.pos.ToVector3();

                var l = lightObject.AddComponent<Light>();
                l.color = light.rgb.ToColor();

                l.type = LightType.Point;
                l.intensity = light.intensity;

                l.range = light.fallEnd;
                //TODO: add script to sync light settings etc
            }
        }

        void CreateFogs()
        {
            for (int i = 0; i < dlf.fogs.Length; i++)
            {
                //TODO: fogs are apparently points where fog particles are spawned in an interval
                //fog extent depends on type. if its directional, no extent, otherwise extent 100
            }
        }

        void CreatePaths()
        {
            for (int i = 0; i < dlf.paths.Length; i++)
            {
                var path = dlf.paths[i];
                var pathObject = new GameObject(ArxIOHelper.GetString(path.header.name));
                pathObject.transform.SetParent(pathsObject.transform);

                pathObject.transform.position = path.header.pos.ToVector3(); //no idea if that is the right pos, or what init_pos is
                //TODO: load paths etc and add sync script...
            }
        }

        void CreateLightsLLF()
        {
            Debug.Log("numLights: " + llf.lights.Length);
        }

        void CreateLighting()
        {
            Debug.Log("numLightings: " + llf.lightColors.Length);
        }

        void CreateLights(DANAE_IO_LIGHT[] lights)
        {
            //TODO: when syncing lights, we dont know if to sync with llf or dlf...
            for (int i = 0; i < lights.Length; i++)
            {
                var light = lights[i];
                var lightObject = new GameObject("light " + i);
                lightObject.transform.SetParent(lightsObject.transform);

                lightObject.transform.localPosition = light.pos.ToVector3();

                var l = lightObject.AddComponent<Light>();
                l.color = light.rgb.ToColor();

                l.type = LightType.Point;
                l.intensity = light.intensity;

                l.range = light.fallEnd;
                //TODO: add script to sync light settings etc
            }
        }

        void CreateMesh()
        {
            levelMesh = new ArxLevelMesh(this);
            levelMesh.CreateMesh(fts, llf);
        }

        public void Load(string name)
        {
            this.name = name;

            LevelObject = new GameObject(name);

            intersObject = new GameObject(name + "_inters");
            intersObject.transform.SetParent(LevelObject.transform);
            lightsObject = new GameObject(name + "_lights");
            lightsObject.transform.SetParent(LevelObject.transform);
            fogsObject = new GameObject(name + "_fogs");
            fogsObject.transform.SetParent(LevelObject.transform);
            pathsObject = new GameObject(name + "_paths");
            pathsObject.transform.SetParent(LevelObject.transform);

            LoadFiles();

            LevelObject.transform.localPosition = dlf.header.offset.ToVector3();

            ProcessDLF();
            ProcessLLF();
            ProcessFTS();

            Vector3 sceneOffset = fts.sceneHeader.Mscenepos.ToVector3();
            //edit cam pos = sceneOffset + dlf.header.positionEdit

            LevelObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f); //1 unit is 1 cm in arx, so scale down so one unit is one meter (at least perceived)
        }
    }
}
