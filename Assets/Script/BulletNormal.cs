using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletNormal : MonoBehaviour
{
    // Start is called before the first frame update

    const float CLEAR_BOUND = 100f; // position绝对值超出此边界的应该析构

    void Start() { }

    Vector3 velocity;

    // 朝向和速度，构造后传参
    public void init(Vector3 _velocity, float orientation)
    {
        velocity = _velocity;
        if (orientation < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        //if (is_left)
            //GetComponent<SpriteRenderer>().flipX = true;

        //TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
        //trailRenderer.Clear();
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
}
