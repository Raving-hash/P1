using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string bulletType;//子弹类型

    [SerializeField] private float destructionTime =1.5f; //子弹销毁时间
    [SerializeField] private float speed = 12f; // 子弹最大射速
    [SerializeField] private float specialEffects; //子弹可能具有特殊的效果，如冰冻目标、点燃目标等。(待定)
    public float orientation {get; private set;}//子弹当前角度
    public float hitBack{get; private set;}// 击退冲量
    private Rigidbody2D rbody;//子弹刚体

    public virtual void init(GameObject bullet,float _orientation)
    {
        rbody = bullet.GetComponent<Rigidbody2D>();
        rbody.velocity = new Vector3(speed * _orientation, 0f, 0f);
        // rbody.rotation = _orientation;
        orientation = _orientation;

        hitBack = 12f;
        DestroySelf(bullet);
    }
    //根据子弹销毁时间来销毁子弹
    void DestroySelf(GameObject bullet)
    {
        Destroy(bullet,destructionTime);
    }
}
