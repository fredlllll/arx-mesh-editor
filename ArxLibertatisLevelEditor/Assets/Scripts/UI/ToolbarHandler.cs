using Assets.Scripts.App;
using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxLevelEditor.Editing;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class ToolbarHandler : MonoBehaviour
    {
        public static void Setup(UIDocument document)
        {
            var toolbarComp = document.GetComponent<ToolbarHandler>();
            if (toolbarComp == null)
            {
                toolbarComp = document.gameObject.AddComponent<ToolbarHandler>();
            }
            toolbarComp.Initialize(document);
        }

        private Button polygonsButton;
        private Button verticesButton;

        private void Initialize(UIDocument doc)
        {
            var toolbar = doc.rootVisualElement.Q<VisualElement>("Toolbar");
            if (toolbar == null)
            {
                Debug.LogError("ToolbarHandler: Could not find 'Toolbar' element in the UI document.");
                return;
            }

            polygonsButton = CreateButton("Polygons", () => SetEditMode(EditState.Polygons));
            verticesButton = CreateButton("Vertices", () => SetEditMode(EditState.Vertices));
            toolbar.Add(polygonsButton);
            toolbar.Add(verticesButton);

            toolbar.Add(CreateSeparator());

            var snapGridSize = new FloatField("Grid Size");
            snapGridSize.AddToClassList("ToolBarField");
            snapGridSize.value = EditorContext.SnapManager.SnapGridSize;
            snapGridSize.RegisterValueChangedCallback(evt =>
            {
                float size = Math.Max(0.00001f, evt.newValue);
                EditorContext.SnapManager.SnapGridSize = size;
                snapGridSize.SetValueWithoutNotify(size);
            });
            toolbar.Add(snapGridSize);

            var snapMode = new DropdownField("Snap Mode");
            snapMode.AddToClassList("ToolBarField");
            foreach (var mode in Enum.GetNames(typeof(SnapMode)))
            {
                snapMode.choices.Add(mode);
            }
            snapMode.index = (int)EditorContext.SnapManager.SnapMode;
            snapMode.RegisterValueChangedCallback(_ =>
            {
                EditorContext.SnapManager.SnapMode = (SnapMode)Math.Max(0, snapMode.index);
            });
            toolbar.Add(snapMode);

            toolbar.Add(CreateSeparator());

            toolbar.Add(CreateButton("Duplicate", () => PolygonSelector.Instance.Duplicate()));
            toolbar.Add(CreateButton("Delete", () => PolygonSelector.Instance.DeleteSelected()));

            toolbar.Add(CreateSeparator());

            Toggle togglePortals = CreateToggle("Portals", EditorContext.CurrentLevel?.LevelPortalsObject?.activeSelf ?? false, value => SetObjectActive(EditorContext.CurrentLevel?.LevelPortalsObject, value));
            Toggle toggleInters = CreateToggle("Inters", EditorContext.CurrentLevel?.LevelIntersObject?.activeSelf ?? false, value => SetObjectActive(EditorContext.CurrentLevel?.LevelIntersObject, value));
            Toggle toggleNavGrid = CreateToggle("NavGrid", EditorContext.CurrentLevel?.LevelNavGridObject?.activeSelf ?? false, value => SetObjectActive(EditorContext.CurrentLevel?.LevelNavGridObject, value));
            toolbar.Add(togglePortals);
            toolbar.Add(toggleInters);
            toolbar.Add(toggleNavGrid);

            toolbar.Add(CreateSeparator());

            var saveButton = CreateButton("Save", () => LevelEditor.SaveLevel());
            saveButton.AddToClassList("ToolBarButtonPrimary");
            toolbar.Add(saveButton);

            SyncEditModeButtons();
        }

        private void SetEditMode(EditState editState)
        {
            EditorContext.EditState = editState;
            SyncEditModeButtons();
        }

        private void SyncEditModeButtons()
        {
            if (polygonsButton == null || verticesButton == null)
            {
                return;
            }
            polygonsButton.SetEnabled(EditorContext.EditState != EditState.Polygons);
            verticesButton.SetEnabled(EditorContext.EditState != EditState.Vertices);
        }

        private static Button CreateButton(string text, Action onClick)
        {
            var button = new Button(onClick) { text = text };
            button.AddToClassList("ToolBarButton");
            return button;
        }

        private static Toggle CreateToggle(string label, bool initialValue, Action<bool> onChanged)
        {
            var toggle = new Toggle(label);
            toggle.AddToClassList("ToolBarToggle");
            toggle.value = initialValue;
            toggle.RegisterValueChangedCallback(evt => onChanged.Invoke(evt.newValue));
            return toggle;
        }

        private static VisualElement CreateSeparator()
        {
            var separator = new VisualElement();
            separator.AddToClassList("ToolBarSeparator");
            return separator;
        }

        private static void SetObjectActive(GameObject obj, bool active)
        {
            if (obj != null && obj.activeSelf != active)
            {
                obj.SetActive(active);
            }
        }
    }
}