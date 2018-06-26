using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AF_Magnet : MonoBehaviour {

    bool inside;
    public Transform magnet;
    float radius = 5f;
    float force = 100f;
    void Start()
    {
        inside = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Magnet")
        {
            Debug.Log("Magnet Enter");
            inside = true;
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Magnet")
        {
            Debug.Log("Magnet Exit");
            inside = false;
        }

    }

    private void FixedUpdate()
    {
        if (inside)
        {
            Vector3 magnetField = magnet.position - transform.position;
            float index = (radius - magnetField.magnitude) / radius;
            GetComponent<Rigidbody>().AddForce(force * magnetField * index);
        }
    }

}
