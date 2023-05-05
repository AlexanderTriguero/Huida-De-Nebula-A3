using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    CharacterController characterController;
    [SerializeField] float movespeed=4f;
    [SerializeField] LayerMask layerMaskAimingDetection;
    Animator animator;
    PlayerShooting playerShooting;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        playerShooting = GetComponent<PlayerShooting>();
    }

    // Update is called once per frame

    private float speedY;

    private float gravity = -9.8f;

    void Update()
    {
        if (!animator.GetBool("IsDead")) 
        {
            if (!playerShooting.IsSwinging())
            {
                UpdateMovement();
                UpdateOrientation();
            }
            
        }
    }

    Vector3 movementFromInput;
    Vector3 movementFromCamera;
    public void UpdateMovement()
    {
        movementFromInput = Vector3.zero;
        if (Input.GetKey(KeyCode.A)) { 
            movementFromInput += Vector3.left;
        }
        if (Input.GetKey(KeyCode.W)) { movementFromInput += Vector3.forward; }
        if (Input.GetKey(KeyCode.S)) { movementFromInput += Vector3.back; }
        if (Input.GetKey(KeyCode.D)) { movementFromInput += Vector3.right; }

        movementFromCamera = Camera.main.transform.TransformDirection(movementFromInput);
        movementFromCamera = Vector3.ProjectOnPlane(movementFromCamera, Vector3.up);
        movementFromCamera.Normalize();

        speedY += gravity * Time.deltaTime;
        movementFromCamera.y = speedY;
        characterController.Move(movementFromCamera * movespeed * Time.deltaTime);
        if (characterController.isGrounded) { speedY = 0; }
        
    }

    
    

    [SerializeField] bool orientatedToCamera;
    public void UpdateOrientation()
    {
        Vector3 desiredForward=Vector3.zero;
        bool performOrientation=false;
        if (orientatedToCamera)
        {
            if (movementFromCamera.sqrMagnitude > (0.01f * 0.01f))
            {
                //Girar el forward en función de la posición de la camara
                desiredForward = Camera.main.transform.forward;
                performOrientation = true;
            }
        }else{ 
            //Orientación hacia el cursor
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            if (Physics.Raycast(ray, out hit, Mathf.Infinity,layerMaskAimingDetection)) {
                desiredForward = hit.point - transform.position;
                performOrientation = true;
            }
        }

        if (performOrientation)
        {
            desiredForward = Vector3.ProjectOnPlane(desiredForward, Vector3.up);
            desiredForward.Normalize();

            Quaternion desiredRotation = Quaternion.LookRotation(desiredForward, Vector3.up);
            Quaternion currentRotation = transform.rotation;
            transform.rotation = Quaternion.Lerp(currentRotation, desiredRotation, 0.1f);
        }
    }
}

