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
        // ��ȡ��������
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // �����ƶ�����
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;

        // Ӧ���ƶ�
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
