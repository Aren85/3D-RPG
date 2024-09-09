using System.Collections;  // 引入集合相關的命名空間
using System.Collections.Generic;  // 引入泛型集合
using RPG.Combat;  // 引入RPG戰鬥系統的命名空間
using RPG.Core;  // 引入RPG核心功能的命名空間
using Unity.VisualScripting;  // 引入Unity的Visual Scripting工具
using UnityEngine;  // 引入Unity引擎的基本命名空間
using UnityEngine.AI;  // 引入Unity的AI導航系統

namespace RPG.Movement  // 自定義的角色移動相關命名空間
{
    public class Mover : MonoBehaviour, IAction  // Mover類別，繼承自MonoBehaviour並實現IAction接口
    {
        [SerializeField] Transform target;  // 可在Unity編輯器中設置的目標Transform

        NavMeshAgent navMeshAgent;  // 宣告一個NavMeshAgent變數，負責角色導航
        Health health;

        private void Start()  // Unity內建的Start方法，遊戲開始時執行一次
        {
            // 從遊戲物件上獲取NavMeshAgent組件
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()  // Unity內建的Update方法，每幀執行一次
        {
            if (health.IsDead())
            {
                navMeshAgent.enabled = !health.IsDead();
            }
            UpdateAnimator();  // 每幀更新動畫狀態
        }

        public void StartMoveAction(Vector3 destination)  // 開始移動行動，傳入目的地Vector3座標
        {
            GetComponent<ActionScheduler>().StartAction(this);  // 使用ActionScheduler來開始新行動
            MoveTo(destination);  // 調用MoveTo方法，開始移動到指定目的地
        }

        public void MoveTo(Vector3 destination)  // 移動到指定目的地的方法
        {
            navMeshAgent.destination = destination;  // 設定NavMeshAgent的目的地
            navMeshAgent.isStopped = false;  // 設置NavMeshAgent為啟動狀態，允許移動
        }

        public void Cancel()  // 取消當前行動
        {
            navMeshAgent.isStopped = true;  // 停止NavMeshAgent的移動
        }

        void UpdateAnimator()  // 更新動畫狀態的方法
        {
            Vector3 velocity = navMeshAgent.velocity;  // 獲取NavMeshAgent的當前速度
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);  // 將世界坐標系下的速度轉換為本地坐標系
            float speed = localVelocity.z;  // 獲取角色在Z軸（前進方向）上的速度
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);  // 將速度賦值給動畫參數"forwardSpeed"，用於控制角色的前進動畫
        }
    }
}
