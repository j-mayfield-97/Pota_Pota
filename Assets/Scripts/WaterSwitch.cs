using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSwitch : MonoBehaviour
{
    public void switch_parent_pipe()
    {
        GetComponentInParent<PipeSwitch>().controller_switch();
    }

}
