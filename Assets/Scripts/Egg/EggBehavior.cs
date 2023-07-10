using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehavior : MonoBehaviour
{
    static private GreenArrowBehavior sGreenArrow = null;
    static public void SetGreenArrow(GreenArrowBehavior g) { sGreenArrow = g; }
    private const float kEggSpeed = 40f;

    void DestroyEgg()
    {
        Destroy(transform.gameObject);
        sGreenArrow.OneLessEgg();
    }
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * (kEggSpeed * Time.smoothDeltaTime);
        CameraSupport s = Camera.main.GetComponent<CameraSupport>();  // Try to access the CameraSupport component on the MainCamera
        if (s != null)   // if main camera does not have the script, this will be null
        {
            Bounds myBound = GetComponent<Renderer>().bounds;  // this is the bound of the collider defined on GreenUp
            CameraSupport.WorldBoundStatus status = s.CollideWorldBound(myBound);
            if (status != CameraSupport.WorldBoundStatus.Inside)
                DestroyEgg();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroyEgg();
    }

}
