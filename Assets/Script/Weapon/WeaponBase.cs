using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    protected virtual float cooldown => .12f; // 配表数据
    protected virtual float bullet_speed => 12f;
    public virtual float anti_impulse => .4f; // 反冲量
    public virtual float hitback => 12f; // 被弹冲量

    public GameObject bullet_prefab;

    float next_shot_time = 0;
    WeaponBase() { }

    public virtual bool CheckAmmo() { return true; }

    public bool TryFire(Transform transform, float orientation)
    {
        if (Time.time >= next_shot_time)
        {
            next_shot_time = Time.time + cooldown;
            Fire(transform, orientation);
            return true;
        }
        return false;
    }

    // 构造子弹
    protected virtual void Fire(Transform transform, float orientation)
    {
        GameObject bullet = Instantiate(bullet_prefab, transform.position + orientation * new Vector3(.5f,0,0), transform.rotation);
        BulletBase bulletModel = BulletFactory.CreateBullet(bullet,"Pistol");
        bulletModel.init(bullet,orientation);
        // BulletNormal bullet_physics = bullet.GetComponent<BulletNormal>();

        // bullet_physics.init(new Vector3(bullet_speed * orientation, 0f, 0f), orientation, hitback);
    }
}
