using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponBase
{
    // Start is called before the first frame update
    void Start()
    {
        bulletMaxCount = 100000000;
        bulletCount = bulletMaxCount;
        bulletType = "Pistol";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool TryFire(Transform transform, float orientation)
    {
        if (Time.time >= next_shot_time)
        {
            next_shot_time = Time.time + cooldown;
            Fire(transform, orientation);
            return true;
        }
        return false;
    }
}
