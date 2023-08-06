using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 定义每个按键在bitset中占用的位，用于支持一些按键的长押监测
public enum KeyType : int
{
    // 业务
    FIRE,
    BOMB,
    //LEFT,
    //RIGHT,
    UP,
    DOWN,
    // 网络发送
    UP_KEYDOWN,
    UP_KEYUP,
    DOWN_KEYDOWN,
    DOWN_KEYUP,
    FIRE_KEYDOWN,
    FIRE_KEYUP,
    BOMB_KEYDOWN,
    BOMB_KEYUP,
    // 服务器特殊指令
    JOIN,
    EXIT,
    EMPTY_FRAME // 本帧为空操作，按原样tick即可
}

/*
 * 服务器不处理裸操作
 * 
*/