using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletFactory : MonoBehaviour
{
    // Start is called before the first frame update
    public static BulletBase CreateBullet(GameObject bullet,string bulletType)
    {
        //分别是手枪，步枪，冲锋枪，霰弹枪，狙击枪
        switch(bulletType)
        {
            case "Pistol":
                return bullet.AddComponent<Pistol>();
            case "Rifle":
                return bullet.AddComponent<Rifle>();
            case "SMG":
                return bullet.AddComponent<SMG>();
            case "ShotGun":
                return bullet.AddComponent<ShotGun>();
            case "Sniper":
                return bullet.AddComponent<Sniper>();
            default:
                throw new ArgumentException("Invalid weapon type.");
        }
    }
}
