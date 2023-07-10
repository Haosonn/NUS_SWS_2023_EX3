using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private int mNumHit = 0;
    private const int kHitsToDestroy = 4;
    private const float kWaypointEnergyLost = 0.25f;


    private void Start()
    {
    }

    private void Update()
    {
    }

    private void PointAtPosition(Vector3 p, float r)
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Waypoint OnTriggerEnter2D");
        TriggerCheck(collision.gameObject);
    }

    private void TriggerCheck(GameObject g)
    {
        Debug.Log("Waypoint OnTriggerEnter");

        if (g.name == "Egg(Clone)")
        {
            Debug.Log("Egg hit");
            mNumHit++;
            if (mNumHit < kHitsToDestroy)
            {
                Color c = GetComponent<Renderer>().material.color;
                c.a -= kWaypointEnergyLost;
                GetComponent<Renderer>().material.color = c;
            }
            else
            {
                ThisWaypointIsHit();
            }
        }
    }

    // reset the Waypoint
    private void ThisWaypointIsHit()
    {
        mNumHit = 0;
        
    }
}