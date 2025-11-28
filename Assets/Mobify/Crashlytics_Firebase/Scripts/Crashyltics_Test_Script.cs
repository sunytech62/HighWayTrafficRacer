using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crashyltics_Test_Script : MonoBehaviour
{
    public UnityEngine.UI.Text info;
    public void Send_FakeCrashReport()
    {
        throw new System.Exception("Test Exception please ignore");
    }
}
