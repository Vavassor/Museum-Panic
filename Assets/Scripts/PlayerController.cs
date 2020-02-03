using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum HoldState
    {
        NotHolding,
        Moving,
        Rotating,
    }

    public Camera camera;
    public List<AudioClip> grabSounds;
    public float heldObjectMinY = 0.0f;

    private AudioSource audioSource;
    private HoldState holdState;
    private Vector3 mouseOffset;
    private Rigidbody rigidbodyHeld;

    public void LetGoOfObject()
    {
        if (rigidbodyHeld)
        {
            rigidbodyHeld.useGravity = true;
            rigidbodyHeld.constraints = RigidbodyConstraints.FreezeAll;
        }
        rigidbodyHeld = null;
        holdState = HoldState.NotHolding;
        Cursor.lockState = CursorLockMode.None;
    }

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        switch (Game.Instance.GetState())
        {
            case GameState.Playing:
            {
                UpdatePlaying();
                break;
            }
            case GameState.RetryMenu:
            {
                break;
            }
        }
    }

    private Vector3 GetMousePositionInGamePlane()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.forward, -transform.position.z);
        float enter = 0.0f;
        plane.Raycast(ray, out enter);
        Vector3 hitPoint = ray.GetPoint(enter);
        return hitPoint;
    }

    private void GrabObject(Rigidbody rigidbody)
    {
        mouseOffset = Vector3.zero;
        rigidbodyHeld = rigidbody;
        rigidbodyHeld.useGravity = false;
        rigidbodyHeld.constraints = RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;
        holdState = HoldState.Moving;
        SoundUtils.PlaySoundNonrepeating(audioSource, grabSounds);
    }

    private Vector3 MoveHeldObject()
    {
        Vector3 movePoint = GetMousePositionInGamePlane() + mouseOffset;
        movePoint.y = Mathf.Max(movePoint.y, heldObjectMinY);
        Vector3 force = 30.0f * (movePoint - rigidbodyHeld.transform.position);
        rigidbodyHeld.AddForce(force, ForceMode.Force);
        return movePoint;
    }

    private void RotateObject()
    {
        float x = 10.0f * Input.GetAxis("Mouse X");
        float z = 10.0f * Input.GetAxis("Mouse Y");
        Vector3 torque = new Vector3(x, 0.0f, z);
        rigidbodyHeld.AddRelativeTorque(torque);
    }

    private void StartMovingAgain()
    {
        Vector3 mousePosition = GetMousePositionInGamePlane();
        mouseOffset = rigidbodyHeld.transform.position - mousePosition;

        rigidbodyHeld.useGravity = false;
        rigidbodyHeld.angularVelocity = Vector3.zero;
        rigidbodyHeld.constraints = RigidbodyConstraints.FreezePositionZ |
                RigidbodyConstraints.FreezeRotationX |
                RigidbodyConstraints.FreezeRotationY |
                RigidbodyConstraints.FreezeRotationZ;
        rigidbodyHeld.gameObject.tag = "Untagged";

        holdState = HoldState.Moving;
        Cursor.lockState = CursorLockMode.None;
    }

    private void StartRotation()
    {
        holdState = HoldState.Rotating;
        rigidbodyHeld.constraints = RigidbodyConstraints.FreezePositionX |
            RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezePositionZ;
        rigidbodyHeld.gameObject.tag = "Sticky";
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void TryToGrabObject()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Rigidbody rigidbody = hit.rigidbody;
            if (rigidbody && rigidbody.gameObject.tag != "Stuck")
            {
                GrabObject(rigidbody);
            }
        }
    }

    private void UpdatePlaying()
    {
        switch (holdState)
        {
            case HoldState.Moving:
            {
                Vector3 heldPoint = MoveHeldObject();
                if (Input.GetMouseButtonDown(0))
                {
                    StartRotation();
                }
                break;
            }
            case HoldState.NotHolding:
            {
                if (Input.GetMouseButtonDown(0))
                {
                    TryToGrabObject();
                }
                break;
            }
            case HoldState.Rotating:
            {
                RotateObject();
                if (Input.GetMouseButtonUp(0))
                {
                    StartMovingAgain();
                }
                break;
            }
        }
    }
}
