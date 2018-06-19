using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap.Unity.Interaction;
using Leap.Unity.Attachments;

public class AF_IndexPointer : MonoBehaviour
{

    public AttachmentPointBehaviour _indexKnuckle;
    public AttachmentPointBehaviour _indexTip;
    public GameObject _cursor;
    public LayerMask _layers;

    private LineRenderer _lr;

    // Use this for initialization
    void Start()
    {

        _lr = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

        Debug.DrawRay(_indexKnuckle.transform.position, _indexTip.transform.position - _indexKnuckle.transform.position, Color.green);
        DrawPointer();

    }

    void DrawPointer()
    {

        // Raycast to find the right point
        RaycastHit hit;
        Vector3[] positions;

        if (Physics.Raycast(_indexTip.transform.position, _indexTip.transform.position - _indexKnuckle.transform.position, out hit, Mathf.Infinity, _layers))
        {

            positions = new Vector3[]
            {
                _indexKnuckle.transform.position,
                _indexTip.transform.position,
                hit.point
            };

            _cursor.transform.position = hit.point;

        }
        else
        {
            positions = new Vector3[]
            {
                _indexKnuckle.transform.position,
                _indexTip.transform.position
            };
        }

        _lr.SetPositions(positions);


    }


}
