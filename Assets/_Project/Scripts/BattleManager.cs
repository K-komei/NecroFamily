using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState
{
    PlayerTurn,
    EnemyTurn,
    Busy,
    Won,
    Lost
}

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Fighter player;
    [SerializeField] private Fighter enemy;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button ExtraAttackButton;
    
    [SerializeField] private TextMeshProUGUI logText;

    private BattleState state;

    void Start()
    {
        attackButton.onClick.AddListener(OnPlayerAttackClicked);
        ExtraAttackButton.onClick.AddListener(OnPlayerExtraAttackClicked);
        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        state = BattleState.PlayerTurn;
        attackButton.interactable = true;
        ExtraAttackButton.interactable = true;
        logText.text = "Your Turn! Click ATTACK.";
    }

    private void OnPlayerAttackClicked()
    {
        if (state != BattleState.PlayerTurn) return;

        state = BattleState.Busy;
        attackButton.interactable = false;
        ExtraAttackButton.interactable = false;

        // Player attacks Enemy
        enemy.TakeDamage(player.AttackPower);
        logText.text = $"{player.CharacterName} attacks! Dealt {player.AttackPower} DMG!";

        if (enemy.IsDead)
        {
            state = BattleState.Won;
            logText.text = $"Victory! {enemy.CharacterName} was defeated!";
            return;
        }

        // Wait 1 sec then switch to Enemy turn
        StartCoroutine(EnemyTurnCoroutine());
    }

    private void OnPlayerExtraAttackClicked()
    {
        if (state != BattleState.PlayerTurn) return;

        state = BattleState.Busy;
        attackButton.interactable = false;
        ExtraAttackButton.interactable = false;

        // Player attacks Enemy

        int attackDamage = player.AttackPower*2;


        enemy.TakeDamage(attackDamage);
        logText.text = $"{player.CharacterName} attacks! Dealt {attackDamage} DMG!";

        if (enemy.IsDead)
        {
            state = BattleState.Won;
            logText.text = $"Victory! {enemy.CharacterName} was defeated!";
            return;
        }

        // Wait 1 sec then switch to Enemy turn
        StartCoroutine(EnemyTurnCoroutine());
    }

    private IEnumerator EnemyTurnCoroutine()
    {
        state = BattleState.EnemyTurn;
        yield return new WaitForSeconds(1.0f);

        // Enemy attacks Player
        player.TakeDamage(enemy.AttackPower);
        logText.text = $"{enemy.CharacterName} attacks back! Dealt {enemy.AttackPower} DMG!";

        if (player.IsDead)
        {
            state = BattleState.Lost;
            logText.text = $"Defeat... {player.CharacterName} has fallen.";
            yield break;
        }

        yield return new WaitForSeconds(1.0f);
        StartPlayerTurn();
    }
}