using UnityEngine;
using UnityEngine.UI;

public class AppRouter : MonoBehaviour
{
    [Header("=== 画面コンテナ (Canvas内の各View) ===")]
    [SerializeField] private GameObject titleView;
    [SerializeField] private GameObject mapView;
    [SerializeField] private GameObject battleView;

    [Header("=== タイトル画面のボタン ===")]
    [SerializeField] private Button startButton;

    [Header("=== マップ画面のボタン ===")]
    [SerializeField] private Button stage1Button;

    [Header("=== バトルマネージャー ===")]
    [SerializeField] private BattleManager battleManager;

    void Start()
    {
        // 1. 各ボタンのルーティングイベント登録
        startButton.onClick.AddListener(ShowMap);
        stage1Button.onClick.AddListener(() => StartBattle(1));

        // 2. 起動時はタイトル画面を表示
        ShowTitle();
    }

    public void ShowTitle()
    {
        titleView.SetActive(true);
        mapView.SetActive(false);
        battleView.SetActive(false);
    }

    public void ShowMap()
    {
        titleView.SetActive(false);
        mapView.SetActive(true);
        battleView.SetActive(false);
    }

    public void StartBattle(int stageId)
    {
        titleView.SetActive(false);
        mapView.SetActive(false);
        battleView.SetActive(true);

        // バトル開始をキックし、終了時のコールバックとして「マップへ戻る」を登録
        battleManager.SetupAndStartBattle(stageId, (isVictory) =>
        {
            ShowMap();
        });
    }
}