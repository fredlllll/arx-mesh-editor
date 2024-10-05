using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    public class SelectionChangedEvent : UnityEvent<Selection> { }

    public class SelectionObjectsAddRemEvent : UnityEvent<Selection, Selectable[]> { }

    public class Selection
    {
        HashSet<Selectable> selectedObjects = new HashSet<Selectable>();

        public IReadOnlyCollection<Selectable> SelectedObjects
        {
            get { return selectedObjects; }
        }

        public int Count { get { return selectedObjects.Count; } }

        public SelectionChangedEvent SelectionChanged { get; } = new SelectionChangedEvent();
        public SelectionObjectsAddRemEvent ObjectsAdded { get; } = new SelectionObjectsAddRemEvent();
        public SelectionObjectsAddRemEvent ObjectsRemoved { get; } = new SelectionObjectsAddRemEvent();

        public Vector3 Center { get; private set; }

        public void Add(Selectable obj)
        {
            selectedObjects.Add(obj);
            SelectionChanged.Invoke(this);
            ObjectsAdded.Invoke(this, new Selectable[] { obj });
            CalculateCenter();
        }

        public void Add(Selectable[] objs)
        {
            foreach (var o in objs)
            {
                selectedObjects.Add(o);
            }
            SelectionChanged.Invoke(this);
            ObjectsAdded.Invoke(this, objs);
            CalculateCenter();
        }

        public void Remove(Selectable obj)
        {
            selectedObjects.Remove(obj);
            SelectionChanged.Invoke(this);
            ObjectsRemoved.Invoke(this, new Selectable[] { obj });
            CalculateCenter();
        }

        public void Clear()
        {
            if (selectedObjects.Count > 0)
            {
                var objs = selectedObjects.ToArray();
                selectedObjects.Clear();
                SelectionChanged.Invoke(this);
                ObjectsRemoved.Invoke(this, objs);
                Center = Vector3.zero;
            }
        }

        public void Translate(Vector3 translation)
        {
            Center += translation;
            foreach (var sel in selectedObjects)
            {
                var pos = sel.target.transform.position;
                pos += translation;
                sel.target.transform.position = pos;
            }
        }

        public void SetCenter(Vector3 cen)
        {
            var translation = cen - Center;
            Translate(translation);
        }

        private void CalculateCenter()
        {
            var pos = Vector3.zero;
            foreach (var sel in selectedObjects)
            {
                pos += sel.target.transform.position;
            }
            pos /= selectedObjects.Count;
            Center = pos;
        }
    }
}
