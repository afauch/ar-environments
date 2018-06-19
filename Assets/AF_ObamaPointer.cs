using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap.Unity.Interaction;
using Leap.Unity.Attachments;

public class AF_ObamaPointer : MonoBehaviour {

    public AttachmentPointBehaviour _thumbProximalJoint;
    public AttachmentPointBehaviour _indexMiddleJoint;

    private LineRenderer _lr;

	// Use this for initialization
	void Start () {

        _lr = GetComponent<LineRenderer>();
		
	}
	
	// Update is called once per frame
	void Update () {

        Debug.DrawRay(_thumbProximalJoint.transform.position, _indexMiddleJoint.transform.position - _thumbProximalJoint.transform.position, Color.green);
        DrawPointer();

	}

    void DrawPointer ()
    {

        // Raycast to find the right point
        RaycastHit hit;

        Vector3[] positions;

        if(Physics.Raycast(_indexMiddleJoint.transform.position, _indexMiddleJoint.transform.position - _thumbProximalJoint.transform.position, out hit, Mathf.Infinity))
        {

            Debug.Log("hit");

            positions = new Vector3[]
            {
                _thumbProximalJoint.transform.position,
                _indexMiddleJoint.transform.position,
                hit.point
            };
        } else
        {
            positions = new Vector3[]
            {
                _thumbProximalJoint.transform.position,
                _indexMiddleJoint.transform.position
            };
        }

        _lr.SetPositions(positions);


    }


}
