using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTicker : MonoBehaviour
{
    // Start is called before the first frame update
    public GlobalVar gv;
    void Start()
    {
        gv.frameID = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ++gv.frameID;
    }
}
