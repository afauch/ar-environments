﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap.Unity.Interaction;
using Leap.Unity.Attachments;
using UnityEngine.Events;

public class AF_ParallelPalmsPointer : MonoBehaviour
{

    public AttachmentPointBehaviour _rightPalm;
    public AttachmentPointBehaviour _indexMiddleJoint;
    public AttachmentPointBehaviour _thumbTip;
    public GameObject _cursor;
    public GameObject _aroundCursor;

    private float _maxScale = 6.0f;
    private float _unThreshold = 0.03f;

    public LayerMask _layers;

    public bool _drawLine = false;

    public float _thresholdDistance = 0.02f;

    private bool _initialized;
    private bool _isDown;

    public UnityEvent OnThumbDown;
    public UnityEvent OnThumbUp;

    private LineRenderer _lr;

    // Use this for initialization
    void Start()
    {

        _lr = GetComponent<LineRenderer>();

        if (_rightPalm != null && _indexMiddleJoint != null && _thumbTip != null) Initialize();

    }

    private void Initialize()
    {
        _isDown = GetIsDown(_thumbTip.transform, _indexMiddleJoint.transform, _thresholdDistance);
        _initialized = true;
    }

    // Update is called once per frame
    void Update()
    {

        DrawPointer();
        CheckClick();

    }

    void DrawPointer()
    {

        if(_drawLine)
        {
            _lr.enabled = true;
        }
        else
        {
            _lr.enabled = false;
        }

        // Raycast to find the right point
        RaycastHit hit;
        Vector3[] positions;

        if (Physics.Raycast(_rightPalm.transform.position, _rightPalm.transform.forward, out hit, Mathf.Infinity, _layers))
        {

            positions = new Vector3[]
            {
                _rightPalm.transform.position,
                hit.point
            };

            _cursor.transform.position = hit.point;

        }
        else
        {
            positions = new Vector3[]
            {
                _rightPalm.transform.position
            };
        }

        _lr.SetPositions(positions);


    }

    private void CheckClick()
    {
        if (_rightPalm != null && _indexMiddleJoint != null && _thumbTip != null && !_initialized)
        {
            Initialize();
        }
        if (!_initialized) return;

        if (GetIsDown(_thumbTip.transform, _indexMiddleJoint.transform, _thresholdDistance) != _isDown)
        {
            _isDown = !_isDown;

            if (_isDown)
            {
                Debug.Log("Click Down");
                OnThumbDown.Invoke();
            }
            else
            {
                Debug.Log("Click Up");
                OnThumbUp.Invoke();
            }
        }
    }


    public bool GetIsDown(Transform thumbTransform, Transform indexMiddleJointTransform, float thresholdDistance)
    {
        float d = Vector3.Distance(thumbTransform.position, indexMiddleJointTransform.position);
        _aroundCursor.transform.localScale = Vector3.Lerp(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(_maxScale, _maxScale, _maxScale), (d/_unThreshold - _thresholdDistance));
        return d < thresholdDistance ? true : false;
    }



}
