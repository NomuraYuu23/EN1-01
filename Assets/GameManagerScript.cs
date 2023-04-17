using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //配列の宣言
    int[] map;

    //配列の表示
    void PrintArray()
    {
        //追加。文字列の宣言と初期化
        string debugText = "";
        for (int i = 0; i < map.Length; i++)
        {
            //変更。文字列に結合していく
            debugText += map[i].ToString() + ",";
        }
        //結合した文字列を出力
        Debug.Log(debugText);
    }

    //インデックスの取得
    int GetPlayerIndex()
    {
        //要素数はmap.Lengthで取得
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == 1)
            {
                return i;
            }
        }
        return -1;
    }

    //移動
    bool MoveNumber(int number, int moveFrom, int moveTo)
    {
        //移動先が範囲外なら移動不可
        if(moveTo<0 || moveTo >= map.Length)
        {
            //動けない条件を先に書き、リターンする、早期リターン
            return false;
        }
        //移動先に2(箱)がいたら
        if (map[moveTo] == 2)
        {
            //どの方向へ移動するかを算出
            int velocity = moveTo - moveFrom;
            /*
             プレイヤーの移動先から、さらに先へ2(箱)を移動させる。
            箱の移動処理。MoveNumberメソッド内でMoveNumberメソッドを
            呼び、根拠が再帰している。移動不可をboolで記録
             */
            bool succes = MoveNumber(2, moveTo, moveTo + velocity);
            //もし箱が移動失敗したら、プレイヤーの移動も失敗
            if(!succes)
            {
                return false;
            }
        }

        //移動処理
        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //配列の実態の作成と初期化
        map = new int[] { 0, 0, 0, 1, 0, 2, 0, 0, 0 };
        PrintArray();
    }

    // Update is called once per frame
    void Update()
    {
        //Dキーを押した瞬間
        if(Input.GetKeyDown(KeyCode.D)) {
            //見つからなかった時のために-1で初期化
            int playerIndex = GetPlayerIndex();
            //移動関数
            MoveNumber(1, playerIndex, playerIndex + 1);
            PrintArray();

        }
        //Aキーを押した瞬間
        if (Input.GetKeyDown(KeyCode.A))
        {
            //見つからなかった時のために-1で初期化
            int playerIndex = GetPlayerIndex();
            //移動関数
            MoveNumber(1, playerIndex, playerIndex - 1);
            PrintArray();

        }
    }
}
