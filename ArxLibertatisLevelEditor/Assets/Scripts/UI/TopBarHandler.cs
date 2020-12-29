using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxLevelEditor.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class TopBarHandler : MonoBehaviour
    {
        public GameObject polygonsButton, verticesButton;

        public InputField snapGridSize;
        public Dropdown snapMode;

        private void Start()
        {
            snapMode.value = (int)LevelEditor.SnapManager.SnapMode;
            snapGridSize.text = LevelEditor.SnapManager.SnapGridSize.ToString(System.Globalization.CultureInfo.InvariantCulture);

            snapMode.onValueChanged.AddListener(this.SnapModeChanged);
            snapGridSize.onValueChanged.AddListener(this.SnapGridSizeChanged);
            snapGridSize.onEndEdit.AddListener(this.SnapGridSizeEditEnd);
        }

        public void SaveClicked()
        {

        }

        public void PolygonsClicked()
        {
            LevelEditor.EditState = EditState.Polygons;
            polygonsButton.GetComponent<Button>().interactable = false;
            verticesButton.GetComponent<Button>().interactable = true;
        }

        public void VerticesClicked()
        {
            LevelEditor.EditState = EditState.Vertices;
            polygonsButton.GetComponent<Button>().interactable = true;
            verticesButton.GetComponent<Button>().interactable = false;
        }

        public void SnapModeChanged(int snapMode)
        {
            LevelEditor.SnapManager.SnapMode = (SnapMode)snapMode;
        }

        public void SnapGridSizeChanged(string value)
        {
            if (float.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float val))
            {
                LevelEditor.SnapManager.SnapGridSize = val;
            }
        }

        void SnapGridSizeEditEnd(string value)
        {
            snapGridSize.text = LevelEditor.SnapManager.SnapGridSize.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public void DuplicatePolygon()
        {
            PolygonSelector.Duplicate();
        }
    }
}
