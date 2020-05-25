using Assets.Scripts.DLF_IO;
using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.DataSync
{
    public class Inter : MonoBehaviour
    {
        public void LoadFrom(ArxLevel level, int index)
        {
            DLF_IO_INTER inter = level.DLF.inters[index];

            name = ArxIOHelper.GetString(inter.name);

            string interPath = name.ToLowerInvariant();
            int graphPos = interPath.IndexOf("graph");
            interPath = interPath.Substring(graphPos);
            int lastDot = interPath.LastIndexOf('.');
            interPath = interPath.Substring(0, lastDot);

            //depending on what the path is load item, npc, fix, camera or marker
            if (interPath.Contains("items"))
            {

            }
            else if (interPath.Contains("npc"))
            {

            }
            else if (interPath.Contains("fix"))
            {

            }
            else if (interPath.Contains("camera"))
            {

            }
            else if (interPath.Contains("marker"))
            {

            }

            transform.localPosition = inter.pos.ToVector3();
            transform.localEulerAngles = inter.angle.ToEuler();
        }
    }
}
