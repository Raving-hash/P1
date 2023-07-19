using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float moveSpeed = 5f;

    void Update()
    {
        // 获取键盘输入
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 计算移动向量
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;

        // 应用移动
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
