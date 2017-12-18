using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    [SerializeField]
    private GameObject card_Pre;                                    // カードオブジェクトのPrefab設定用
    [SerializeField]
    private Canvas canvas;                                          // 親に当たるキャンバス設定用
    [SerializeField]
    private Player player;
    [SerializeField]
    private Player croupier;

    public static List<GameObject> Card_List { get; set; }          // デッキ格納用リスト
    private List<Player> Player_List;                               // プレイヤー格納リスト
    private int drawCnt;
    private int playerNumber = 1;

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

        // ゲーム参加人数分生成
        /*
        for (int i = 0; i < playerNumber; i++)
        {
            var createPlayer = Instantiate(player, this.transform);
            createPlayer.transform.position = new Vector3(0.0f, -134.75f);
            createPlayer.CroupierFlag = false;
            Player_List.Add(createPlayer);
        }
        */
    }

    private void Start()
    {
        player.CroupierFlag = false;
        Player_List.Add(player);
        croupier.CroupierFlag = true;
        Player_List.Add(croupier);

        // 参加者全員に初期手札配布
        for (int i = 0; i < 2; i++)
        {
            for( int drawCard = 0; drawCard < 2; drawCard++)
                DeckDrawCard(Player_List[i]);

            Player_List[i].HitUpdate();
        }
    }

    // Update is called once per frame
    void Update ()
    {
        // ゲーム処理関数
        Game();

        // 結果表示
        if ((Player_List[0].MyStatus == Status.STATUS.STATUS_STAND &&
            Player_List[1].MyStatus == Status.STATUS.STATUS_STAND) ||
            Player_List[0].MyStatus == Status.STATUS.STATUS_BURST)
        {
            var winner = Result.FinalResult(Player_List);
            winner = new Color(255, 255,255);
//            Debug.Log("バチバチの点数バトル");
        }
	}

    // デッキ生成処理関数
    void DeckCreate()
    {
        for (int design = 0; design < (int)Card.Design.DESIGN_MAX; design++)
        {
            for (int number = 1; number < (int)Card.Number.NUMBER_MAX; number++)
            {
                var card = Instantiate(card_Pre, canvas.transform);
                card.transform.position = new Vector3(card.transform.position.x + 350, card.transform.position.y + 150);
                card.GetComponent<Card>().MyDesign = (Card.Design)design;
                card.GetComponent<Card>().MyNumber = (Card.Number)number;
                card.GetComponent<Image>().sprite = card.GetComponent<Card>().GetSprite(number + design * 13);
                Card_List.Add(card);
            }
        }

        var backcard = Instantiate(card_Pre, canvas.transform);
        backcard.transform.position = new Vector3(backcard.transform.position.x + 350, backcard.transform.position.y + 150);
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
        if (player.MyCard.Count < 5)
        {
            // デッキからカードを受け取る
            var drawCard = Card_List[drawCnt++];
            // プレイヤーの手札リストに引いたカードを追加
            player.MyCard.Add(drawCard);
            // カードを引いた人のカード置き場が親になる様にデッキから引いたカードをセット
            drawCard.transform.parent = player.MyHandPos[player.MyCard.Count - 1].transform;
            // 親の位置(カード置き場)にカードを移動
            drawCard.transform.position = player.MyHandPos[player.MyCard.Count - 1].transform.position;
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
                    Debug.Log("Stand");
                    break;
            }
        }

    }



}
