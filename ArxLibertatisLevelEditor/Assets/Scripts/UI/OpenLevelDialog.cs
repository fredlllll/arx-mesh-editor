using Assets.Scripts.App;
using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.Util;
using SFB;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Modal UI Toolkit dialog for picking the Arx data directory and the level to open.
    /// Replaces the legacy uGUI MenuHandler panel.
    /// </summary>
    public static class OpenLevelDialog
    {
        private const string ArxDirPathName = "ArxDirPathName";

        private static UIDocument document;
        private static VisualElement overlay;
        private static TextField arxDirField;
        private static DropdownField levelField;
        private static Label statusLabel;
        private static Button openButton;

        /// <summary>Shows the dialog. If a revoked/invalid saved dir exists it is pre-filled and levels are listed.</summary>
        public static void Show(UIDocument doc)
        {
            document = doc;

            RemoveOverlay();

            overlay = new VisualElement();
            overlay.AddToClassList("DialogOverlay");

            var panel = new VisualElement();
            panel.AddToClassList("DialogPanel");
            overlay.Add(panel);

            var title = new Label("Open Level");
            title.AddToClassList("DialogTitle");
            panel.Add(title);

            var dirRow = new VisualElement();
            dirRow.AddToClassList("DialogRow");
            panel.Add(dirRow);

            var dirLabel = new Label("Arx Directory");
            dirLabel.AddToClassList("DialogFieldLabel");
            dirRow.Add(dirLabel);

            arxDirField = new TextField();
            arxDirField.AddToClassList("DialogField");
            dirRow.Add(arxDirField);

            var browseButton = new Button(BrowseForDirectory);
            browseButton.text = "Browse";
            dirRow.Add(browseButton);

            arxDirField.RegisterValueChangedCallback(evt => OnArxDirChanged(evt.newValue));

            var levelRow = new VisualElement();
            levelRow.AddToClassList("DialogRow");
            panel.Add(levelRow);

            var levelLabel = new Label("Level");
            levelLabel.AddToClassList("DialogFieldLabel");
            levelRow.Add(levelLabel);

            levelField = new DropdownField();
            levelField.AddToClassList("DialogField");
            levelRow.Add(levelField);

            statusLabel = new Label();
            statusLabel.AddToClassList("DialogStatus");
            panel.Add(statusLabel);

            var buttons = new VisualElement();
            buttons.AddToClassList("DialogButtons");
            panel.Add(buttons);

            var cancelButton = new Button(RemoveOverlay);
            cancelButton.text = "Cancel";
            buttons.Add(cancelButton);

            openButton = new Button(TryOpenLevel);
            openButton.text = "Open";
            openButton.AddToClassList("DialogButtonPrimary");
            buttons.Add(openButton);

            document.rootVisualElement.Add(overlay);

            arxDirField.value = GetSavedDir() ?? "";
            RefreshLevels();
        }

        /// <summary>Shows the dialog at startup only if no valid arx dir is saved (old MenuHandler.Awake behavior).</summary>
        public static void ShowIfNeeded(UIDocument doc)
        {
            string dir = GetSavedDir();
            if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir))
            {
                Show(doc);
            }
        }

        private static void RemoveOverlay()
        {
            if (overlay != null && overlay.parent != null)
            {
                overlay.RemoveFromHierarchy();
            }
            overlay = null;
        }

        private static void BrowseForDirectory()
        {
            PathUtil.SetCurrentWorkingDirectoryToProjectRoot();
            StandaloneFileBrowser.OpenFolderPanelAsync("Find Arx Directory", arxDirField.value, false, folders =>
            {
                if (folders.Length > 0)
                {
                    arxDirField.value = folders[0];
                }
            });
        }

        private static void OnArxDirChanged(string newValue)
        {
            if (!Directory.Exists(newValue))
            {
                levelField.choices = new List<string>();
                levelField.index = -1;
                SetStatus("Arx directory not found.", true);
                openButton.SetEnabled(false);
                return;
            }

            PlayerPrefs.SetString(ArxDirPathName, newValue);
            SetStatus("", false);
            openButton.SetEnabled(true);
            RefreshLevels();
        }

        private static void RefreshLevels()
        {
            string dir = arxDirField.value;
            var levels = new List<string>();

            var levelsDir = Path.Combine(dir, "graph", "levels");
            if (Directory.Exists(levelsDir))
            {
                foreach (string subDir in Directory.GetDirectories(levelsDir))
                {
                    string name = Path.GetFileName(subDir);
                    if (File.Exists(Path.Combine(subDir, name + ".dlf")))
                    {
                        levels.Add(name);
                    }
                }
            }
            levels.Sort((a, b) =>
            {
                int na = ExtractTrailingNumber(a);
                int nb = ExtractTrailingNumber(b);
                if (na >= 0 && nb >= 0)
                {
                    return na.CompareTo(nb);
                }
                return string.Compare(a, b, StringComparison.OrdinalIgnoreCase);
            });

            levelField.choices = levels;
            if (levels.Count > 0)
            {
                levelField.index = Math.Max(0, levels.FindIndex(l => l == PlayerPrefs.GetString("LevelName", "")));
            }
            else
            {
                levelField.index = -1;
                SetStatus("No levels found in " + levelsDir, levels.Count == 0);
            }
        }

        private static int ExtractTrailingNumber(string name)
        {
            int i = name.Length;
            while (i > 0 && char.IsDigit(name[i - 1]))
            {
                i--;
            }
            if (i == name.Length)
            {
                return -1;
            }
            return int.TryParse(name.Substring(i), out int result) ? result : -1;
        }

        private static void TryOpenLevel()
        {
            string dir = arxDirField.value;
            if (string.IsNullOrEmpty(dir))
            {
                SetStatus("Select the Arx data directory first.", true);
                return;
            }
            if (!Directory.Exists(dir))
            {
                SetStatus("Arx directory not found.", true);
                return;
            }

            string name = levelField.value;
            if (string.IsNullOrEmpty(name))
            {
                SetStatus("No level selected.", true);
                return;
            }

            PlayerPrefs.SetString(ArxDirPathName, dir);
            PlayerPrefs.SetString("LevelName", name);

            ArxLibertatisEditorIO.ArxPaths.DataDir = dir;
            EditorSettings.DataDir = dir;

            LevelEditor.OpenLevel(name);
            RemoveOverlay();
        }

        private static void SetStatus(string text, bool isError)
        {
            statusLabel.text = text;
            statusLabel.EnableInClassList("DialogStatusError", isError);
        }

        private static string GetSavedDir()
        {
            string dir = null;
            try
            {
                dir = PlayerPrefs.GetString(ArxDirPathName, null);
            }
            catch
            {
                dir = null;
            }
            if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir))
            {
                return null;
            }
            return dir;
        }
    }
}