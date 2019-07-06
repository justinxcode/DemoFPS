using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

  public ObjectPool bulletPool;
  public Transform muzzle;

  public int curAmmo;
  public int maxAmmo;
  public bool infiniteAmmo;

  public float bulletSpeed;

  public float shootRate;
  private float lastShootTime;
  public bool isPlayer;

  public AudioClip shootSfx;
  private AudioSource audioSource;

  private void Awake()
  {
    //is attached to player
    if(GetComponent<Player>())
    {
      isPlayer = true;
    }

    audioSource = GetComponent<AudioSource>();

  }

  public bool CanShoot()
  {

    if(Time.time - lastShootTime >= shootRate)
    {

      if(curAmmo > 0 || infiniteAmmo == true)
      {

        return true;

      }

    }

    return false;

  }

  public void Shoot()
  {

    lastShootTime = Time.time;
    curAmmo--;

    if(isPlayer)
    {

      GameUI.instance.UpdateAmmoText(curAmmo, maxAmmo);

    }

    audioSource.PlayOneShot(shootSfx);

    GameObject bullet = bulletPool.GetObject();

    bullet.transform.position = muzzle.position;
    bullet.transform.rotation = muzzle.rotation;

    //set velocity
    bullet.GetComponent<Rigidbody>().velocity = muzzle.forward * bulletSpeed;

  }

}
