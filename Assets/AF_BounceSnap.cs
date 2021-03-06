﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using Leap.Unity.Attachments;
using Crayon;

public class AF_BounceSnap : MonoBehaviour {

    public Transform _baseTransform;
    private Transform _pinchMidpoint;
    public GameObject _snappableObject;
    private Vector3 _basePosition;
    public float _tweenLength = 0.4f;
    public LineRenderer _lr;

    private float _hysteresis = 0.5f;

    private bool _handleIsInPlace = false;

	// Use this for initialization
	void Start () {

        _basePosition = _baseTransform.position;

	}

    private void Update()
    {
        _lr.SetPosition(0, _lr.gameObject.transform.position);
        _lr.SetPosition(1, _snappableObject.transform.position);

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Magnet")
        {
            _pinchMidpoint = other.gameObject.transform;
            if (!other.GetComponent<AF_PinchMidpoint>()._isOccupied)
            {

                StartCoroutine(SnapToPinch(true));
                _pinchMidpoint.GetComponent<AF_PinchMidpoint>().OnBeginPinch.AddListener(StartMoveBase);
                _pinchMidpoint.GetComponent<AF_PinchMidpoint>().OnEndPinch.AddListener(StopMoveBase);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Magnet")
        {
            StartCoroutine(SnapToPinch(false));
            _pinchMidpoint.GetComponent<AF_PinchMidpoint>().OnBeginPinch.RemoveListener(StartMoveBase);
            _pinchMidpoint.GetComponent<AF_PinchMidpoint>().OnEndPinch.RemoveListener(StopMoveBase);
        }
    }

    private IEnumerator SnapToPinch(bool towardsPinch)
    {

        if (towardsPinch)
        {
            _pinchMidpoint.GetComponent<AF_PinchMidpoint>()._isOccupied = true;
            //TODO: Currently tween is going to the wrong place
            _snappableObject.transform.SetParent(null);
            _snappableObject.SetTransform(_pinchMidpoint.transform, _tweenLength, Easing.BounceOut);
            yield return new WaitForSeconds(_hysteresis * _tweenLength);
            _handleIsInPlace = true;
            yield return new WaitForSeconds(((1-_hysteresis) * _tweenLength) + 0.01f);
            _snappableObject.transform.SetParent(_pinchMidpoint);
            _snappableObject.transform.localPosition = Vector3.zero;
            //_snappableObject.transform.position = _pinchMidpoint.transform.position;
            //_snappableObject.transform.SetParent(_pinchMidpoint);
        }
        else
        {
            _pinchMidpoint.GetComponent<AF_PinchMidpoint>()._isOccupied = false;
            _snappableObject.transform.SetParent(null);
            _snappableObject.transform.SetParent(_baseTransform);
            _snappableObject.SetPosition(Vector3.zero, _tweenLength, Easing.BounceOut);
            yield return new WaitForSeconds(_tweenLength);
            _handleIsInPlace = false;
            // _snappableObject.SetPosition(_basePosition);
            //yield return new WaitForSeconds(0.8f);
            //_snappableObject.transform.position = _basePosition;
        }

        yield return null;
    }

    private void StartMoveBase ()
    {
        if (_handleIsInPlace)
        {
            _baseTransform.SetParent(_pinchMidpoint);
        }
    }

    private void StopMoveBase ()
    {
        _baseTransform.SetParent(null);
        StartCoroutine(SnapToPinch(false));
    }

}
