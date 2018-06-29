using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using Leap.Unity.Attachments;
using Crayon;

public class AF_PulldownHandle : MonoBehaviour
{

    public Transform _baseTransform;
    private Transform _pinchMidpoint;
    public GameObject _snappableObject;
    private Vector3 _basePosition;
    public float _tweenLength = 0.4f;
    public LineRenderer _lr;

    public float _fullExtensionAmount = 0.8f; // at full extension, the handle ceases to do anything

    public Transform _stencilCurtain;
    private Vector3 _stencilCurtainBasePosition;
    public float _stencilCurtainMaxYDisplacement = 0.8f; // how much higher than its current position can the stencil curtain go
    private bool _isBeingPulled = false;
    private bool _isReceding = false;

    private float _hysteresis = 0.5f;

    private bool _handleIsInPlace = false;

    // Use this for initialization
    void Start()
    {

        _basePosition = _baseTransform.position;
        _stencilCurtainBasePosition = _stencilCurtain.position;
    }

    private void Update()
    {
        _lr.SetPosition(0, _lr.gameObject.transform.position);
        _lr.SetPosition(1, _snappableObject.transform.position);

        if(_isBeingPulled || _isReceding)
        {
            MoveStencilCurtain();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Magnet")
        {
            _pinchMidpoint = other.gameObject.transform;
            if (!other.GetComponent<AF_PinchMidpoint>()._isOccupied)
            {

                StartCoroutine(SnapToPinch(true));
                _pinchMidpoint.GetComponent<AF_PinchMidpoint>().OnBeginPinch.AddListener(StartPull);
                _pinchMidpoint.GetComponent<AF_PinchMidpoint>().OnEndPinch.AddListener(StopPull);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Magnet" && !_isBeingPulled)
        {
            StartCoroutine(SnapToPinch(false));
            _pinchMidpoint.GetComponent<AF_PinchMidpoint>().OnBeginPinch.RemoveListener(StartPull);
            _pinchMidpoint.GetComponent<AF_PinchMidpoint>().OnEndPinch.RemoveListener(StopPull);
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
            yield return new WaitForSeconds(((1 - _hysteresis) * _tweenLength) + 0.01f);
            _snappableObject.transform.SetParent(_pinchMidpoint);
            _snappableObject.transform.localPosition = Vector3.zero;
            //_snappableObject.transform.position = _pinchMidpoint.transform.position;
            //_snappableObject.transform.SetParent(_pinchMidpoint);
        }
        else
        {
            _isReceding = true;
            _pinchMidpoint.GetComponent<AF_PinchMidpoint>()._isOccupied = false;
            _snappableObject.transform.SetParent(null);
            _snappableObject.transform.SetParent(_baseTransform);
            _snappableObject.SetPosition(Vector3.zero, _tweenLength, Easing.BounceOut);
            yield return new WaitForSeconds(_tweenLength);
            _handleIsInPlace = false;
            _isReceding = false;
            // _snappableObject.SetPosition(_basePosition);
            //yield return new WaitForSeconds(0.8f);
            //_snappableObject.transform.position = _basePosition;
        }

        yield return null;
    }

    private void StartPull()
    {
        _isBeingPulled = true;
    }

    private void StopPull()
    {
        _isBeingPulled = false;
        StartCoroutine(SnapToPinch(false));
    }

    private void MoveStencilCurtain()
    {
        float d = Vector3.Distance(_snappableObject.transform.position, _baseTransform.position);
        float f = d / _fullExtensionAmount;

        _stencilCurtain.transform.position = new Vector3(
                _stencilCurtain.transform.position.x,
                _stencilCurtainBasePosition.y + (f * _stencilCurtainMaxYDisplacement),
                _stencilCurtain.transform.position.z
            );
    }

}
