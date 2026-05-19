using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class BilliardsGameManager : MonoBehaviour
{
    public static BilliardsGameManager Instance;

    [Header("공 설정")]
    public List<BilliardsBall> allBalls; // 씬에 있는 공 4개를 모두 넣어주세요.
    public float stopThreshold = 0.01f;   // 정지 상태로 판정할 속도 임계값

    [Header("UI 설정")]
    public TextMeshProUGUI uiText;       

    [HideInInspector] public int currentTurn = 1; // 1 = 1P, 2 = 2P
    private int player1Score = 0;
    private int player2Score = 0;

    private bool isBallsMoving = false;
    private bool hitRed1 = false;
    private bool hitRed2 = false;
    private bool hitOpponent = false;

    private List<GameObject> redBallObjects = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        foreach (var ball in allBalls)
        {
            if (ball.ballType == BallType.Red)
            {
                redBallObjects.Add(ball.gameObject);
            }
        }
        UpdateUI();
    }

    void Update()
    {
        if (isBallsMoving && AreAllBallsStopped())
        {
            isBallsMoving = false;
            ProcessTurnEnd();
        }
    }

    public bool CanHit()
    {
        return !isBallsMoving && player1Score < 5 && player2Score < 5;
    }

    public void OnBallHitStart()
    {
        isBallsMoving = true;
        hitRed1 = false;
        hitRed2 = false;
        hitOpponent = false;
    }

    public void RecordCollision(BallType myType, BallType otherType, GameObject otherObj)
    {

        if (currentTurn == 1 && myType != BallType.Player1) return;
        if (currentTurn == 2 && myType != BallType.Player2) return;

        if (otherType == BallType.Red)
        {
            if (redBallObjects.Count > 0 && otherObj == redBallObjects[0]) hitRed1 = true;
            if (redBallObjects.Count > 1 && otherObj == redBallObjects[1]) hitRed2 = true;
        }
        else if ((currentTurn == 1 && otherType == BallType.Player2) ||
                 (currentTurn == 2 && otherType == BallType.Player1))
        {
            hitOpponent = true;
        }
    }


    bool AreAllBallsStopped()
    {
        foreach (var ball in allBalls)
        {
            if (ball.rb.linearVelocity.magnitude > stopThreshold || ball.rb.angularVelocity.magnitude > stopThreshold)
            {
                return false;
            }
        }
        return true;
    }


    void ProcessTurnEnd()
    {
        int scoreChange = 0;

        if (hitOpponent)
        {
            scoreChange = -1;
        }
        else if (hitRed1 && hitRed2)
        {
            scoreChange = 1;
        }

        if (currentTurn == 1)
            player1Score = Mathf.Max(0, player1Score + scoreChange);
        else
            player2Score = Mathf.Max(0, player2Score + scoreChange);

        if (player1Score >= 5 || player2Score >= 5)
        {
            UpdateUI();
            return;
        }

        currentTurn = (currentTurn == 1) ? 2 : 1;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (player1Score >= 5)
            uiText.text = "<color=blue>1P 승리!! 공이 모두 멈췄습니다.</color>";
        else if (player2Score >= 5)
            uiText.text = "<color=red>2P 승리!! 공이 모두 멈췄습니다.</color>";
        else
            uiText.text = $"<b>현재 턴: {currentTurn}P</b>\n1P 점수: {player1Score}점 / 2P 점수: {player2Score}점";
    }
}