﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Enemy : MonoBehaviour
{

  [Header("Stats")]
  public int curHp;
  public int maxHp;
  public int scoreToGive;

  [Header("Movement")]
  public float moveSpeed;
  public float attackRange;
  public float yPathOffset;

  private List<Vector3> path;

  private Weapon weapon;
  private GameObject target;


  private void Start()
  {
    //get components
    weapon = GetComponent<Weapon>();
    target = FindObjectOfType<Player>().gameObject;

    InvokeRepeating("UpdatePath", 0.0f, 0.5f);

  }

  private void Update()
  {

    float dist = Vector3.Distance(transform.position, target.transform.position);

    if(dist <= attackRange)
    {
    
      if(weapon.CanShoot())
      {

        weapon.Shoot();

      }

    }
    else
    {

      ChaseTarget();

    }

    //track target
    Vector3 dir = (target.transform.position - transform.position).normalized;
    float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

    transform.eulerAngles = Vector3.up * angle;

  }

  private void ChaseTarget()
  {

    if(path == null || path.Count == 0)
    {

      return;

    }

    //move towards closest path
    transform.position = Vector3.MoveTowards(transform.position, path[0] + new Vector3(0, yPathOffset, 0), moveSpeed * Time.deltaTime);

    if(transform.position == path[0] + new Vector3(0, yPathOffset, 0))
    {

      path.RemoveAt(0);

    }

  }

  private void UpdatePath()
  {

    //calculate path to target
    NavMeshPath navMeshPath = new NavMeshPath();

    NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, navMeshPath);

    //save path as list
    path = navMeshPath.corners.ToList();
  }


  public void TakeDamage(int damage)
  {

    curHp -= damage;

    if(curHp <= 0)
    {

      Die();

    }

  }

  private void Die()
  {

    GameManager.instance.AddScore(scoreToGive);

    Destroy(gameObject);

  }

}
