using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cue : MonoBehaviour
{
  public Vector3 force;
  public float time;
  public Vector3 velocity;
  public Vector3 linearMomentum;
  public float mass;
  public float coeffficientOfFriction;
  public float gravity;
  public Vector3 distance;
  public Vector3 impulse;
  public float radius = 0.5f;
  public GameObject obj;
  public float totalTime;

  public bool collided;
  public Vector3 velocityAtCollision;
  // public GameObject obj = null;
  // Start is called before the first frame update
  void Start()
  {
    gravity = 9.8f;
    coeffficientOfFriction = 0.2f;
    // obj = GameObject.FindGameObjectsWithTag("Player");
    // force = new Vector3[obj.Length];
    // velocity = new Vector3[obj.Length];
    mass = 5;
    distance = obj.transform.position;
    collided = false;
    // distance = new Vector3[obj.Length];
    // linearMomentum =  new Vector3(10, 0, 0);
    // impulse = new Vector3[obj.Length];
    // collided = new bool[obj.Length];
    // distance = Vector3.zero;
    // for (int i = 0; i < obj.Length; i++)
    // {
    //   force = Vector3.zero;
    //   velocity = Vector3.zero;
    //   mass = 1f;
    //   distance = obj.transform.position;
    //   impulse = Vector3.zero;
    //   if (obj.name == "cue")
    //   {
    //     linearMomentum = new Vector3(10, 0, 0);
    //   }
    //   else
    //   {
    //     linearMomentum = new Vector3(0, 0, 0);
    //   }
    // }
    // Debug.Log(obj.Length);
  }

  // Update is called once per frame
  void Update()
  {
    totalTime += Time.deltaTime;
    if (totalTime > 2)
    {
      // STEP 1: Calculate force
      // force = previous force + friction force
      // only if body is in motion
      // velocity != 0
      // friction force = Unit vector opposite side of velocity
      var frictionForce = Vector3.zero;
      if (velocity.magnitude <= 0.01)
      {
        frictionForce = Vector3.zero;
        velocity = Vector3.zero;
      }
      else
      {
        frictionForce = (-velocity / velocity.magnitude) * coeffficientOfFriction * mass * gravity;
      }
      force = frictionForce;
      time = Time.deltaTime;
      // STEP 2: Integrate velocities
      distance = distance + velocity * time;
      // STEP 3: Perform collision
      // Debug.Log(obj.Length);
      var objs = GameObject.FindGameObjectsWithTag("Player");
      velocityAtCollision = velocity;

      for (int j = 0; j < objs.Length; j++)
      {
        if (objs[j] == obj)
        {
          continue;
        }
        else
        {
          var a = obj.transform.position;
          var b = objs[j].transform.position;
          if (Vector3.Distance(a, b) <= 2.2 * radius && !collided)
          {
            collided = true;
            var script = objs[j].GetComponent<Cue>();
            impulse = (script.velocityAtCollision - velocityAtCollision) * mass / 2;
            // impulse[j] = (velocity[j] - velocity) * mass / 2;
            Debug.Log("Colided");
            // break;
          }
          else if (Vector3.Distance(a, b) > 2.2 * radius)
          {
            collided = false;
            impulse = Vector3.zero;
          }
          // Debug.Log(Vector3.Distance(a, b));
        }

        // }
        // STEP 4: Update 
      }

      linearMomentum = linearMomentum + force * time + impulse;
      // Step 5: Calculate velocities for next step
      velocity = linearMomentum / (float)mass;
      obj.transform.position = distance;

    }

  }
}
