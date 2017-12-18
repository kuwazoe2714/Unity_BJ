using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    // uGUIを触るための宣言
    [SerializeField]
    private InputField nameField;
    [SerializeField]
    private Image playerImage;
    [SerializeField]
    private Slider playerImageScaleSlider;

    // uGUIから画像を触るための宣言
    [SerializeField]
    private Sprite[] kawaiiSprite;

    [SerializeField]
    private GameObject kawaiiTogglePrefub;

    [SerializeField]
    private RectTransform dest;             // 「Toggle」の親になる箇所

    // 初期化(どちらかと言えばコンストラクタ。生成直後に呼ばれる)
    private void Awake()
    {
        var kawaiiToggle = new Toggle[kawaiiSprite.Length];     // 「kawaiiSprite」のSize分、生成
        for (int i = 0; i < kawaiiSprite.Length; i++)
        {
            var toggle = Instantiate(kawaiiTogglePrefub, dest);

            TriggerEvent triggerEvent = toggle.GetComponent<TriggerEvent>();
            triggerEvent.ParentCanvas = this;
            triggerEvent.MyNumber = i;
        }

    }

    // 左右反転処理
    bool isFlipPlayerImage;
    public bool IsFlipPlayerImage
    {
        set
        {
            isFlipPlayerImage = value;
            UpdatePlayerImageScale();
        }
    }

    // 上下反転処理
    bool isFlipPlayerImage2;
    public bool IsFlipPlayerImage2
    {
        set
        {
            isFlipPlayerImage2 = value;
            UpdatePlayerImageScale();
        }
    }

    float playerImageScale = 0.5f;                  // 表示される画像の大きさを定義&初期化
    public void ChangerPlayerImageScale(float scale)
    {
        playerImageScale = scale;
        UpdatePlayerImageScale();
    }

    void UpdatePlayerImageScale()
    {
        // 表示されている画像の左右反転処理
        Vector3 scale = Vector3.one * (playerImageScale);

        // 「左右反転」のチェックボックスが付いていたら左右反転させる
        if (isFlipPlayerImage)
        {
            scale.x *= -1.0f;
        }

        // 「上下反転」のチェックボックスついていたら上下反転させる
        if (isFlipPlayerImage2)
        {
            scale.y *= -1.0f;
        }

        playerImage.transform.parent.localScale = scale;
    }

    public void ChangePlayerType(int type)
    {
        // 現在の画像と同じ値が返ってきたら処理抜ける
        if (kawaiiSprite.Length <= type || type <= -1)
        {
            return;
        }

        playerImage.sprite = kawaiiSprite[type];

        playerImage.SetNativeSize();
    }


}
