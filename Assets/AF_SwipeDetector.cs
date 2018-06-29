using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crayon;

public class AF_SwipeDetector : MonoBehaviour {

    private bool _isExtended = false;
    public Transform _palmFacingTransform;
    public GameObject _tube;

    public GameObject[] _tubeComponents;
    private int _currentTubeComponentIndex = 0;
    public float _tweenTime = 1.2f;

    public AudioSource _audioSource;


    private void Update()
    {
        // GetCouldSwipe();
    }

    public void OnAllExtended()
    {
        Debug.Log("All Extended.");
        _isExtended = true;

        // Rotate the chassis
        _tube.SetRelativeRotation(new Vector3(0.0f, 90.0f, 0.0f), _tweenTime, Easing.BounceOut);
        _audioSource.Play();

        Debug.Log(_tubeComponents[_currentTubeComponentIndex].name);

        // Fade Out the current one
        _tubeComponents[_currentTubeComponentIndex].SetOpacity(0.0f, _tweenTime);
        _tubeComponents[_currentTubeComponentIndex].SetPosition(new Vector3(0.0f, 0.0f, 0.0f), _tweenTime, Easing.QuarticIn);


        if (_currentTubeComponentIndex == 0)
        {
            _currentTubeComponentIndex = 3;
        } else
        {
            _currentTubeComponentIndex -= 1;
        }

        // Fade in the new component
        _tubeComponents[_currentTubeComponentIndex].SetOpacity(1.0f, _tweenTime);
        _tubeComponents[_currentTubeComponentIndex].SetPosition(new Vector3(0.0f, 6.0f, 0.0f), _tweenTime, Easing.CubicOut);


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
