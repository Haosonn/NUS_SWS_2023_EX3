using UnityEngine;
using System.Collections;

public partial class EnemyBehavior : MonoBehaviour {

    public GameObject mMyTarget = null;
    private const float kMySpeed = 5f;
    // All instances of Enemy shares this one WayPoint and EnemySystem
    static private EnemySpawnSystem sEnemySystem = null;
    static public void InitializeEnemySystem(EnemySpawnSystem s) { sEnemySystem = s; }

    private int mNumHit = 0;
    private const int kHitsToDestroy = 4;
    private const float kEnemyEnergyLost = 0.8f;
    private const float mTurnRate = 0.5f;

    private static bool mIfRandom = false;

    public static string GetEnemyState() { return "Waypoints(" + mIfRandom ? "Random" : "Sequence" + ")\n";}

    // Start is called before the first frame update
    void Start()
    {
        //find the hero
        mMyTarget = GameObject.Find("Hero");  
    }

    // Update is called once per frame
    void Update()
    {
        PointAtPosition(mMyTarget.transform.localPosition, mTurnRate * Time.smoothDeltaTime);
        transform.localPosition += kMySpeed * Time.smoothDeltaTime * transform.up;
        //If close enough to the target, turn to next target
        if (Vector3.Distance(transform.localPosition, mMyTarget.transform.localPosition) < 0.5f)
        {
            if (mIfRandom)
            {
                mMyTarget = sEnemySystem.GetRandomWaypoint();
            }
            else
            {
                mMyTarget = sEnemySystem.GetNextWaypoint();
            }
        }
    }

    private void PointAtPosition(Vector3 p, float r)
    {
        Vector3 v = p - transform.localPosition;
        transform.up = Vector3.LerpUnclamped(transform.up, v, r);
    }

    #region Trigger into chase or die
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Emeny OnTriggerEnter");
        TriggerCheck(collision.gameObject);
    }

    private void TriggerCheck(GameObject g)
    {
        if (g.name == "Hero")
        {
            ThisEnemyIsHit();

        } else if (g.name == "Egg(Clone)")
        {
            mNumHit++;
            if (mNumHit < kHitsToDestroy)
            {
                Color c = GetComponent<Renderer>().material.color;
                c.a = c.a * kEnemyEnergyLost;
                GetComponent<Renderer>().material.color = c;
            } else
            {
                ThisEnemyIsHit();
            }
        }
    }

    private void ThisEnemyIsHit()
    {
        sEnemySystem.OneEnemyDestroyed();
        Destroy(gameObject);
    }

    #endregion
}
