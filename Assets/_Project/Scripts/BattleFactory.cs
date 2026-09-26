using UnityEngine;

public class BattleFactory : MonoBehaviour
{
    [Header("=== スポーン位置 ===")]
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform enemySpawnPoint;

    [Header("=== プレハブ ===")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;

    /// <summary>
    /// 指定されたステージのプレイヤーと敵を生成して返す
    /// </summary>
    public (Fighter player, Fighter enemy) CreateBattleUnits(int stageId)
    {
        // 第3引数に spawnPoint.transform を指定して、その子オブジェクトとして生成する！
        GameObject pObj = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation, playerSpawnPoint.transform);
        Fighter player = pObj.GetComponent<Fighter>();

        GameObject eObj = Instantiate(enemyPrefab, enemySpawnPoint.position, enemySpawnPoint.rotation, enemySpawnPoint.transform);
        Fighter enemy = eObj.GetComponent<Fighter>();

        return (player, enemy);
    }
}