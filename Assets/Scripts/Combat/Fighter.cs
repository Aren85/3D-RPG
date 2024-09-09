using System.Collections;  // 引入集合相關的命名空間
using System.Collections.Generic;  // 引入泛型集合相關的命名空間
using UnityEngine;  // 引入Unity引擎的基本命名空間
using RPG.Movement;  // 引入RPG移動系統的命名空間
using RPG.Core;  // 引入RPG核心功能的命名空間

namespace RPG.Combat  // 定義在RPG戰鬥系統下的命名空間
{
    public class Fighter : MonoBehaviour, IAction  // 定義Fighter類別，繼承MonoBehaviour並實現IAction接口
    {
        // 武器的攻擊範圍，通過Unity的檢查器中設置的可序列化字段
        [SerializeField] float weaponRange = 2f;  // 設置攻擊範圍為2米
        [SerializeField] float timeBetweenAttacks = 1f;  // 設置每次攻擊間隔為1秒
        [SerializeField] float weaponDamage = 5f;
        // 當前攻擊目標的Transform
        Health target;  // 儲存攻擊目標的Transform
        float timeSinceLastAttack = Mathf.Infinity;  // 記錄自上次攻擊以來經過的時間

        private void Update()  // Unity內建的Update方法，每一幀執行一次
        {
            timeSinceLastAttack += Time.deltaTime;  // 更新距離上次攻擊的時間

            // 如果沒有目標則直接返回，結束本幀的更新
            if (target == null) return;
            if (target.IsDead()) return;

            // 如果目標存在且目標在攻擊範圍之外
            if (!GetIsInRange())  // 如果目標不在攻擊範圍內
            {
                print(target.gameObject);
                // 移動到目標的位置
                GetComponent<Mover>().MoveTo(target.transform.position);  // 使用Mover組件移動到目標
            }
            else  // 如果目標在攻擊範圍內
            {
                // 停止移動
                GetComponent<Mover>().Cancel();  // 停止移動
                AttackBehaviour();  // 執行攻擊行為
            }
        }

        private void AttackBehaviour()  // 攻擊行為方法
        {
            transform.LookAt(target.transform);
            // 檢查自上次攻擊以來是否已經過足夠的時間
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // 觸發動畫控制器的攻擊動畫
                TriggerAttack();
                timeSinceLastAttack = 0;  // 重置攻擊計時器

            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private void Hit()
        {
            if (target == null) return;
            target.TakeDamage(weaponDamage);
        }

        // 檢查當前物體與目標物體之間的距離是否小於武器的攻擊範圍
        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;  // 返回是否在攻擊範圍內
        }

        // 設置當前攻擊的目標
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
        public void Attack(GameObject combatTarget)
        {
            // 使用ActionScheduler開始新的行動
            GetComponent<ActionScheduler>().StartAction(this);

            // 將目標設置為傳入的CombatTarget的Transform
            target = combatTarget.GetComponent<Health>();

        }

        // 取消當前的攻擊目標
        public void Cancel()
        {
            StopAttack();
            target = null;  // 將目標設置為null，停止攻擊行為
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        //動畫事件（在動畫中可以觸發這個方法，可能是攻擊動作中真正命中的時刻）

    }
}
