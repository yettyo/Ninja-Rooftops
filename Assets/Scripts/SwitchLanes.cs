using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Forever;

public class SwitchLanes : MonoBehaviour
{
    LaneRunner runner;
    void Start()
    {
            runner = GetComponent<LaneRunner>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow)) runner.lane--;
        if(Input.GetKeyDown(KeyCode.RightArrow)) runner.lane++;
    }
}
