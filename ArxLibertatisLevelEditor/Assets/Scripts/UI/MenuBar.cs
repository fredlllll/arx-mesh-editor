using ArxLibertatisLightingCalculatorLib;
using Assets.Scripts.App;
using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxLevelEditor.Editing;
using Assets.Scripts.ArxLevelLoading;
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

            EditorViewport.Setup(document);

            ToolbarHandler.Setup(document);

            InspectorHandler.Setup(document);

            OpenLevelDialog.ShowIfNeeded(document);
        }

        public void File_New()
        {
            OpenLevelDialog.Show(GetComponent<UIDocument>());
        }

        public void File_Open()
        {
            OpenLevelDialog.Show(GetComponent<UIDocument>());
        }

        public void File_Save()
        {
            LevelEditor.SaveLevel();
        }

        public void File_Exit()
        {
            Application.Quit();
        }

        public void Edit_ImportObj()
        {

        }

        public void LC_Recalculate(LightingProfile profile)
        {
            var lvl = EditorContext.CurrentLevel;
            if (lvl == null)
            {
                return;
            }

            var raycastProvider = new UnityRaycastProvider();
            PolygonSelector.Instance.Deselect(); //to prevent selected polygon from being lost
            LevelSaver.SaveMesh(lvl);
            ArxLibertatisLightingCalculator.Calculate(lvl.MediumArxLevel, profile, raycastProvider);
            raycastProvider.Dispose();
            LevelLoader.ReloadMesh(lvl);
        }
    }
}
