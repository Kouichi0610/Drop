using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Rules;
using View;
using Model;

namespace Presenter {
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField, Tooltip("x個そろうと消える")]
        int EraseCount = 0;

        [SerializeField, Tooltip("フィールド幅")]
        int FieldWidth = 0;
        [SerializeField, Tooltip("フィールド高さ")]
        int FieldHeight = 0;
        
        [SerializeField, Tooltip("落下ジェム")]
        DropGems drop = null;
        [SerializeField, Tooltip("ジェム生成")]
        GemsFactory factory = null;

        /*
            落とすジェムの生成(*2)
            操作
            落下、フィールドに追加
            {   無くなるまで
                消去処理
                ドロップ
            }
            上に戻る
        */

        void Start()
        {
            var size = new FieldSize(FieldWidth, FieldHeight);
            drop.SetUp(factory, size);

            // TODO:ランダム生成
            drop.Initialize(Gems.Red, Gems.Red);
            
        }
    }
}
