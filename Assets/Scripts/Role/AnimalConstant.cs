using UnityEngine;
public static class AnimalConstant {

    public const int Origin = -1;
    public const int Up = 0;
    public const int Down = 1;
    public const int Left = 2;
    public const int Right = 3;

    /////////////////////////////////////////////////////////////
    //角色相關
    public const string RolePrefab = "Role";    //遊戲物件名稱

    public const float RoleWinkColdDown = 5;    //動畫眨眼間隔
    public const float RoleGripperColdDown = 9; //動畫抓頭間隔

    public const float RoleAttackRange = 5;    //攻擊範圍
    public const float RolePickUpRange = 10;   //撿取範圍    
    public const float RoleCollectRange = 5;   //採集範圍

    public const float RoleTyrants = 2f;       //無敵時間
    //////////////////////////////////////////////////////////////
    //怪物相關
    public const float RunMagnification = 2;  //跑步跟移動的比值
    public const float RunAwayDistance = 10;  //逃跑距離
    public const float ChaseDistance = 10;    //追逐距離
}
