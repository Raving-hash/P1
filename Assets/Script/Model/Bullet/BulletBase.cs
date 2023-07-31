using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string bulletType;//子弹类型
    [SerializeField] private float hitBack; // 击退冲量
    [SerializeField] private float destructionTime ; //子弹销毁时间
    [SerializeField] private float speed = 12f; // 子弹射速
    [SerializeField] private float specialEffects; //子弹可能具有特殊的效果，如击退目标、冰冻目标、点燃目标等。(待定)

    public void init(GameObject bullet,float orientation)
    {
        BulletNormal bullet_physics = bullet.GetComponent<BulletNormal>();
        bullet_physics.init(new Vector3(speed * orientation, 0f, 0f), orientation, hitBack);
    }
    //根据子弹销毁时间来销毁子弹
    void DestroySelf()
    {
        Destroy(this.gameObject,destructionTime);
    }
    //子弹碰到带有玩家控制的脚本物体会自动销毁()
    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerPhysicalController pc = other.gameObject.GetComponent<PlayerPhysicalController>();
        if(pc==null) return ;
        CalcHitback(pc,hitBack);
        Destroy(this.gameObject);
    }
    void CalcHitback(PlayerPhysicalController pc,float hitBack)
    {

    }
}
