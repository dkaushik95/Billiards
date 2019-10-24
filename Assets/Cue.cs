using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cue : MonoBehaviour
{
  public Vector3[] force;
  public float time;
  public Vector3[] velocity;
  public Vector3[] linearMomentum;
  public float[] mass;
  public float coeffficientOfFriction;
  public float gravity;
  public Vector3[] distance;
  public Vector3[] impulse;
  public float radius = 0.5f;
  public GameObject[] objs = new GameObject[2];
  private Vector3 offset;
  public GameObject camera;
  public GameObject cue;
  public float totalTime;
  // public GameObject obj = null;
  // Start is called before the first frame update
  void Start()
  {
    offset = camera.transform.position - cue.transform.position;
    gravity = 9.8f;
    coeffficientOfFriction = 0.4f;
    objs = GameObject.FindGameObjectsWithTag("Player");
    force = new Vector3[objs.Length];
    velocity = new Vector3[objs.Length];
    mass = new float[objs.Length];
    distance = new Vector3[objs.Length];
    linearMomentum = new Vector3[objs.Length];
    impulse = new Vector3[objs.Length];
    // distance = Vector3.zero;
    for (int i = 0; i < objs.Length; i++)
    {
      force[i] = Vector3.zero;
      velocity[i] = Vector3.zero;
      mass[i] = 1f;
      distance[i] = objs[i].transform.position;
      impulse[i] = Vector3.zero;
      if (objs[i].name == "cue")
      {
        linearMomentum[i] = new Vector3(20, 0, 0);
      }
      else
      {
        linearMomentum[i] = new Vector3(0, 0, 0);
      }
    }
    Debug.Log(objs.Length);
  }

  // Update is called once per frame
  void Update()
  {
    totalTime += Time.deltaTime;
    if (totalTime > 2)
    {
      camera.transform.position = cue.transform.position + offset;

      for (int i = 0; i < objs.Length; i++)
      {
        // STEP 1: Calculate force
        // force = previous force + friction force
        // only if body is in motion
        // velocity != 0
        // friction force = Unit vector opposite side of velocity
        var frictionForce = Vector3.zero;
        if (velocity[i].magnitude <= 0.01)
        {
          frictionForce = Vector3.zero;
          velocity[i] = Vector3.zero;
        }
        else
        {
          frictionForce = (-velocity[i] / velocity[i].magnitude) * coeffficientOfFriction * mass[i] * gravity;
        }
        force[i] = frictionForce;
        time = Time.deltaTime;
        // STEP 2: Integrate velocities
        distance[i] = distance[i] + velocity[i] * time;
        // STEP 3: Perform collision
        // Debug.Log(objs.Length);
        for (int j = 0; j < objs.Length; j++)
        {
          if (i == j)
          {
            continue;
          }
          else
          {
            var a = objs[i].transform.position;
            var b = objs[j].transform.position;
            if (Vector3.Distance(a, b) < 2 * radius)
            {
              impulse[i] = (velocity[j] - velocity[i]);
              // impulse[j] = (velocity[j] - velocity[i]) * mass[i] / 2;
              Debug.Log("Colided");
              // break;
            }

            else
            {
              impulse[i] = Vector3.zero;
            }
            // Debug.Log(Vector3.Distance(a, b));
          }

          // }
          // STEP 4: Update 
        }

        linearMomentum[i] = linearMomentum[i] + force[i] * time + impulse[i];
        // Step 5: Calculate velocities for next step
        velocity[i] = linearMomentum[i] / (float)mass[i];
        objs[i].transform.position = distance[i];
      }
    }

  }
}
