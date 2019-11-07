using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour
{
    private GameObject rayHitObject;
    private Animator eleAnimator;
    private Transform camTransform;
    private float eleTimer;
    private bool eleActiv, canRayCast;

    public Material thirteenMat;

    void Start()
    {
        eleAnimator = GameObject.Find("ElevatorDoor").GetComponent<Animator>();
        camTransform = GetComponentInChildren<Camera>().GetComponent<Transform>();
        eleActiv = false;
        canRayCast = false;
        
    }


    void Update()
    {
        ClosingEleDoor();
        RayCasting();
        if (canRayCast)
        {
            Debug.LogWarning("CanCast");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "ElevatorCastArea")
            canRayCast = true;
        if (other.name == "ElevatorTrigger")
            canRayCast = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "ElevatorTrigger" && Input.GetKey("e"))
        {
            OpeningDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "ElevatorTrigger")
            canRayCast = false;
        if (other.name == "ElevatorCastArea")
            canRayCast = false;
    }

    void OpeningDoor()
    {
        eleAnimator.SetBool("OpenDoor", true);
        eleTimer = Time.time + 5;
        eleActiv = true;
    }

    void ClosingEleDoor()
    {
        if (eleActiv)
        {
            if (Time.time > eleTimer)
            {
                eleActiv = false;
                eleAnimator.SetBool("OpenDoor", false);
            }
        }
    }
    void ForceEleDoorClose()
    {
        eleActiv = false;
        eleAnimator.SetBool("OpenDoor", false);
    }

    void RayCasting()
    {
        if (canRayCast)
        {
            RaycastHit hit;
            if (Physics.Raycast(camTransform.position, camTransform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(camTransform.position, camTransform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.LogWarning(hit.collider.name);
                rayHitObject = hit.collider.gameObject;
            }
        }
        if (rayHitObject)
            RayCastingAfterMath();
    }

    void RayCastingAfterMath()
    {
        if (rayHitObject.name == "ElevatorTrigger" || rayHitObject.name == "eleButton")
        {
            if (Input.GetMouseButton(0))
                OpeningDoor();
        }
        
        if (rayHitObject.name.Contains("LiftButton") && Input.GetMouseButton(0))
        {
            rayHitObject.GetComponent<Renderer>().material = thirteenMat;
            ForceEleDoorClose();
            //teleport && open door 
        }

    }

    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 10, 10), "");
    }
}
