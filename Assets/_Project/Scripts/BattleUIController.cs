using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ActionType { None, NormalAttack, ExtraAttack }

public class BattleUIController : MonoBehaviour
{
    [Header("=== UI部品 ===")]
    [SerializeField] private Button attackButton;
    [SerializeField] private Button extraAttackButton;
    [SerializeField] private TextMeshProUGUI logText;

    private ActionType selectedAction = ActionType.None;

    void Awake()
    {
        attackButton.onClick.AddListener(() => selectedAction = ActionType.NormalAttack);
        extraAttackButton.onClick.AddListener(() => selectedAction = ActionType.ExtraAttack);
        SetButtonsActive(false);
    }

    /// <summary>
    /// ボタンの表示 / 非表示
    /// </summary>
    public void SetButtonsActive(bool isActive)
    {
        attackButton.gameObject.SetActive(isActive);
        extraAttackButton.gameObject.SetActive(isActive);
    }

    /// <summary>
    /// ログメッセージの更新
    /// </summary>
    public void SetLog(string message)
    {
        logText.text = message;
    }

    /// <summary>
    /// プレイヤーの入力を待機し、選択されたアクションを返す
    /// </summary>
    public IEnumerator WaitForPlayerAction(Action<ActionType> onActionSelected)
    {
        selectedAction = ActionType.None;
        SetButtonsActive(true);

        // ボタンが押されるまでフレーム待機
        yield return new WaitUntil(() => selectedAction != ActionType.None);

        SetButtonsActive(false);
        onActionSelected?.Invoke(selectedAction);
    }
}