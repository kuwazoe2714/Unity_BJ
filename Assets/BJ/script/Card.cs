using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    //
    // 列挙体宣言
    //

    // トランプの絵柄情報
    public enum Design
    {
        SPADE = 0,
        CLUB,
        DIAMOND,
        HEART,
        DESIGN_MAX,
    };

    // トランプの数字情報
    public enum Number
    {
        A = 1,
        _2,
        _3,
        _4,
        _5,
        _6,
        _7,
        _8,
        _9,
        _10,
        J,
        Q,
        K,
        NUMBER_MAX,
    };

    //
    // メンバ変数宣言
    //

    public static List<Sprite> cardSprite;             // カードのスプライト一覧

    [SerializeField]
    private Card_image card_image;

    public Design MyDesign { get; set; }
    public Number MyNumber { get; set; }
    public Sprite MySprite { get; set; }

    // 生成された時の処理関数
    private void Awake()
    {
        SetSpriteCards();
    }

    // カードの情報を与える関数(1枚ずつ作るやり方。一応残しておく)
    private void SetSpriteCard()
    {
        var card = Resources.Load<Sprite>("card_spade_01");
        GetComponent<Image>().sprite = card;
    }

    // カードの情報を与える関数(デッキ作ってない？？？)
    public void SetSpriteCards()
    {
        /*
        // カード全体の大きさ(長さ)取得
        var max_Card = new Sprite[card_image.Spade_Card.Length + 
                              card_image.Club_Card.Length +
                              card_image.Diamond_Card.Length +
                              card_image.Heart_Card.Length];
        Debug.Log(max_Card.Length);

        var spade = card_image.Spade_Card;
        var club = card_image.Club_Card;
        var diamond = card_image.Diamond_Card;
        var heart = card_image.Heart_Card;

        spade.CopyTo(max_Card, 0);
        club.CopyTo(max_Card, spade.Length);
        diamond.CopyTo(max_Card, spade.Length + club.Length);
        heart.CopyTo(max_Card, spade.Length + club.Length + diamond.Length);
        */

        var card_List = new List<Sprite>();

        card_List.AddRange(card_image.Spade_Card);
        card_List.AddRange(card_image.Club_Card);
        card_List.AddRange(card_image.Diamond_Card);
        card_List.AddRange(card_image.Heart_Card);
        card_List.Add(card_image.Back_Card);

        cardSprite = card_List;
    }

    public Sprite GetSprite( int SpriteNumber )
    {
        MySprite = cardSprite[SpriteNumber - 1];

        return MySprite;
    }
}
