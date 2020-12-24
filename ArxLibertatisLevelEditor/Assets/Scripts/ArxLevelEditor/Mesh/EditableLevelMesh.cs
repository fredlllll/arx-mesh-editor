using Assets.Scripts.ArxLevelEditor;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Mesh
{
    public class EditableLevelMesh : MonoBehaviour
    {
        readonly Dictionary<EditorMaterial, MaterialMesh> materialMeshes = new Dictionary<EditorMaterial, MaterialMesh>();

        public IEnumerable<KeyValuePair<EditorMaterial, MaterialMesh>> MaterialMeshes
        {
            get
            {
                return materialMeshes;
            }
        }

        public MaterialMesh GetMaterialMesh(EditorMaterial key)
        {
            if (!materialMeshes.TryGetValue(key, out var retval))
            {
                var go = new GameObject();
                go.transform.SetParent(gameObject.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localEulerAngles = Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.layer = gameObject.layer;

                retval = go.AddComponent<MaterialMesh>();
                retval.EditorMaterial = key;

                materialMeshes[key] = retval;
            }
            return retval;
        }

        private void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("EditableLevelMesh");
        }
    }
}
