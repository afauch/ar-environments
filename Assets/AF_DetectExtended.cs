using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using Leap.Unity;
using Leap.Unity.Attachments;
using UnityEngine.Events;

public class AF_DetectExtended : MonoBehaviour {

    public AttachmentPointBehaviour _palm;
    private AttachmentPointBehaviour _thisFinger;
    public float _thresholdDistance = 0.08f;

    private bool _initialized;
    private bool _isExtended;

    public UnityEvent OnBeginExtended;
    public UnityEvent OnEndExtended;

    // Use this for initialization
    void Start () {

        _thisFinger = GetComponent<AttachmentPointBehaviour>();
        if (_palm != null && _thisFinger != null) Initialize();

    }

    private void Initialize()
    {
        _isExtended = GetIsExtended(_thisFinger.transform, _palm.transform, _thresholdDistance);
        _initialized = true;
    }

    // Update is called once per frame
    void Update () {

        if (_palm != null && _thisFinger != null && !_initialized)
        {
            Initialize();
        }
        if (!_initialized) return;

        if (GetIsExtended(_thisFinger.transform, _palm.transform, _thresholdDistance) != _isExtended)
        {
            _isExtended = !_isExtended;
            Debug.Log(_isExtended);

            if (_isExtended)
            {
                Debug.Log("Begin extended " + gameObject.name);
                OnBeginExtended.Invoke();
            }
            else
            {
                Debug.Log("End extended " + gameObject.name);
                OnEndExtended.Invoke();
            }
        }

	}

    public static bool GetIsExtended(Transform fingerTransform, Transform palm, float thresholdDistance)
    {
        float d = Vector3.Distance(palm.position, fingerTransform.position);
        return d > thresholdDistance ? true : false;
    }

}
