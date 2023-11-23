using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigInteractions : MonoBehaviour
{

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    public RoomController _mainRoomController;

    /*
    private AudioSource _audioSource;
    public bool resetPosition = false;
    public bool regenerateRoom = false;
    */
    
    // Start is called before the first frame update
    void Start()
    {
        _startPosition = gameObject.transform.position;
        _startRotation = gameObject.transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "End Curtain")
        {
            other.gameObject.GetComponent<AudioSource>().Play();
            gameObject.transform.position = _startPosition;
            gameObject.transform.rotation = _startRotation;
            _mainRoomController.GenerateRoom();
        } else if (other.gameObject.name == "Through Curtain" || other.gameObject.name == "Start Curtain")
        {
            other.gameObject.GetComponent<AudioSource>().Play();
        }
        else if (other.gameObject.name == "Transition Cube")
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddRelativeForce(Vector3.forward * 5);
            rb.isKinematic = true;
        }
    }
}
