using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  [Header("Movement")]
  public float moveSpeed;          //movement speed in units per second
  public float jumpForce;          //force applied upward

  [Header("Camera")]
  public float lookSensitivity;    //mouse look sensitivity
  public float maxLookX;           //lowest down we can look
  public float minLookX;           //highest up we can look

  private float rotX;              //current x rotation of camera
  private Camera cam;              //camera var
  private Rigidbody rig;           //body var

  void Awake()
  {
    //get components
    cam = Camera.main;
    rig = GetComponent<Rigidbody>();
  }

  void Update()
  {
    Move();

    if(Input.GetButtonDown("Jump"))
    {

      TryJump();

    }

    CamLook();
  }

  //move horizontally based on movement inputs
  void Move()
  {
    //get x a z input
    float x = Input.GetAxis("Horizontal") * moveSpeed;
    float z = Input.GetAxis("Vertical") * moveSpeed;

    Vector3 dir = (transform.right * x) + (transform.forward * z);
    dir.y = rig.velocity.y;

    //apply velocity
    rig.velocity = dir;

  }

  void CamLook()
  {

    float y = Input.GetAxis("Mouse X") * lookSensitivity;
    rotX += Input.GetAxis("Mouse Y") * lookSensitivity;


    rotX = Mathf.Clamp(rotX, minLookX, maxLookX);

    cam.transform.localRotation = Quaternion.Euler(-rotX, 0, 0);
    transform.eulerAngles += Vector3.up * y;
  }

  void TryJump()
  {

    Ray ray = new Ray(transform.position, Vector3.down);

    if(Physics.Raycast(ray, 1.1f))
    {

      rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

    }

  }

}
