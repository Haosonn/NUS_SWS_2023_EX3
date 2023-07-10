using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenArrowBehavior : MonoBehaviour
{
    public float mHeroSpeed = 20f;
    public float mHeroAccel = 10f;
    public float mHeroRotateSpeed = 90f / 2f; // 90-degrees in 2 seconds
    private float mInvokeTime = 0f;
    private float mTimeToNextEgg = 0.2f;
    public GameManager mGameManager = null; 
    private int mTotalEggCount = 0;

    private int mTotalTouchCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        mInvokeTime =mTimeToNextEgg;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(transform.forward, mHeroRotateSpeed * Time.smoothDeltaTime);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(transform.forward, -mHeroRotateSpeed * Time.smoothDeltaTime);
        if (mGameManager.mControlMode == ControlMode.Mouse) {
            // Move this object to mouse position
            Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            p.z = 0f;
            transform.localPosition = p;
        }
        else {
            Vector3 p = transform.localPosition;
            p += ((mHeroSpeed * Time.smoothDeltaTime) * transform.up);
            if (Input.GetKey(KeyCode.W))
                mHeroSpeed += mHeroAccel * Time.smoothDeltaTime;
            if (Input.GetKey(KeyCode.S))
                mHeroSpeed -= mHeroAccel * Time.smoothDeltaTime;
            CameraSupport s = Camera.main.GetComponent<CameraSupport>();  // Try to access the CameraSupport component on the MainCamera
            if (s != null)   // if main camera does not have the script, this will be null
            {
                Bounds myBound = GetComponent<Renderer>().bounds;  // this is the bound of the collider defined on GreenUp
                CameraSupport.WorldBoundStatus status = s.CollideWorldBound(myBound);

                if (status != CameraSupport.WorldBoundStatus.Inside)
                {
                    Debug.Log("Touching the world edge: " + status);
                    if (p.x < s.GetWorldBound().min.x)
                        p.x = s.GetWorldBound().max.x;
                    if (p.x > s.GetWorldBound().max.x)
                        p.x = s.GetWorldBound().min.x;
                    if (p.y < s.GetWorldBound().min.y)
                        p.y = s.GetWorldBound().max.y;
                    if (p.y > s.GetWorldBound().max.y)
                        p.y = s.GetWorldBound().min.y;
                }
            }
            transform.localPosition = p;
        }

        // Now spawn an egg when space bar is hit
        if (Input.GetKey(KeyCode.Space))
        {
            mInvokeTime += Time.smoothDeltaTime;
            if (mInvokeTime < mTimeToNextEgg)
                return;
            mInvokeTime = 0f;
            GameObject e = Instantiate(Resources.Load("Prefabs/Egg") as GameObject); // Prefab MUST BE locaed in Resources/Prefab folder!
            e.transform.localPosition = transform.localPosition;
            e.transform.localRotation = transform.localRotation;
            mTotalEggCount++;
            Debug.Log("Eggs on screen: " + mTotalEggCount);
        }
    }
    
    public void OneLessEgg() { mTotalEggCount--;  }

    public string EggStatus() { return "Eggs on screen: " + mTotalEggCount; }
    public string TouchStatus() { return "Touching: " + mTotalTouchCount; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Arrow: OnTriggerEnter2D");
        mTotalTouchCount++;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Arrow: OnTriggerStay2D");
        // collision.gameObject.mHealth -= 100;
    }
}
