using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crayon;

public class TestCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Hide ()
    {
        gameObject.SetState(CrayonStateType.Custom, "Hidden");
    }

    public void Show ()
    {
        gameObject.SetState(CrayonStateType.Default);
    }

}
