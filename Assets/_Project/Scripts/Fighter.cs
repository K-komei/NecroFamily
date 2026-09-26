using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] private string characterName = "Character";
    [SerializeField] private int maxHp = 100;
    [SerializeField] private int attackPower = 25;

    public int CurrentHp { get; private set; }
    public string CharacterName => characterName;
    public int AttackPower => attackPower;
    public bool IsDead => CurrentHp <= 0;

    void Awake()
    {
        CurrentHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        CurrentHp = Mathf.Max(CurrentHp - damage, 0);
        Debug.Log($"{characterName} took {damage} damage! HP: {CurrentHp}/{maxHp}");
    }
}