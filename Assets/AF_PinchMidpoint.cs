using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AF_PinchMidpoint : MonoBehaviour {

    public Transform _indexTip;
    public Transform _thumbTip;
    public float _thresholdDistance = 0.05f;

    private bool _initialized;
    private bool _isPinching;
    public bool _isOccupied = false;

    public UnityEvent OnBeginPinch;
    public UnityEvent OnEndPinch;

    private void Start()
    {
        if (_indexTip != null && _thumbTip != null) Initialize();
    }

    private void Initialize()
    {
        _isPinching = GetIsPinching(_indexTip, _thumbTip, _thresholdDistance);
        _initialized = true;
    }

    // Update is called once per frame
    void Update()
    {

        this.gameObject.transform.position = (_indexTip.position + _thumbTip.position) / 2;

        if (_indexTip != null && _thumbTip != null && !_initialized)
        {
            Initialize();
        }
        if (!_initialized) return;

        if (GetIsPinching(_indexTip, _thumbTip, _thresholdDistance) != _isPinching)
        {
            _isPinching = !_isPinching;
            // Debug.Log(_isExtended);

            if (_isPinching)
            {
                Debug.Log("Begin pinch " + gameObject.name);
                OnBeginPinch.Invoke();
            }
            else
            {
                Debug.Log("End pinch " + gameObject.name);
                OnEndPinch.Invoke();
            }
        }
    }


    public static bool GetIsPinching(Transform index, Transform thumb, float thresholdDistance)
    {
        float d = Vector3.Distance(index.position, thumb.position);
        return d < thresholdDistance ? true : false;
    }


}
