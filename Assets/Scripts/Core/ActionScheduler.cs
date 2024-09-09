using System.Collections;  // 引入集合相關的命名空間（未使用但保留）
using System.Collections.Generic;  // 引入泛型集合相關的命名空間（未使用但保留）
using UnityEngine;  // 引入Unity引擎的基本命名空間

namespace RPG.Core  // 定義在RPG核心功能下的命名空間
{
    public class ActionScheduler : MonoBehaviour  // 定義ActionScheduler類別，繼承自MonoBehaviour
    {
        IAction currentAction;  // 宣告一個IAction類型的變數，用來保存當前的行動

        public void StartAction(IAction action)  // 定義StartAction方法，用來開始一個新的行動
        {
            if (currentAction == action) return;  // 如果當前行動和新行動相同，直接返回，避免重複執行相同行動

            if (currentAction != null)  // 如果當前有正在執行的行動
            {
                currentAction.Cancel();  // 取消當前的行動，調用它的Cancel方法
                Debug.Log("取消" + currentAction);  // 在控制台輸出當前行動已被取消
            }
            currentAction = action;  // 將新行動設為當前行動
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}
