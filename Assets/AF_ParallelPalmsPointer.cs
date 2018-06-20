using System.Collections;
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

    [Header("Cursor")]
    private bool _show = false;
    public GameObject _cursor;
    public GameObject _aroundCursor;

    [Header("Fist Detection")]
    public AttachmentPointBehaviour _indexTip;
    public AttachmentPointBehaviour _middleTip;
    public AttachmentPointBehaviour _ringTip;
    public AttachmentPointBehaviour _pinkyTip;
    public float _fistThreshold = 0.08f;
    private bool _isFist = false;

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

        if (ShowCursor())
        {
            DrawPointer();
            CheckClick();
        }

    }

    bool ShowCursor()
    {
        // get the current state of the fist
        bool fist = GetIsFist();

        // if the current state is different than the previous frame's state...
        if (fist != _isFist)
        {

            // let the cursor know the fist state. If fist, then it will show
            _cursor.GetComponent<AF_Cursor>().Show(fist);

            // Update this frame
            _isFist = fist;
        } else
        {
            _isFist = fist;
        }

        return fist;

    }


    bool GetIsFist()
    {
        bool fist = false;

        float avg = GetFourFingerAvg();
        if (avg >= 1.0f)
        {
            fist = true;
        }

        return fist;
    }

    float GetFourFingerAvg()
    {
        float fourFingerAvg = 0;

        float index = Vector3.Distance(_indexTip.transform.position, _rightPalm.transform.position) < _fistThreshold ? 1.0f : 0.0f;
        float middle = Vector3.Distance(_middleTip.transform.position, _rightPalm.transform.position) < _fistThreshold ? 1.0f : 0.0f;
        float ring = Vector3.Distance(_ringTip.transform.position, _rightPalm.transform.position) < _fistThreshold ? 1.0f : 0.0f;
        float pinky = Vector3.Distance(_pinkyTip.transform.position, _rightPalm.transform.position) < _fistThreshold ? 1.0f : 0.0f;

        fourFingerAvg = (index + middle + ring + pinky) / 4;

        return fourFingerAvg;
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
