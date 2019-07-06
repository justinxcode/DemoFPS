using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

  [Header("Stats")]
  public int curHp;
  public int maxHp;

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
  private Weapon weapon;

  private void Awake()
  {
    //get components
    cam = Camera.main;
    rig = GetComponent<Rigidbody>();
    weapon = GetComponent<Weapon>();

    //disable mouse
    Cursor.lockState = CursorLockMode.Locked;
  }

  private void Start()
  {
    //initialize UI
    GameUI.instance.UpdateHealthBar(curHp, maxHp);
    GameUI.instance.UpdateScoreText(0);
    GameUI.instance.UpdateAmmoText(weapon.curAmmo, weapon.maxAmmo);
  }

  private void Update()
  {

    //don't update if paused
    if(GameManager.instance.gamePaused == true)
    {

      return;

    }

    Move();
    CamLook();

    if(Input.GetButtonDown("Jump"))
    {

      TryJump();

    }

    if(Input.GetButton("Fire1"))
    {

      if(weapon.CanShoot())
      {
        weapon.Shoot();
      }

    }
  }

  //move horizontally based on movement inputs
  private void Move()
  {
    //get x a z input
    float x = Input.GetAxis("Horizontal") * moveSpeed;
    float z = Input.GetAxis("Vertical") * moveSpeed;

    Vector3 dir = (transform.right * x) + (transform.forward * z);
    dir.y = rig.velocity.y;

    //apply velocity
    rig.velocity = dir;

  }

  private void CamLook()
  {

    float y = Input.GetAxis("Mouse X") * lookSensitivity;
    rotX += Input.GetAxis("Mouse Y") * lookSensitivity;


    rotX = Mathf.Clamp(rotX, minLookX, maxLookX);

    cam.transform.localRotation = Quaternion.Euler(-rotX, 0, 0);
    transform.eulerAngles += Vector3.up * y;
  }

  private void TryJump()
  {

    Ray ray = new Ray(transform.position, Vector3.down);

    if(Physics.Raycast(ray, 1.1f))
    {

      rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

    }

  }

  public void TakeDamage(int damage)
  {

    curHp -= damage;

    GameUI.instance.UpdateHealthBar(curHp, maxHp);

    if(curHp <= 0)
    {

      Die();

    }

  }

  private void Die()
  {

    GameManager.instance.LoseGame();

  }

  public void GiveHealth(int amountToGive)
  {

    curHp = Mathf.Clamp(curHp + amountToGive, 0, maxHp);

    GameUI.instance.UpdateHealthBar(curHp, maxHp);

  }

  public void GiveAmmo(int amountToGive)
  {

    weapon.curAmmo = Mathf.Clamp(weapon.curAmmo + amountToGive, 0, weapon.maxAmmo);

    GameUI.instance.UpdateAmmoText(weapon.curAmmo, weapon.maxAmmo);

  }

}
