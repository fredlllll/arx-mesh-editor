using Assets.Scripts.App;
using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxLevelEditor.Editing;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class TopBarHandler : MonoBehaviour
    {
        public GameObject polygonsButton, verticesButton;

        public InputField snapGridSize;
        public Dropdown snapMode;

        public Toggle togglePortals, toggleInters, toggleNavGrid;

        private void Start()
        {
            snapMode.value = (int)EditorContext.SnapManager.SnapMode;
            snapGridSize.text = EditorContext.SnapManager.SnapGridSize.ToString(System.Globalization.CultureInfo.InvariantCulture);

            snapMode.onValueChanged.AddListener(this.SnapModeChanged);
            snapGridSize.onValueChanged.AddListener(this.SnapGridSizeChanged);
            snapGridSize.onEndEdit.AddListener(this.SnapGridSizeEditEnd);
        }

        public void SaveClicked()
        {
            LevelEditor.SaveLevel();
        }

        public void PolygonsClicked()
        {
            EditorContext.EditState = EditState.Polygons;
            polygonsButton.GetComponent<Button>().interactable = false;
            verticesButton.GetComponent<Button>().interactable = true;
        }

        public void VerticesClicked()
        {
            EditorContext.EditState = EditState.Vertices;
            polygonsButton.GetComponent<Button>().interactable = true;
            verticesButton.GetComponent<Button>().interactable = false;
        }

        public void SnapModeChanged(int snapMode)
        {
            EditorContext.SnapManager.SnapMode = (SnapMode)snapMode;
        }

        public void SnapGridSizeChanged(string value)
        {
            if (float.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float val))
            {
                EditorContext.SnapManager.SnapGridSize = val;
            }
        }

        void SnapGridSizeEditEnd(string value)
        {
            snapGridSize.text = EditorContext.SnapManager.SnapGridSize.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public void DuplicatePolygon()
        {
            PolygonSelector.Instance.Duplicate();
        }

        public void DeletePolygon()
        {
            PolygonSelector.Instance.DeleteSelected();
        }

        public void TogglePortals()
        {
            EditorContext.CurrentLevel.LevelPortalsObject.SetActive(togglePortals.isOn);
        }

        public void ToggleInters()
        {
            EditorContext.CurrentLevel.LevelIntersObject.SetActive(toggleInters.isOn);
        }

        public void ToggleNavGrid()
        {
            EditorContext.CurrentLevel.LevelNavGridObject.SetActive(toggleNavGrid.isOn);
        }
    }
}
