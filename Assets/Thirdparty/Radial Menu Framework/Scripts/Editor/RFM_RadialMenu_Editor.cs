using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RMF_RadialMenu))]
public class NewBehaviourScript : Editor {

    public override void OnInspectorGUI() {

        DrawDefaultInspector();


        RMF_RadialMenu rm = (RMF_RadialMenu)target;

        GUIContent visualize = new GUIContent("Visualize Arrangement", "Press this to preview what the radial menu will look like ingame.");
        GUIContent reset = new GUIContent("Reset Arrangement", "Press this to reset all elements to a 0 rotation for easy editing.");

        if (!Application.isPlaying) {
            if (GUILayout.Button(visualize)) {

                arrangeElementsInEditor(rm, false);

            }

            if (GUILayout.Button(reset)) {

                arrangeElementsInEditor(rm, true);

            }

        }

    }




    public void arrangeElementsInEditor(RMF_RadialMenu rm, bool reset) {

        if (reset) {


            for (int i = 0; i < rm.elements.Count; i++) {
                if (rm.elements[i] == null) {
                    Debug.LogError("Radial Menu: element " + i.ToString() + " in the radial menu " + rm.gameObject.name + " is null!");
                    continue;
                }
                RectTransform elemRt = rm.elements[i].GetComponent<RectTransform>();
                elemRt.rotation = Quaternion.Euler(0, 0, 0);

            }

            return;
        }


        for (int i = 0; i < rm.elements.Count; i++) {
            if (rm.elements[i] == null) {
                Debug.LogError("Radial Menu: element " + i.ToString() + " in the radial menu " + rm.gameObject.name + " is null!");
                continue;
            }
            RectTransform elemRt = rm.elements[i].GetComponent<RectTransform>();
            elemRt.rotation = Quaternion.Euler(0, 0, -((360f / (float)rm.elements.Count) * i) - rm.globalOffset);

        }


    }



}
