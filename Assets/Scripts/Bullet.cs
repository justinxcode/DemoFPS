﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

  public int damage;
  public float lifetime;
  private float shootTime;

  public GameObject hitParticle;

  private void OnEnable()
  {

    shootTime = Time.time;

  }

  private void Update()
  {
  
    if(Time.time - shootTime >= lifetime)
    {

      gameObject.SetActive(false);

    }

  }

  private void OnTriggerEnter(Collider other)
  {
    
    //did it hit player
    if(other.CompareTag("Player"))
    {

      other.GetComponent<Player>().TakeDamage(damage);

    }
    else if (other.CompareTag("Enemy"))
    {

      other.GetComponent<Enemy>().TakeDamage(damage);

    }

    //create hit particle
    GameObject obj = Instantiate(hitParticle, transform.position, Quaternion.identity);

    Destroy(obj, 0.5f);

    //disable bullet
    gameObject.SetActive(false);

  }

}
