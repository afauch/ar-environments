using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crayon;

public class AF_SwipeDetector : MonoBehaviour {

    private bool _isExtended = false;
    public Transform _palmFacingTransform;
    public GameObject _tubeShell;
    public GameObject _tube;
    public Transform _bodyCenter;

    private float _snapThreshold = 25.0f;

    public GameObject[] _tubeComponents;
    public float[] _snapRotations;
    private int _currentTubeComponentIndex = 0;
    public float _tweenTime = 4.0f;

    public LineRenderer _lr1;
    public LineRenderer _lr2;
    public LineRenderer _lr3;

    public AudioSource _audioSource;

    float _initAngle;
    float _angleDelta = 0.0f;
    float _initTubeYRot = 0.0f;


    private void FixedUpdate()
    {
        // Debug.Log(_currentTubeComponentIndex);
        AdjustTransform();
    }

    public void OnAllExtended()
    {
        Debug.Log("All Extended.");
        _angleDelta = 0.0f;
        _initTubeYRot = _tube.transform.localRotation.eulerAngles.y;
        _initAngle = GetSignedAngle(_bodyCenter, _palmFacingTransform);
        _isExtended = true;
        // RotateToNextLens();

    }

    public void OnNotExtended()
    {
        Debug.Log("Not Extended.");
        SnapTo(_currentTubeComponentIndex);
        _isExtended = false;
    }


    private void AdjustTransform()
    {

        if(_isExtended)
        {
            _angleDelta = _initAngle - GetSignedAngle(_bodyCenter.transform, _palmFacingTransform);
            // Debug.Log(_angleDelta);


            _tubeShell.transform.position = _bodyCenter.position;
            _tubeShell.transform.rotation = Quaternion.Euler(
                0.0f,
                _bodyCenter.rotation.eulerAngles.y,
                0.0f);


            float offset = _initTubeYRot - _angleDelta;

            // TODO: Normalize angle value between 0 and 360
            if(offset < 0)
            {
                offset += 360;
            }

            if (_angleDelta > _snapThreshold)
            {
                Debug.Log("Threshold passed (positive)");
                _currentTubeComponentIndex = GetNext(_currentTubeComponentIndex);

                // TODO: I Think the problem here has to do with how the tweener handles
                // negative angles and 
                SnapTo(_currentTubeComponentIndex);
                _isExtended = false; // reset interaction

                _audioSource.Play();


            }
            else if (_angleDelta < (-1.0f * _snapThreshold))
            {
                Debug.Log("Threshold passed (negative)");
                _currentTubeComponentIndex = GetPrev(_currentTubeComponentIndex);
                SnapTo(_currentTubeComponentIndex);
                _isExtended = false; // reset interaction

                _audioSource.Play();

            }

            _tube.transform.localRotation = Quaternion.Euler(
                0.0f,
                offset,
                0.0f);

        } else
        {

            _tubeShell.transform.position = _bodyCenter.position;
            _tubeShell.transform.rotation = Quaternion.Euler(
                0.0f,
                _bodyCenter.rotation.eulerAngles.y,
                0.0f);
        }

    }

    private int GetNext(int current)
    {
        int nextIndex = current;
        if (current == 3)
        {
            nextIndex = 0;
        }
        else
        {
            nextIndex += 1;
        }
        return nextIndex;
    }

    private int GetPrev(int current)
    {
        int prevIndex = current;
        if (current == 0)
        {
            prevIndex = 3;
        }
        else
        {
            prevIndex -= 1;
        }
        return prevIndex;
    }

    private void SnapTo(int i)
    {

        // get the angle to snap to
        float angle = _snapRotations[i];

        _tube.SetRotation(new Vector3(0.0f, angle, 0.0f), _tweenTime, Easing.BounceOut);


    }


    private void RotateToNextLens()
    {
        // Rotate the chassis
        _tube.SetRelativeRotation(new Vector3(0.0f, 90.0f, 0.0f), _tweenTime, Easing.BounceOut);
        _audioSource.Play();


        // Fade Out the current one
        _tubeComponents[_currentTubeComponentIndex].SetOpacity(0.0f, _tweenTime);
        _tubeComponents[_currentTubeComponentIndex].SetPosition(new Vector3(0.0f, 0.0f, 0.0f), _tweenTime, Easing.QuarticIn);


        if (_currentTubeComponentIndex == 0)
        {
            _currentTubeComponentIndex = 3;
        }
        else
        {
            _currentTubeComponentIndex -= 1;
        }

        // Fade in the new component
        _tubeComponents[_currentTubeComponentIndex].SetOpacity(1.0f, _tweenTime);
        _tubeComponents[_currentTubeComponentIndex].SetPosition(new Vector3(0.0f, 6.0f, 0.0f), _tweenTime, Easing.CubicOut);

    }

    private float GetSignedAngle(Transform center, Transform satellite)
    {
        //Vector3 centerPos = center.position;
        //Vector3 satellitePos = new Vector3 (satellite.position.x, center.position.y, satellite.position.z);
        //Vector3 cornerPos = center.forward * satellitePos.z;

        //DrawLine(_lr1, centerPos, satellitePos);
        //DrawLine(_lr2, satellitePos, cornerPos);
        //DrawLine(_lr3, centerPos, cornerPos);

        Vector3 targetDir = new Vector3 (satellite.position.x, 0.0f, satellite.position.z) - new Vector3(center.position.x, 0.0f, center.position.z);
        // normalize the world rotation

        Vector3 forward = center.transform.forward;
        forward.y = 0.0f;

        float angleBetween = Vector3.SignedAngle(forward, targetDir, Vector3.up);
        // Debug.Log(angleBetween);

        return angleBetween;

    }

    private void DrawLine(LineRenderer lr, Vector3 point1, Vector3 point2)
    {
        lr.SetPosition(0, point1);
        lr.SetPosition(1, point2);
    }

}
