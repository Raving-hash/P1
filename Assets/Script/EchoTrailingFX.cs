using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoTrailingFX : MonoBehaviour
{
    public GameObject echo_pref;
    public float cd;
    public float ddl;
    float next_spawn_time = 0f;

    // Start is called before the first frame update
    void Start(){}


    // Update is called once per frame
    void Update()
    {
        if(next_spawn_time <= Time.time)
        {
            GameObject ins = Instantiate(echo_pref, transform.position, transform.rotation);
            ins.transform.localScale = transform.localScale;
            Destroy(ins, ddl);
            next_spawn_time = Time.time + cd;
        }
    }
}
