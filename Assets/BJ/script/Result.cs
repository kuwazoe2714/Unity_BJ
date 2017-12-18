using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result {

    public static Color FinalResult( List<Player> player )
    {
        var playerScore_List = new List<int>(player.Count);

        for(int playerCnt = 0; playerCnt < player.Count; playerCnt++)
        {
            if (player[playerCnt].MyStatus != Status.STATUS.STATUS_BURST)
            {
                playerScore_List.Add(player[playerCnt].GetPlayerScore);
            }
            else
            {
                playerScore_List.Add(0);
            }
            
        }

        // ディーラーと比べて…
        // プレイヤーの方が値が大きいなら
        if (playerScore_List[0] > playerScore_List[1])
        {
            return player[1].GetComponent<Image>().color = new Color(255, 255, 255);
        }
        // ディーラーのが値が大きかったら
        else if(playerScore_List[0] < playerScore_List[1])
        {
            return player[0].GetComponent<Image>().color = new Color(255, 255, 255);
        }
        // 同点の場合
        else
        {
            Debug.Log("引き分け");
        }

        return new Color(255,255,255);
    }
}
