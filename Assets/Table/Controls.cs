using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Controls", order = 1)]
public class Controls : ScriptableObject
{
    public float horizontalVelocityLimit = 10f;
    public float frictionRate = .95f; // 滑动摩擦力（按百分比衰减
    public float groundHeight = -3f; // 后期要干掉
    public float gravity = .98f;

    public float jumpForce1 = 10f; // 着陆起跳力
    public float jumpForce2 = 6f; // 二段跳力

}
