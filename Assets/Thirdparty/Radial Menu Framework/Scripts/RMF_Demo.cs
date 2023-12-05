using UnityEngine;
using System.Collections;

public class RMF_Demo : MonoBehaviour {

    public ParticleSystem ps;
    public RMF_RadialMenu rm;
	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update() {


        if (Input.GetKeyDown(KeyCode.S) && rm.useLazySelection) {

            rm.useSelectionFollower = !rm.useSelectionFollower;
            rm.selectionFollowerContainer.gameObject.SetActive(rm.useSelectionFollower);


        }


        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Application.LoadLevel(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Application.LoadLevel(1);
        }


    }

    public void emitButton(int count) {

        ps.Emit(count);



    }


}
