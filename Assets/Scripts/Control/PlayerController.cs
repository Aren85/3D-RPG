using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;
        private void Start()
        {
            health = GetComponent<Health>();
        }
        private void Update()
        {
            if (health.IsDead()) { return; }
            if (InteractWithCombat()) { return; }
            if (InteractWithMovement()) { return; }
            //Debug.DrawRay(GetMouseRay().origin, GetMouseRay().direction * 100);
        }

        private bool InteractWithCombat()//戰鬥互動
        {
            // 使用射線投射來檢測玩家鼠標所指向的所有物體
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            // 遍歷所有射線擊中的物體
            foreach (RaycastHit hit in hits)
            {
                // 嘗試獲取該物體上的CombatTarget組件
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                // 如果該物體沒有CombatTarget組件，則繼續檢查下一個物體
                if (target == null) { continue; }

                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }
                // 如果玩家按下了滑鼠左鍵
                if (Input.GetMouseButton(0))
                {
                    // 獲取Fighter組件並攻擊該目標
                    GetComponent<Fighter>().Attack(target.gameObject);
                }

                // 返回true表示成功與某個戰鬥目標進行了互動
                return true;
            }

            // 返回false表示沒有檢測到任何戰鬥目標
            return false;
        }

        private bool InteractWithMovement()//移動互動
        {
            RaycastHit hit; // 用來存儲射線擊中的信息

            // 發射一條從攝像機到鼠標指針方向的射線，檢測是否擊中任何物體
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            // 如果射線擊中某個物體
            if (hasHit)
            {
                // 如果玩家按住了滑鼠左鍵
                if (Input.GetMouseButton(0))
                {
                    // 獲取Mover組件並移動到擊中的點
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                // 返回true表示成功與場景進行了移動互動
                return true;
            }
            // 返回false表示沒有檢測到可以移動的位置
            return false;
        }

        private static Ray GetMouseRay()
        {
            // 從主攝像機(Camera.main)的屏幕坐標(mousePosition)生成一條射線(Ray)
            // 這條射線是從攝像機位置開始，穿過鼠標指針的位置
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
