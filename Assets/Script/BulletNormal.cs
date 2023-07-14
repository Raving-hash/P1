using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletNormal : MonoBehaviour
{
    // Start is called before the first frame update

    const float CLEAR_BOUND = 100f; // position绝对值超出此边界的应该析构

    void Start() { }

    Vector3 velocity;

    public float hitback; // 击退冲量

    // 朝向和速度，构造后传参
    public void init(Vector3 _velocity, float _orientation, float _hitback)
    {
        velocity = _velocity;
        orientation = _orientation;
        hitback = _hitback;
        //if (orientation < 0)
            //transform.localScale = new Vector3(-1, 1, 1);
        //if (is_left)
            //GetComponent<SpriteRenderer>().flipX = true;

        //TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
        //trailRenderer.Clear();
    }

    // 子弹贴图向右，所以右时localScale.x=1
    public float orientation
    {
        get => transform.localScale.x > 0 ? 1f : -1f;
        set => transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (velocity != null)
        {
            transform.position += velocity * Time.deltaTime;
            if (Mathf.Abs(transform.position.x) > CLEAR_BOUND)
            {
                //TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
                //trailRenderer.enabled = false;

                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
