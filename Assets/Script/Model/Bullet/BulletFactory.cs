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
                return bullet.AddComponent<PistolBullet>();
            case "Rifle":
                return bullet.AddComponent<RifleBullet>();
            case "SMG":
                return bullet.AddComponent<SMGBullet>();
            case "ShotGun":
                return bullet.AddComponent<ShotGunBullet>();
            case "Sniper":
                return bullet.AddComponent<SniperBullet>();
            default:
                throw new ArgumentException("Invalid weapon type.");
        }
    }
}
