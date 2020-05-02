using Assets.Scripts.DLF;
using System.IO;
using UnityEngine;

public class ArxLevelEditor : MonoBehaviour
{
    public void OpenLevel()
    {
        var dlf = new DLF();
        using (FileStream fs = new FileStream(@"F:\Program Files\Arx Libertatis\paks\graph\levels\level0\level0.dlf", FileMode.Open, FileAccess.Read))
        {
            dlf.LoadFrom(fs);
        }
    }
}
