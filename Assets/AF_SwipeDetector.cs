using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crayon;

public class AF_SwipeDetector : MonoBehaviour {

    private bool _isExtended = false;
    public Transform _palmFacingTransform;
    public GameObject _tube;

    private void Update()
    {
        // GetCouldSwipe();
    }

    public void OnAllExtended()
    {
        Debug.Log("All Extended.");
        _isExtended = true;

        _tube.SetRelativeRotation(new Vector3(0.0f, -90.0f, 0.0f));

    }

    public void OnNotExtended()
    {
        Debug.Log("Not Extended.");
        _isExtended = false;
    }

    private void GetCouldSwipe()
    {

        if(_isExtended)
        {
            
        }

    }

}
