using UnityEngine;
using System.Collections;

public partial class EnemyBehavior : MonoBehaviour {

    public GameObject mMyTarget = null;
    public static GameManager mGameManager = null;
    private const float kMySpeed = 20f;
    // All instances of Enemy shares this one WayPoint and EnemySystem
    static private EnemySpawnSystem sEnemySystem = null;
    static public void InitializeEnemySystem(EnemySpawnSystem s) { sEnemySystem = s; }

    private int mNumHit = 0;
    private const int kHitsToDestroy = 4;
    private const float kEnemyEnergyLost = 0.8f;
    //private const float mTurnRate = 0.5f;
    private const float mTurnRate = 0.03f / 60f;
    public static string GetWaypointState() { return ("Waypoints(" + (mGameManager.mIfRandom ? "Random" : "Sequence") + ")\n");}

    // Start is called before the first frame update

    void Start()
    {
        mMyTarget = sEnemySystem.GetRandomWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        PointAtPosition(mMyTarget.transform.localPosition, mTurnRate);
        transform.localPosition += kMySpeed * Time.smoothDeltaTime * transform.up;
        if (Vector3.Distance(transform.localPosition, mMyTarget.transform.localPosition) < 25f)
        {
            if (mGameManager.mIfRandom)
            {
                mMyTarget = sEnemySystem.GetRandomWaypoint();
            }
            else
            {
                mMyTarget = sEnemySystem.GetNextWaypoint(mMyTarget.name);
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
