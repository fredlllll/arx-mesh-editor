using ArxLibertatisEditorIO.MediumIO.FTS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI.Elements
{
    public static class MenuBarFactory
    {
        public static void AddItems(VisualElement root, MenuBar bar)
        {
            var items = bar.GetMenuBarItems();

            foreach (var item in items)
            {
                AddItem(root, item);
            }
        }

        private static void AddItem(VisualElement parent, MenuBarItemData item)
        {
            var button = new Button();
            
            parent.Add(button);
            if (item.children.Count > 0)
            {
                button.text = item.text+" >";
                var subMenu = new VisualElement();
                subMenu.style.position = Position.Absolute;
                subMenu.style.left = button.worldBound.xMin;
                subMenu.style.top = button.worldBound.yMin + button.worldBound.height;
                subMenu.style.display = DisplayStyle.None;
                parent.Add(subMenu);
                button.clicked += () => { 
                    subMenu.style.display = subMenu.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
                    if(subMenu.style.display == DisplayStyle.None)
                    {
                        button.text = item.text + " >";
                    }
                    else
                    {
                        button.text = item.text + " V";
                    }
                };
                foreach (var child in item.children)
                {
                    AddItem(subMenu,child);
                }
            }
            else
            {
                button.text = item.text;
                button.clicked += item.callback;
            }
        }
    }
}
