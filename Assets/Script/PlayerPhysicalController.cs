using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 负责处理角色的物理逻辑，动画状态机应该参照本脚本
public class PlayerPhysicalController : MonoBehaviour
{
    public string deviceID;

    Animator animator;

    public Controls constants;
    // Start is called before the first frame update
    public StoreOnlyController ctrl = new StoreOnlyController(); // 如果还没被JoiningGame放进来，则开一个空输入的BaseController，这样我们就不用判空了


    public GameObject default_weapon_prefab;
    WeaponBase current_weapon;
    void Awake()
    {
        GameObject weapon = Instantiate(default_weapon_prefab.gameObject, transform);
        current_weapon = weapon.GetComponent<WeaponBase>();
        animator = GetComponent<Animator>();
        velocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, constants.groundHeight, transform.position.z);
    }

    //int orientation; // 
    Vector3 velocity;

    int floating_jump_ctr = 0;
    public int floating_jump_cnt_limit = 1; // 支持后续的多段跳道具

    public enum VerticalStateCode : int
    {
        GROUNDED,
        FLOATING,
    }
    // 垂直方向StateMachine(不一定需要，状态很简单)

    System.Func<PlayerPhysicalController, VerticalStateCode>[] VerticalSM = {
        (obj) => { // 0号GROUNDED
            if (obj.ctrl.OnUp())
            {
                obj.velocity.y += obj.constants.jumpForce1;
                obj.animator.SetBool("is_grounded", false);
                obj.animator.SetFloat("vertical_v", obj.velocity.y);
                return VerticalStateCode.FLOATING;
            }
            // 检测踏空逻辑
            if(obj.constants.groundHeight < obj.transform.position.y)
            {
                obj.velocity.y -= obj.constants.gravity;
                obj.animator.SetBool("is_grounded", false);
                obj.animator.SetFloat("vertical_v", obj.velocity.y);
                return VerticalStateCode.FLOATING;
            }
            return VerticalStateCode.GROUNDED;
        },
        (obj) => {  // FLOATING
            // 检测着地逻辑
            if(obj.constants.groundHeight >= obj.transform.position.y)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, obj.constants.groundHeight, obj.transform.position.z);
                obj.velocity.y = 0f;
                obj.animator.SetFloat("vertical_v", obj.velocity.y);
                obj.animator.SetBool("is_grounded", true);
                obj.floating_jump_ctr = 0;
                return VerticalStateCode.GROUNDED;
            }
            if (obj.ctrl.OnUp() && obj.floating_jump_ctr < obj.floating_jump_cnt_limit)
            {
                ++obj.floating_jump_ctr;
                obj.velocity.y = Mathf.Max(obj.velocity.y + obj.constants.jumpForce2, obj.constants.jumpForce2);
            }
            obj.velocity.y -= obj.constants.gravity;
            obj.animator.SetFloat("vertical_v", obj.velocity.y);
            return VerticalStateCode.FLOATING;
        },
    };
    // 垂直
    // 控制人物起降状态，维护多段跳，后期的火箭等垂直方向的移动
    VerticalStateCode vertical_state = VerticalStateCode.GROUNDED; // 默认着陆状态

    // 后期开火逻辑用，因为可以直接取用transform.localScale.x，这里就不额外用一个变量记了
    public float orientation // 这个以input为准，左负右正，localScale的x默认是和这个相反的，因为贴图是朝左的，所以这里反转一下
    {
        get { return transform.localScale.x < 0 ? 1f : -1f; }
        set
        {
            if (Mathf.Sign(transform.localScale.x) == Mathf.Sign(value)) // 符号是反的
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public static float GetOrientation(Vector3 localScale)
    {
        return localScale.x < 0 ? 1f : -1f;
    }

    // 横向位移操作处理函数，这里抽出来与开火和被弹分开
    void HorizontalMovementHandler()
    {
        float movement = ctrl.Horizon();
        if (movement != 0)
        {
            animator.SetBool("is_moving", true);
            orientation = movement;
        }
        else
            animator.SetBool("is_moving", false);
        velocity.x *= constants.frictionRate;
        velocity.x += movement;
        velocity.x = Mathf.Max(-constants.horizontalVelocityLimit, velocity.x);
        velocity.x = Mathf.Min(constants.horizontalVelocityLimit, velocity.x);
    }

    void FireHandler()
    {
        if (ctrl.OnFire() && current_weapon.TryFire(transform, orientation))
        {
            velocity.x -= orientation * current_weapon.anti_impulse;
        }
    }

    //void FixedUpdate()
    //{
    //    Tick(Time.fixedDeltaTime);
    //}

    // 刷入ctrl，传入deltaTime即可更新物理状态，所有的物理逻辑开始于此处
    public void Tick(float deltaTime)
    {
        ctrl.OnLogicFrameUpdate();
        vertical_state = VerticalSM[(int)vertical_state](this); // 处理纵向物理层逻辑
        HorizontalMovementHandler();
        FireHandler();
        // 应用移动
        transform.Translate(velocity * deltaTime);
    }

    // 这个需要换掉
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Triggered" + collision.gameObject.name);
        if(collision.gameObject.name.Contains("bullet"))
        {
            BulletNormal bullet = collision.gameObject.GetComponent<BulletNormal>();
            velocity.x += bullet.orientation * bullet.hitback;
            Destroy(collision.gameObject);
        }
    }
}
