using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Rules;
using View;
using Model;
using UniRx;

namespace Presenter {
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField, Tooltip("x個そろうと消える")]
        int EraseCount = 0;

        [SerializeField, Tooltip("フィールド幅")]
        int FieldWidth = 0;
        [SerializeField, Tooltip("フィールド高さ")]
        int FieldHeight = 0;
        
        [SerializeField, Tooltip("ジェム生成")]
        GemsFactory factory = null;
        [SerializeField, Tooltip("落下中のジェム操作")]
        ControllGems controll = null;
        [SerializeField, Tooltip("落下先決定後の自由落下")]
        FallGems fall = null;

        Field field = null;

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
            field = new Field(size);
            controll.SetUp(factory, field);

            controll.Initialize(Gems.Red, Gems.Green);

            controll.OnResult.Subscribe(results => {
                fall.Initialize(results, field);
            });

            fall.OnDropped.Subscribe(result => {
                foreach (var res in result.List) {
                    field.Drop(res.Line, res.GemType);
                }
                controll.Initialize(Gems.Blue, Gems.Green);
            });
        }

        void Update() {
        }
    }
}
