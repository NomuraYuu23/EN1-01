using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Compilation;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;

    public GameObject clearText;

    //配列の宣言
    int[,] map;//変更。二次元配列で宣言
    GameObject[,] field;//ゲーム管理用の配列

    //インデックスの取得
    Vector2Int GetPlayerIndex()
    {
        //要素数はmap.Lengthで取得
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                    if (field[y,x] == null) {  continue; }
                    if (field[y,x].tag == "Player") { return new Vector2Int(x,y); }
            }
        }
        return new Vector2Int(-1, -1);
    }



    //移動
    bool MoveNumber(int tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        //縦軸横軸の配列外参照をしてないか
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }

        //Boxタグを持っていたら再帰処理
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(tag, moveTo, moveTo + velocity);
            if(!success) { return false; }
        }

        //GameObjectの座標(position)を移動させてからインデックスの入れ替え
        field[moveFrom.y, moveFrom.x].transform.position =
            new UnityEngine.Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;




        return true;
    }

    //クリア判定
    bool IsCleard()
    {
        //Vector2Int型の可変長配列の作成
        List<Vector2Int> goals = new List<Vector2Int>();

        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                //格納場所か否かを判断
                if (map[y,x] == 3)
                {
                    //格納場所のインデックスを控えておく
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        //要素数はgoals.Countで取得
        for(int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if(f == null || f.tag != "Box")
            {
                //一つでも箱がなかったら条件未達成
                return false;
            }
        }
        //条件未達成でなければ条件達成
        return true;


    }

    // Start is called before the first frame update
    void Start()
    {

    //配列の実態の作成と初期化
    map = new int[,]//変更。わかりやすく3x5サイズ
        {
            {0,0,0,0,0 },
            {0,3,0,3,0 },
            {0,0,1,2,0 },
            {0,2,3,2,0 },
            {0,0,0,0,0 },
        };
        field = new GameObject
        [
            map.GetLength(0),
            map.GetLength(1)
        ];
        string debugText = "";
        //変更。二重for文で二次元配列の情報を出力
        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0;x < map.GetLength(1); x++)
            {
                debugText += map[y, x].ToString() + ",";
                if (map[y,x] == 1)
                {
                    //追加
                    field[y,x] = Instantiate(
                        playerPrefab,
                        new UnityEngine.Vector3(x, map.GetLength(0) - y, 0),
                        UnityEngine.Quaternion.identity
                        );
                }
                else if(map[y, x] == 2)
                {
                    //追加
                    field[y, x] = Instantiate(
                        boxPrefab,
                        new UnityEngine.Vector3(x, map.GetLength(0) - y, 0),
                        UnityEngine.Quaternion.identity
                        );
                }
                else if (map[y, x] == 3)
                {
                    //追加
                    field[y, x] = Instantiate(
                        goalPrefab,
                        new UnityEngine.Vector3(x, map.GetLength(0) - y, 0.01f),
                        UnityEngine.Quaternion.identity
                        );
                }

            }
            debugText += "\n";//改行
        }
        Debug.Log(debugText);
    }

    // Update is called once per frame
    void Update()
    {
        //Dキーを押した瞬間
        if(Input.GetKeyDown(KeyCode.D)) {
            //見つからなかった時のために-1で初期化
            Vector2Int playerIndex = GetPlayerIndex();
            Vector2Int playerIndexNext = GetPlayerIndex();
            playerIndexNext.x += 1;
            //移動関数
            MoveNumber(1, playerIndex, playerIndexNext);
        }

        //Aキーを押した瞬間
        if (Input.GetKeyDown(KeyCode.A))
        {
            //見つからなかった時のために-1で初期化
            Vector2Int playerIndex = GetPlayerIndex();
            Vector2Int playerIndexNext = GetPlayerIndex();
            playerIndexNext.x -= 1;
            //移動関数
            MoveNumber(1, playerIndex, playerIndexNext);
        }

        //Wキーを押した瞬間
        if (Input.GetKeyDown(KeyCode.W))
        {
            //見つからなかった時のために-1で初期化
            Vector2Int playerIndex = GetPlayerIndex();
            Vector2Int playerIndexNext = GetPlayerIndex();
            playerIndexNext.y -= 1;
            //移動関数
            MoveNumber(1, playerIndex, playerIndexNext);
        }

        //Sキーを押した瞬間
        if (Input.GetKeyDown(KeyCode.S))
        {
            //見つからなかった時のために-1で初期化
            Vector2Int playerIndex = GetPlayerIndex();
            Vector2Int playerIndexNext = GetPlayerIndex();
            playerIndexNext.y += 1;
            //移動関数
            MoveNumber(1, playerIndex, playerIndexNext);
        }

        if (IsCleard())
        {
            Debug.Log("clear!");
            //ゲームオブジェクトのSetActiveメソッドを使い有効化
            clearText.SetActive(true);
        }

    }
 
}
