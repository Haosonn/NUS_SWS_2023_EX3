using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager sTheGlobalBehavior = null;

    public Text mGameStateEcho = null;  // Defined in UnityEngine.UI
    public HeroBehavior mHero = null;

    public bool mIfRandom = false;
    private EnemySpawnSystem mEnemySystem = null;

    private CameraSupport mMainCamera;

    private void Start()
    {
        GameManager.sTheGlobalBehavior = this;  // Singleton pattern
        EnemyBehavior.mGameManager = this;
        Debug.Assert(mHero != null);

        mMainCamera = Camera.main.GetComponent<CameraSupport>();
        Debug.Assert(mMainCamera != null);

        Bounds b = mMainCamera.GetWorldBound();
        mEnemySystem = new EnemySpawnSystem(b.min, b.max);
    }

	void Update () {
        EchoGameState(); // always do this

        if (Input.GetKeyDown(KeyCode.J)) 
            mIfRandom = !mIfRandom;
        if (Input.GetKey(KeyCode.Q))
            Application.Quit();
    }


    #region Bound Support
    public CameraSupport.WorldBoundStatus CollideWorldBound(Bounds b) { return mMainCamera.CollideWorldBound(b); }
    #endregion 

    private void EchoGameState()
    {
        mGameStateEcho.text =  EnemyBehavior.GetWaypointState() + " " + mHero.GetHeroState();
    }
}