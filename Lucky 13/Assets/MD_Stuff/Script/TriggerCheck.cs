using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour
{
    private GameObject rayHitObject, prevHitObject;
    private Animator eleAnimator;
    private Transform camTransform;
    private float eleTimer, tpTimer;
    private bool eleActiv, canRayCast, teleportPlayer, leftLiftButton, rayClicked, doTp;
    private Material curretMat;
    
    public Material thirteenMat;
    public GameObject Elevator, elevatorTPArea;

    void Start()
    {
        eleAnimator = GameObject.Find("ElevatorDoor").GetComponent<Animator>();
        camTransform = GetComponentInChildren<Camera>().GetComponent<Transform>();
        eleActiv = false;
        canRayCast = false;
        teleportPlayer = false;
        leftLiftButton = false;
        rayClicked = false;
        doTp = false;
    }


    void Update()
    {
        ClosingEleDoor();
        RayCasting();
        TeleportElevator();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "ElevatorCastArea")
            canRayCast = true;
        if (other.name == "ElevatorTrigger")
            canRayCast = true;
        if (other.name == "KeyTrigger")
            canRayCast = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "ElevatorTrigger" && Input.GetKey("e"))
        {
            OpeningDoor();
        }
        if (other.name == "KeyTrigger" && Input.GetKey("e"))
        {
            gameObject.transform.position = new Vector3(-19, 0, -2);
            canRayCast = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "ElevatorTrigger")
            canRayCast = false;
        if (other.name == "ElevatorCastArea")
            canRayCast = false;
        if (other.name == "KeyTrigger")
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
    void ForceEleDoorOpen()
    {
        eleAnimator.SetBool("OpenDoor", true);
    }
    void ForceEleDoorClose()
    {
        eleActiv = false;
        eleAnimator.SetBool("OpenDoor", false);
        tpTimer = Time.time + 2.5f;
        doTp = true;
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

            RayCastingAfterMath();
        }      
    }

    void RayCastingAfterMath()
    {
        if (rayHitObject.name == "ElevatorTrigger" || rayHitObject.name == "eleButton")
        {
            if (Input.GetMouseButton(0))
                OpeningDoor();
        }

        if (rayHitObject.name.Contains("LiftButton") && !leftLiftButton)
        {
            prevHitObject = rayHitObject;
            curretMat = rayHitObject.GetComponent<Renderer>().material;
            rayHitObject.GetComponent<Renderer>().material = thirteenMat;
            leftLiftButton = true;
            rayClicked = false;
        }

        if (rayHitObject.name.Contains("LiftButton") && Input.GetMouseButton(0))
        {
            rayHitObject.GetComponent<Renderer>().material = thirteenMat;
            ForceEleDoorClose();
            rayClicked = true;
            leftLiftButton = false;
        }

        if (rayHitObject.name.Contains("Key") && Input.GetMouseButton(0))
        {
            canRayCast = false;
            gameObject.transform.position = new Vector3(-19, 0, -2);
        }

        if (leftLiftButton && !rayHitObject.name.Contains("LiftButton") && !rayClicked)
        {
            leftLiftButton = false;
            prevHitObject.GetComponent<Renderer>().material = curretMat;
        }


    }

    void TeleportElevator()
    {
        if (Time.time > tpTimer && doTp)
        {
            doTp = false;
            gameObject.transform.parent = Elevator.transform;
            Elevator.transform.position = elevatorTPArea.transform.position;
            GameObject cObj = GameObject.Find("MainCamera");
            cObj.transform.parent = null;
            Elevator.transform.rotation = elevatorTPArea.transform.rotation;
            cObj.transform.parent = gameObject.transform;
            cObj.transform.position = new Vector3(0, 0.6f, 0);
            ForceEleDoorOpen();
            gameObject.transform.parent = null;
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 10, 10), "");
    }
}
