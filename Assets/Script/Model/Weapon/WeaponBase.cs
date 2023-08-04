using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public virtual float cooldown => .12f; // 配表数据开火冷却期
    public virtual float bullet_speed => 12f;//子弹射速（废弃）
    public virtual float anti_impulse => .4f; // 反冲量
    public virtual float hitback => 12f; // 被弹冲量


    public int bulletCount = 30; //当前弹匣数量
    public int bulletMaxCount = 30; //弹匣最大容量
    public string bulletType = "pistol";

    public GameObject bullet_prefab;

    public float next_shot_time = 0;
    public WeaponBase() { }

    public virtual bool CheckAmmo() { return true; }

    public virtual bool TryFire(Transform transform, float orientation)
    {
        if (Time.time >= next_shot_time)
        {
            next_shot_time = Time.time + cooldown;
            Fire(transform, orientation);
            return true;
        }
        return false;
    }
    //检查子弹数量
    public virtual bool CheckBulletCount()
    {
        return (bulletCount == 0);
    }

    // 构造子弹
    public virtual void Fire(Transform transform, float orientation)
    {
        GameObject bullet = Instantiate(bullet_prefab, transform.position + orientation * new Vector3(.5f,0,0), transform.rotation);
        Transform bulletTransform = bullet.GetComponent<Transform>();
        //人物方向是反的?
        bulletTransform.localScale = -transform.localScale;
        BulletBase bulletModel = BulletFactory.CreateBullet(bullet,bulletType);
        bulletModel.init(bullet,orientation);
        bulletCount -= 1;
        // BulletNormal bullet_physics = bullet.GetComponent<BulletNormal>();

        // bullet_physics.init(new Vector3(bullet_speed * orientation, 0f, 0f), orientation, hitback);
    }
}
