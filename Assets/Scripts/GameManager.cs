using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Required to work with UI, e.g., Text

public enum ControlMode :ushort {Keyboard = 0, Mouse = 1};

public class GameManager : MonoBehaviour
{
    public static GameManager sTheGlobalBehavior = null; // Single pattern

    public ControlMode mControlMode = ControlMode.Mouse; // default

    public GreenArrowBehavior mHero = null;  // must set in the editor

    // for display egg count
    public Text mInfo = null;

    public string GetInfo() {
        return "Control Mode: " + mControlMode.ToString() + "\n"
            + mHero.EggStatus() + "\n"
            + mHero.TouchStatus() + "\n"
            + PlaneBehavior.ElimStatus() + "\n"
            + "Plane numbers: 10";
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.sTheGlobalBehavior = this;  // Singleton pattern
        Debug.Assert(mInfo != null);    // Assume setting in the editor!
        Debug.Assert(mHero != null);
        mControlMode = ControlMode.Mouse;
        mInfo.text = GetInfo();
        // Connect up everyone who needs to know about each other
        EggBehavior.SetGreenArrow(mHero);
        for (int i = 0; i < 10; i++) {
            PlaneBehavior.SpawnPlane();
        }
        // Notice the symantics: this is a call to class method (NOT instance method)
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            mControlMode = (mControlMode == ControlMode.Keyboard) ? ControlMode.Mouse : ControlMode.Keyboard;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //quit
            Application.Quit();
        }
        mInfo.text = GetInfo();
    }

}
