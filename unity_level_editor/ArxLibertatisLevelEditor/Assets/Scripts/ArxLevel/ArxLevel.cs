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

namespace Assets.Scripts.ArxLevel
{
    public class ArxLevel
    {
        public Vector3 EditCameraPos
        {
            get;
            set;
        }
        public Vector3 EditCameraEuler
        {
            get;
            set;
        }
        private Vector3 levelOffset;
        public Vector3 LevelOffset
        {
            get { return levelOffset; }
            set
            {
                levelOffset = value;
                LevelObject.transform.localPosition = value;
            }
        }

        /*public string SceneName
        {
            get { return ArxIOHelper.GetString(dlf.scenes[0].name); }
            set { dlf.scenes[0].name = ArxIOHelper.GetBytes(value, 512); }
        }*/

        public string Name
        {
            get;
            private set;
        }

        public ArxLevelNative ArxLevelNative
        {
            get;
            private set;
        }

        public GameObject LevelObject { get; private set; }
        GameObject intersObject;
        GameObject lightsObject;
        GameObject fogsObject;
        GameObject pathsObject;

        ArxLevelBigMesh levelBigMesh;
        ArxLevelAnchors levelAnchors;
        ArxLevelCellMesh levelCellMesh;

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
            for (int i = 0; i < ArxLevelNative.DLF.inters.Length; i++)
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
            if (ArxLevelNative.DLF.header.lighting == 0) { return; }

            Debug.Log("if you see this, lighting is present in DLF, write code for it!");
        }

        void CreateLightsDLF()
        {
            for (int i = 0; i < ArxLevelNative.DLF.lights.Length; i++)
            {
                var light = ArxLevelNative.DLF.lights[i];
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
            for (int i = 0; i < ArxLevelNative.DLF.fogs.Length; i++)
            {
                //TODO: fogs are apparently points where fog particles are spawned in an interval
                //fog extent depends on type. if its directional, no extent, otherwise extent 100
            }
        }

        void CreatePaths()
        {
            for (int i = 0; i < ArxLevelNative.DLF.paths.Length; i++)
            {
                var path = ArxLevelNative.DLF.paths[i];
                var pathObject = new GameObject(ArxIOHelper.GetString(path.header.name));
                pathObject.transform.SetParent(pathsObject.transform);

                pathObject.transform.position = path.header.pos.ToVector3(); //no idea if that is the right pos, or what init_pos is
                //TODO: load paths etc and add sync script...
            }
        }

        void CreateLightsLLF()
        {
            Debug.Log("numLights: " + ArxLevelNative.LLF.lights.Length);
        }

        void CreateLighting()
        {
            Debug.Log("numLightings: " + ArxLevelNative.LLF.lightColors.Length);
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
            levelBigMesh = new ArxLevelBigMesh(this);
            levelBigMesh.CreateMesh();

            levelAnchors = new ArxLevelAnchors(this);
            var anchors = levelAnchors.CreateAnchors();
            anchors.transform.SetParent(LevelObject.transform);
            /*levelCellMesh = new ArxLevelCellMesh(this);
            levelCellMesh.CreateMesh();*/
        }

        public void Load(string name)
        {
            ArxLevelNative = new ArxLevelNative();
            ArxLevelNative.LoadLevel(name);

            Name = name;

            LevelObject = new GameObject(name);

            intersObject = new GameObject(name + "_inters");
            intersObject.transform.SetParent(LevelObject.transform);
            lightsObject = new GameObject(name + "_lights");
            lightsObject.transform.SetParent(LevelObject.transform);
            fogsObject = new GameObject(name + "_fogs");
            fogsObject.transform.SetParent(LevelObject.transform);
            pathsObject = new GameObject(name + "_paths");
            pathsObject.transform.SetParent(LevelObject.transform);

            EditCameraPos = ArxLevelNative.DLF.header.positionEdit.ToVector3();
            EditCameraEuler = ArxLevelNative.DLF.header.angleEdit.ToEuler();
            LevelOffset = ArxLevelNative.DLF.header.offset.ToVector3();

            ProcessDLF();
            ProcessLLF();
            ProcessFTS();

            Vector3 sceneOffset = ArxLevelNative.FTS.sceneHeader.Mscenepos.ToVector3();
            intersObject.transform.localPosition = sceneOffset;

            LevelObject.transform.localScale = new Vector3(0.01f, -0.01f, 0.01f); //1 unit is 1 cm in arx, so scale down so one unit is one meter (at least perceived)
        }

        public void Save(string name)
        {
            //TODO: set values on arx level native

            ArxLevelNative.SaveLevel(name);
        }
    }
}
