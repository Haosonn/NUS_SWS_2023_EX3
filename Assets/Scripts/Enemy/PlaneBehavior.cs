using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBehavior: MonoBehaviour
{
    // Start is called before the first frame update
    private static int sElim = 0;
    public int mHealth = 100;
    void DestroyPlane()
    {
        Destroy(transform.gameObject);
        sElim++;
    }
    
    public static void SpawnPlane() {
        CameraSupport s = Camera.main.GetComponent<CameraSupport>();  // Try to access the CameraSupport component on the MainCamera if (s != null)   // if main camera does not have the script, this will be null
        GameObject newPlane = Instantiate(Resources.Load("Prefabs/Plane") as GameObject);
        Vector3 p = newPlane.transform.localPosition;
        p.x = s.GetWorldBound().min.x + (0.9f * Random.value + 0.05f) * s.GetWorldBound().size.x;
        p.y = s.GetWorldBound().min.y + (0.9f * Random.value + 0.05f) * s.GetWorldBound().size.y;
        newPlane.transform.localPosition = p;
    }
    void Start()
    {
        mHealth = 100;
        Debug.Log("Plane: Started");
    }

    // Update is called once per frame
    void Update()
    {
        if (mHealth <= 0)
        {
            DestroyPlane();
            SpawnPlane();
        }
    }

    private void UpdateColor()
    {
        SpriteRenderer s = GetComponent<SpriteRenderer>();
        Color c = s.color;
        c.a *= 0.8f;
        s.color = c;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "GreenUp") {
            mHealth -= 100;
        }
        else {
            mHealth -= 25;
            UpdateColor();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Plane: OnTriggerStay2D");
        UpdateColor();
    }

    public static string ElimStatus()
    {
        return "Eliminated: " + sElim;
    }
}
