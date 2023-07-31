using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletFactory : MonoBehaviour
{
    // Start is called before the first frame update
    public static BulletBase CreateBullet(string bulletType)
    {
        //分别是手枪，步枪，冲锋枪，霰弹枪，狙击枪
        switch(bulletType)
        {
            case "Pistol":
                return new Pistol();
            case "Rifle":
                return new Rifle();
            case "SMG":
                return new SMG();
            case "ShotGun":
                return new ShotGun();
            case "Sniper":
                return new Sniper();
            default:
                throw new ArgumentException("Invalid weapon type.");
        }
    }
}
