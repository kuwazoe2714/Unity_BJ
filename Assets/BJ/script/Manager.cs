using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    // カードオブジェクトのPrefab設定用
    [SerializeField] private GameObject card_Pre;
    // 親に当たるキャンバス設定用
    [SerializeField] private Canvas canvas;
    // プレイヤーのキャンバス設定用
    [SerializeField] private Player player;
    // ディーラーのキャンバス設定用
    [SerializeField] private Player croupier;
    // 「Hit」ボタン設定用
    [SerializeField] private Button hitButton;
    // 「Stand」ボタン設定用
    [SerializeField] private Button standButton;

    public static List<GameObject> Card_List { get; set; }          // デッキ格納用リスト
    private List<Player> Player_List;                               // プレイヤー格納リスト
    private int drawCnt;

    // 生成時処理関数
    private void Awake()
    {
        // プレイヤーのリスト生成
        Player_List = new List<Player>();
        // デッキのリスト生成
        Card_List = new List<GameObject>();
        // デッキ生成
        DeckCreate();
        // デッキシャッフル
        DeckShuffle();
        // デッキから引かれたカードカウント用(でも「リスト名.Count」で現在の値取れるからいらなくない…？)
        drawCnt = 0;
    }

    // Update is called once per frame
    void Start ()
    {
        StartCoroutine(Update());
	}

    // デッキ生成処理関数
    void DeckCreate()
    {
        for (int design = 0; design < (int)Card.Design.DESIGN_MAX; design++)
        {
            for (int number = 1; number < (int)Card.Number.NUMBER_MAX; number++)
            {
                var card_Instan = Instantiate(card_Pre, canvas.transform);
                var card = card_Instan.GetComponent<Card>();
                card.transform.Translate(new Vector3(350f, 150f));
                card.MyDesign = (Card.Design)design;
                card.MyNumber = (Card.Number)number;
                card.GetComponent<Image>().sprite = card.GetSprite(number + design * 13);
                Card_List.Add(card_Instan);
            }
        }

        var backcard = Instantiate(card_Pre, canvas.transform);
        backcard.transform.Translate(new Vector3(350, 150));
        backcard.GetComponent<Image>().sprite = backcard.GetComponent<Card>().GetSprite(Card_List.Count+1);
    }

    // デッキシャッフル処理関数
    void DeckShuffle()
    {
        GameObject ShuffleDeck;           // 一時的にデッキを格納する器

        var rand = new System.Random();
        int nRand = 0;

        for (int i = 0; i < Card_List.Count; i++)
        {
            nRand = rand.Next(i, Card_List.Count);
            ShuffleDeck = Card_List[i];
            Card_List[i] = Card_List[nRand];
            Card_List[nRand] = ShuffleDeck;
        }
    }

    // デッキからカードを引く
    public void DeckDrawCard(Player player)
    {
        // プレイヤーの手札が5枚に達していないならカードを引く
        if (player.MyCard.Count < 5 || player.GetPlayerScore <= 21 )
        {
            var drawCard = Card_List[drawCnt++];
            player.MyCard.Add(drawCard);
            drawCard.transform.parent = player.MyHandPos[player.MyCard.Count - 1].transform;
            var from = drawCard.transform.position;
            var to = player.MyHandPos[player.MyCard.Count - 1].transform.position;

            StartCoroutine(EasingMove(from,to,drawCard));
            // アニメーション中はボタン押させないように
            hitButton.interactable = false;
            standButton.interactable = false;
        }
    }

    // ゲーム内処理
    void Game()
    {
        for(int i = 0; i < 2; i++)
        {
            // バーストチェック(バーストしていない人だけ更新する)
            if (Player_List[i].MyStatus == Status.STATUS.STATUS_BURST)
            {
                continue;
            }
            // TODO プレイヤーかどうかチェック
            if (Player_List[i].CroupierFlag == true)
            {
                Player_List[i].Croupier_Update();
            }
            // バーストしていないなら聞く
            switch (Player_List[i].MyStatus)
            {
                // まだ何も行動していない
                case Status.STATUS.STATUS_NONE:
                    return;

                // 「Hit」する意思を見せた
                case Status.STATUS.STATUS_HIT:
                    DeckDrawCard(Player_List[i]);
                    Player_List[i].HitUpdate();
                    break;

                // 「Stand」する意思を見せた
                case Status.STATUS.STATUS_STAND:
                    break;
            }
        }
    }

    /**
     * <summary> ゲーム更新 </summary>
     */
    private IEnumerator Update()
    {
        yield return GameInit();
        yield return GameUpdate();
        yield return GameResult();
    }


    /**
     * <summary> 初期化処理コルーチン </summary>
     */
    private IEnumerator GameInit()
    {
        player.CroupierFlag = false;
        Player_List.Add(player);
        croupier.CroupierFlag = true;
        Player_List.Add(croupier);

        // 参加者全員に初期手札配布
        for (int i = 0; i < 2; i++)
        {
            for (int drawCard = 0; drawCard < 2; drawCard++)
                DeckDrawCard(Player_List[i]);

            Player_List[i].HitUpdate();
        }

        yield return null;
    }

    /**
     * <summary> 更新処理コルーチン </summary>
     */
    private IEnumerator GameUpdate()
    {
        // ゲーム処理関数
        Game();

        // 結果表示
        if ((Player_List[0].MyStatus == Status.STATUS.STATUS_STAND &&
            Player_List[1].MyStatus == Status.STATUS.STATUS_STAND) ||
            Player_List[0].MyStatus == Status.STATUS.STATUS_BURST)
        {
            var winner = Result.FinalResult(Player_List);
            winner = new Color(255, 255, 255);
        }

        yield return null;
    }

    /**
     * <summary> 終了処理コルーチン </summary>
     */
    private IEnumerator GameResult()
    {
        Debug.Log("ゲーム終わり！");
        yield return null;
    }

    /**
     * <summary> カードを引いた時にアニメーションさせるコルーチン </summary>
     */
    private IEnumerator EasingMove( Vector3 fromPos, Vector3 toPos, GameObject moveCard )
    {
        yield return easing.CreateVector3(easing.easeInQuad, fromPos, toPos, 1,
            (x) => moveCard.transform.position = x);
        Debug.Log("カード移動完了！");

        // TODO ここにボタン復活処理が書かれていたがなにかおかしい

        hitButton.interactable = true;
        standButton.interactable = true;
    }
}
