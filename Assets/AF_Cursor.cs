using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AF_Cursor : MonoBehaviour {

    [HideInInspector]
    public bool _showing;

    public void Show(bool show)
    {
        Debug.Log("Show called: " + show);
        GetComponent<MeshRenderer>().enabled = show;
        // GetComponent<Collider>().enabled = show;

        GetComponentsInChildren<MeshRenderer>()[1].enabled = show;

        _showing = show;
    }


}
