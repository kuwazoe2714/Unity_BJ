using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    private int myCardCnt;                              // プレイヤーの手札枚数
    private int playerScore;                            // プレイヤーの点数
    public int GetPlayerScore { get { return playerScore; } }   // プレイヤー点数のゲッターだけ作成

    [SerializeField]
    private GameObject myHand;                          // プレイヤーの手札を置く枠
    [SerializeField]
    private Text playerScore_Text;                      // プレイヤーの点数表示テキスト

    public List<GameObject> MyCard { get; set; }        // プレイヤーが持っている手札
    public List<GameObject> MyHandPos { get; set; }     // プレイヤーの手札管理リスト

    private bool IsBust { get;set; }
    public Status.STATUS MyStatus { get; set; }         // プレイヤーの状態(削除予定)
    public bool CroupierFlag { get; set; }              // 自分はディーラーかフラグ(falseならPlayer)

    // 初期化処理関数
    private void Awake()
    {
        MyCard = new List<GameObject>();
        MyHandPos = new List<GameObject>();
        
        var screenSize = GetComponent<RectTransform>().transform.position;          // 一応親(Panel)の位置情報見て設定しているけどズレてる。何かしら修正必要
        // 手札最大枚数の5枚で初期化
        for (int i = 0; i < 5; i++)
        {
            var cardPos = Instantiate(myHand, transform);

            //
            // これ書き方汚い
            //
            cardPos.transform.position = new Vector3(myHand.transform.position.x + (-150 + (75 * i)) + screenSize.x, myHand.transform.position.y + screenSize.y);
            MyHandPos.Add(cardPos);
        }

        myCardCnt = 0;

        MyStatus = Status.STATUS.STATUS_NONE;
    }

    /**
     * <summary> ディーラーがバーストしてたら呼ばれない </summary>
     */
    public void Croupier_Update()
    {
        // ディーラーの処理
        if (playerScore < 17)
        {
            MyStatus = Status.STATUS.STATUS_HIT;
        }
        else
        {
            MyStatus = Status.STATUS.STATUS_STAND;
        }

    }

    /**
     * <summary> 対応するボタン(Hit)が押された時に呼ぶ関数 </summary>
     */
    public void PlayerHit()
    {
        MyStatus = Status.STATUS.STATUS_HIT;
    }

    /**
     * <summary> 対応するボタン(Stand)が押された時に呼ぶ関数 </summary>
     */
    public void PlayerStand()
    {
        MyStatus = Status.STATUS.STATUS_STAND;
    }

    /**
     * <summary> 手札増えた時の処理 </summary>
     */
    public void HitUpdate()
    {
        // 点数算出
        playerScore = Scorecalc(MyCard);
        // バーストしていないかチェック
        BurstStateCheck(playerScore);
        // 点数表示
        ScoreDisplay();

        // プレイヤー以外の場合
        // 2枚目以降の手札を隠す処理
        //
        // TODO ありえない…
        /*
        if(CroupierFlag && 1 < MyCard.Count )
        {
            for (int screenhand = 1; screenhand < MyCard.Count; screenhand++)
            {
                MyCard[screenhand].GetComponent<Image>().sprite = Card.cardSprite[52];
            }
        }
        */

    }

    /**
     *<summary> 点数割り出し関数 </summary>
     */
    int Scorecalc(List<GameObject> Hands)
    {
        int score = 0;
        int cntA = 0;

        for (int card = 0; card < Hands.Count; card++)
        {
            Card.Number number = Hands[card].GetComponent<Card>().MyNumber;

            switch (number)
            {
                case Card.Number.A:
                    score += 1;
                    cntA++;
                    break;

                case Card.Number._2:
                case Card.Number._3:
                case Card.Number._4:
                case Card.Number._5:
                case Card.Number._6:
                case Card.Number._7:
                case Card.Number._8:
                case Card.Number._9:
                case Card.Number._10:
                    score += (int)number;
                    break;

                case Card.Number.J:
                case Card.Number.Q:
                case Card.Number.K:
                    score += 10;
                    break;
            }
        }
        // TODO なにかしょりする

        // 「A」を持っている、かつ点数が11点以下(バーストしない)なら
        // 「A」を11点としてカウントする
        if (score <= 11 && cntA != 0)
        {
            score += 10;
        }

        return score;
    }

    /**
     * <summary> 自分の点数を渡して「Burst」していたら「Burst」状態に。 </summary>
     */
     void BurstStateCheck( int myScore )
    {
        if( 21 < myScore )
        {
            MyStatus = Status.STATUS.STATUS_BURST;
        }
        else
        {
            MyStatus = Status.STATUS.STATUS_NONE;
        }
    }

    /**
     * <summary> 点数表示関数 </summary>
     */
     void ScoreDisplay()
    {
        if (MyStatus != Status.STATUS.STATUS_BURST)
        {
            playerScore_Text.text = "Player:" + playerScore;
        }
        else
        {
            playerScore_Text.text = "Player:BURST";
        }
    }

}
