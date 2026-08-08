using ArxLibertatisLightingCalculatorLib;
using Assets.Scripts.UI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class MenuBar : MonoBehaviour
    {
        private VisualElement rootElement;

        public List<MenuBarItemData> GetMenuBarItems()
        {
            var items = new List<MenuBarItemData>();

            var fileMenu = new MenuBarItemData
            {
                text = "File",
                children =
                {
                    new() { text = "New", callback = File_New },
                    new() { text = "Open", callback = File_Open },
                    new() { text = "Save", callback = File_Save },
                    new() { text = "Exit", callback = File_Exit },
                }
            };
            items.Add(fileMenu);
            var editMenu = new MenuBarItemData
            {
                text = "Edit",
                children =
                {
                    new() { text = "Import .OBJ", callback = Edit_ImportObj },
                }
            };
            items.Add(editMenu);
            var lcMenu = new MenuBarItemData
            {
                text = "Lighting Recalculation",
                children =
                {
                    new() { text = "Danae", callback =  ()=>{ LC_Recalculate(LightingProfile.Danae); } },
                    new() { text = "Distance", callback =  ()=>{ LC_Recalculate(LightingProfile.Distance); } },
                    new() { text = "Distance & Angle", callback =  ()=>{ LC_Recalculate(LightingProfile.DistanceAngle); } },
                    new() { text = "Distance, Angle & Shadow", callback =  ()=>{ LC_Recalculate(LightingProfile.DistanceAngleShadow); } },
                    new() { text = "Distance, Angle, Shadow & No Transparency", callback =  ()=>{ LC_Recalculate(LightingProfile.DistanceAngleShadowNoTransparency); } },
                }
            };
            items.Add(lcMenu);
            return items;
        }

        private void Start()
        {
            var document = GetComponent<UIDocument>();
            rootElement = document.rootVisualElement.Query<VisualElement>("MenuBar");

            MenuBarFactory.AddItems(rootElement, this);
        }

        public void File_New()
        {

        }

        public void File_Open()
        {

        }

        public void File_Save()
        {

        }

        public void File_Exit()
        {

        }

        public void Edit_ImportObj()
        {

        }

        public void LC_Recalculate(LightingProfile profile)
        {

        }
    }
}
