using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using Leap.Unity.Attachments;
using Crayon;

public class AF_BounceSnap : MonoBehaviour {

    public Transform _baseTransform;
    public Transform _pinchMidpoint;
    public GameObject _snappableObject;
    private Vector3 _basePosition;
    public float _tweenLength = 0.4f;
    public LineRenderer _lr;

	// Use this for initialization
	void Start () {

        _basePosition = _baseTransform.position;

	}

    private void FixedUpdate()
    {
        _lr.SetPosition(0, _lr.gameObject.transform.position);
        _lr.SetPosition(1, _snappableObject.transform.position);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Magnet")
        {
            StartCoroutine(SnapToPinch(true));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Magnet")
        {
            StartCoroutine(SnapToPinch(false));
        }
    }

    private IEnumerator SnapToPinch(bool towardsPinch)
    {

        if (towardsPinch)
        {
            //TODO: Currently tween is going to the wrong place
            _snappableObject.transform.SetParent(null);
            _snappableObject.SetTransform(_pinchMidpoint.transform, _tweenLength, Easing.BounceOut);
            yield return new WaitForSeconds(_tweenLength);
            _snappableObject.transform.SetParent(_pinchMidpoint);
            _snappableObject.transform.localPosition = Vector3.zero;
            //_snappableObject.transform.position = _pinchMidpoint.transform.position;
            //_snappableObject.transform.SetParent(_pinchMidpoint);
        }
        else
        {
            _snappableObject.transform.SetParent(null);
            _snappableObject.transform.SetParent(_baseTransform);
            _snappableObject.SetPosition(Vector3.zero, _tweenLength, Easing.BounceOut);
            yield return new WaitForSeconds(_tweenLength);
            // _snappableObject.SetPosition(_basePosition);
            //yield return new WaitForSeconds(0.8f);
            //_snappableObject.transform.position = _basePosition;
        }

        yield return null;
    }

}
