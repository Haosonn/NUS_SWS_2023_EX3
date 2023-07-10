using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager sTheGlobalBehavior = null;

    public Text mGameStateEcho = null; // Defined in UnityEngine.UI
    public HeroBehavior mHero = null;

    public bool mIfRandom = false;
    public bool mIfHidden = false;
    private EnemySpawnSystem mEnemySystem = null;

    private CameraSupport mMainCamera;

    private void Start() {
        GameManager.sTheGlobalBehavior = this; // Singleton pattern
        EnemyBehavior.mGameManager = this;
        Waypoint.mGameManager = this;
        Debug.Assert(mHero != null);

        mMainCamera = Camera.main.GetComponent<CameraSupport>();
        Debug.Assert(mMainCamera != null);

        Bounds b = mMainCamera.GetWorldBound();
        mEnemySystem = new EnemySpawnSystem(b.min, b.max);
        string[] arr = { "A", "B", "C", "D", "E", "F" };
        for (int i = 0; i < arr.Length; i++) {
            mEnemySystem.mTargets[i] = GameObject.Find(arr[i]);
            mEnemySystem.mTargets[i].SetActive(true);
        }
    }

    void Update() {
        EchoGameState(); // always do this

        if (Input.GetKeyDown(KeyCode.J))
            mIfRandom = !mIfRandom;
        if (Input.GetKey(KeyCode.Q))
            Application.Quit();
        if (Input.GetKeyDown(KeyCode.H)) {
            mIfHidden = !mIfHidden;
            if (mIfHidden) {
                string[] arr = { "A", "B", "C", "D", "E", "F" };
                for (int i = 0; i < arr.Length; i++) {
                    mEnemySystem.mTargets[i].SetActive(false);
                }
            }
            else {
                for (int i = 0; i < mEnemySystem.mTargets.Length; i++) {
                    mEnemySystem.mTargets[i].SetActive(true);
                }
            }
        }
    }



    #region Bound Support

    public CameraSupport.WorldBoundStatus CollideWorldBound(Bounds b) {
        return mMainCamera.CollideWorldBound(b);
    }

    #endregion

    private void EchoGameState() {
        mGameStateEcho.text = EnemyBehavior.GetWaypointState() + " " + mHero.GetHeroState();
    }
}