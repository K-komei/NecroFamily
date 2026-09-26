using System;
using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("=== Dependencies ===")]
    [SerializeField] private BattleFactory battleFactory;
    [SerializeField] private BattleUIController uiController;

    private Fighter player;
    private Fighter enemy;
    private Action<bool> onBattleEndCallback;

    /// <summary>
    /// バトル準備と開始（完了時のコールバックを受け取る）
    /// </summary>
    public void SetupAndStartBattle(int stageId, Action<bool> onBattleEnd)
    {
        onBattleEndCallback = onBattleEnd;

        // 1. 前回の残骸を破棄
        if (player != null) Destroy(player.gameObject);
        if (enemy != null) Destroy(enemy.gameObject);

        // 2. Factory でユニット生成
        (player, enemy) = battleFactory.CreateBattleUnits(stageId);

        // 3. UI初期化
        uiController.SetLog($"Stage {stageId}: {enemy.CharacterName} appeared!");

        // 4. バトルループ開始
        StartCoroutine(BattleLoop());
    }

    private IEnumerator BattleLoop()
    {
        yield return new WaitForSeconds(1.0f);

        while (!player.IsDead && !enemy.IsDead)
        {
            // --- Player Turn ---
            yield return StartCoroutine(PlayerTurnPhase());
            if (enemy.IsDead) break;

            // --- Enemy Turn ---
            yield return StartCoroutine(EnemyTurnPhase());
            if (player.IsDead) break;
        }

        // --- Result Phase ---
        yield return new WaitForSeconds(0.5f);
        bool isVictory = enemy.IsDead;

        if (player.IsDead)
        {
            uiController.SetLog($"DEFEAT... {player.CharacterName} was defeated.");
        }
        else if (enemy.IsDead)
        {
            uiController.SetLog($"VICTORY! {enemy.CharacterName} was eliminated!");
            yield return new WaitForSeconds(1.0f);
            Destroy(enemy.gameObject);
        }

        // 決着メッセージを読ませる待機
        yield return new WaitForSeconds(1.5f);

        // 終了通知（勝利フラグを渡して外側へ通知）
        onBattleEndCallback?.Invoke(isVictory);
    }

    private IEnumerator PlayerTurnPhase()
    {
        uiController.SetLog("Your turn! Choose an action.");

        ActionType chosenAction = ActionType.None;
        yield return StartCoroutine(uiController.WaitForPlayerAction(action => chosenAction = action));

        int damage = (chosenAction == ActionType.ExtraAttack) ? player.AttackPower * 2 : player.AttackPower;
        string actionName = (chosenAction == ActionType.ExtraAttack) ? "Heavy Attack" : "Attack";

        yield return StartCoroutine(ExecuteAttack(player, enemy, damage, actionName));
    }

    private IEnumerator EnemyTurnPhase()
    {
        uiController.SetLog($"{enemy.CharacterName}'s turn...");
        yield return new WaitForSeconds(1.0f);

        yield return StartCoroutine(ExecuteAttack(enemy, player, enemy.AttackPower, "Attack"));
    }

    private IEnumerator ExecuteAttack(Fighter attacker, Fighter target, int damage, string actionName)
    {
        uiController.SetLog($"{attacker.CharacterName} uses {actionName}!");
        yield return new WaitForSeconds(0.5f);

        target.TakeDamage(damage);
        yield return new WaitForSeconds(0.8f);
    }

}