using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Card_image : ScriptableObject {

    // それぞれの絵柄カードのデータ格納用
    [SerializeField] private Sprite[] spade_Card;
    [SerializeField] private Sprite[] club_Card;
    [SerializeField] private Sprite[] diamond_Card;
    [SerializeField] private Sprite[] heart_Card;
    [SerializeField] private Sprite back_Card;

    // 設定された値の確保用
    public Sprite[] Spade_Card{ get { return spade_Card; } }
    public Sprite[] Club_Card { get { return club_Card; } }
    public Sprite[] Diamond_Card { get { return diamond_Card; } }
    public Sprite[] Heart_Card { get { return heart_Card; } }
    public Sprite Back_Card { get { return back_Card; } }
}
