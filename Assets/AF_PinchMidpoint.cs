using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AF_PinchMidpoint : MonoBehaviour {

    public Transform _indexTip;
    public Transform _thumbTip;
	
	// Update is called once per frame
	void Update () {

        this.gameObject.transform.position = (_indexTip.position + _thumbTip.position) / 2;


	}
}
