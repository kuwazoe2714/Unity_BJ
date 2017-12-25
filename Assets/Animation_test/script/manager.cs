using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manager : MonoBehaviour {

    // カード情報を持ったプレハブ
    [SerializeField]
    private GameObject card_Pre;
    // 親に当たるキャンバス設定用
    [SerializeField]
    private Canvas canvas;
    // プレイヤーの手札Panel作成
    [SerializeField]
    private Image playerParent;
    // ボタンを設定する用
    [SerializeField]
    private Button hitButton;


    public static List<GameObject> Card_List { get; set; }
    private List<GameObject> Player_Hand;
    private int drawcnt;

    private void Awake()
    {
        // プレイヤー手札リスト生成
        Player_Hand = new List<GameObject>();
        // デッキのリスト生成
        Card_List = new List<GameObject>();
    }

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(Update());
    }
	
    void DeckCreate()
    {
        for (int design = 0; design < (int)Card.Design.DESIGN_MAX; design++)
        {
            for (int number = 1; number < 3; number++)
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
    }

    public void DeckDrawCard()
    {
        // デッキからカードを受け取る
        var drawCard = Card_List[drawcnt++];
        // プレイヤーの手札リストに引いたカードを追加
        Player_Hand.Add(drawCard);
        // カードを引いた人のカード置き場が親になる様にデッキから引いたカードをセット
        drawCard.transform.parent = playerParent.transform;
        // 親の位置(カード置き場)にカードを移動
        var from = drawCard.transform.position;
        var to = playerParent.transform.position;

        StartCoroutine(EasingUpdate( from, to, drawCard ));
        // TODO アニメーション終わるまでボタン押せない様にする
        hitButton.interactable = false;
    }

    /**
     * <summary> ゲーム更新コルーチン </summary>
     */
     private IEnumerator Update()
    {
        yield return GameInit();
        yield return GameUpdate();
        yield return GameFinish();
    }

    /**
     * <summary> 初期化処理コルーチン </summary>
     */
    private IEnumerator GameInit()
    {
        DeckCreate();
        drawcnt = 0;

        for (int i = 0; i < 2; i++)
        {
            DeckDrawCard();
            yield return test_wait.Create();
        }

        yield return null;
    }

    /**
     * <summary> 更新処理コルーチン </summary>
     */
     private IEnumerator GameUpdate()
    {
        // デッキにカードがあるなら引くことができる
        while (drawcnt < Card_List.Count)
        {
            Debug.Log(drawcnt);

            // ボタンが押されていないかチェック
            if (!Test_player.buttonOn)
            {
                yield return new Test_player();
            }
            else
            {
                DeckDrawCard();
                yield return test_wait.Create();
            }
        }
        yield return null;
    }

    /**
     * <summary> 終了処理コルーチン </summary>
     */
     private IEnumerator GameFinish()
    {
        Debug.Log("カード全部引ききった!");
        yield return null;
    }

    /**
     * <summary> エフェクトの更新コルーチン </summary>
     */
    private IEnumerator EasingUpdate( Vector3 frompos, Vector3 topos, GameObject Card )
    {
        yield return easing.CreateVector3(easing.easeInQuad, frompos, topos, 2,(v) => Card.transform.position = v);
        test_wait.WaitRelease();
        Test_player.buttonOn = false;
        hitButton.interactable = true;
        Debug.Log("更新おわおわり");
    }

}
