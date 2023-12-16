using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossDotProduct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 myOldV = new Vector3(0, 0, 1);
        Vector3 myVector = new Vector3(0, 0, -1);

        Debug.Log(Vector3.Dot(myOldV, myVector));
        Debug.Log(Vector3.Cross(myVector, myOldV));
    }

   
}
