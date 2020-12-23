using Assets.Scripts.ArxLevelEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class TopBarHandler :MonoBehaviour
    {
        public GameObject polygonsButton, verticesButton;
        private void Start()
        {
            
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
    }
}
