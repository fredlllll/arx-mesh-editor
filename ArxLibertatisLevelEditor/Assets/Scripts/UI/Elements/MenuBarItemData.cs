using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.UI.Elements
{
    public class MenuBarItemData
    {
        public string text;
        public Action callback;
        public List<MenuBarItemData> children = new();
    }
}
