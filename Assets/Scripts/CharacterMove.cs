using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMove : MonoBehaviour
{
    [SerializeField, Range(0.1f, 100f)] private float speed = 10f;
    [SerializeField, Range(0.1f, 100f)] private float speedBooster = 3f;
    [SerializeField, Range(0.1f, 600f)] private float rotationSpeed = 200f;
    [SerializeField, Range(0.1f, 100f)] private float maxAcceleration = 20f;
    [SerializeField, Range(0.1f, 300f)] public float JumpForce = 10f;

    public bool isGround;

    enum MoveState
    {
        Walk,
        Run
    }
    private MoveState moveState = MoveState.Walk;
    private Vector3 velocity;
    private Vector3 inputs;
    private Rigidbody rb;

    

    private float inputRotation, currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = speed;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
       GetInputs();
       CheckJump();
       Rotate();
       
    }

    private void CheckMoveState()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)
            && moveState == MoveState.Walk && isGround){
                moveState = MoveState.Run;
                return;
            }
        if(Input.GetKeyUp(KeyCode.LeftShift)
            && moveState == MoveState.Run){
                moveState = MoveState.Walk;
                return;
            }
    }

    private void CheckJump(){
        if(Input.GetKeyDown(KeyCode.Space) && isGround){
            rb.AddForce(Vector3.up * JumpForce, ForceMode.VelocityChange);
        }
    }
    
    private void Rotate(){
         float yRotation = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
         transform.Rotate(0, yRotation , 0);
    }

    private void GetInputs(){
         inputs = transform.right * Input.GetAxis("Horizontal") 
                            + transform.forward * Input.GetAxis("Vertical");

        CheckMoveState();
        currentSpeed = GetSpeed();
        
        inputRotation = Input.GetAxis("Mouse X");
    }

    private float GetSpeed(){
        return moveState switch{
            MoveState.Run => speed * speedBooster,
            MoveState.Walk => speed,
            _ => speed
        };
    }

    private void FixedUpdate()
    {
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        //------------- Оси X и Z-----------Ускорение------------------------Ось Y--------------------
        rb.velocity = ((inputs * currentSpeed * maxSpeedChange) + (Vector3.up * rb.velocity.y));
       
    }

    private void OnCollisionStay(Collision other) {
        foreach (var contact in other.contacts)
        {
            Debug.DrawLine(transform.position, contact.normal);
        }
        Vector3 normal = other.contacts[0].normal;
        
        
    }
    private void OnCollisionExit(Collision other) {
        isGround = false;
    }
}


