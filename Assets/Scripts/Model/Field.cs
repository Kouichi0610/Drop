using System.Collections;
using System.Collections.Generic;
using Model.Rules;
using System.Linq;

namespace Model {
    /// <summary>
    /// 落下するジェムを保管するフィールド
    /// 
    /// </summary>
    public sealed class Field {
        public int Width { get { return fields[0].Length; } }
        public int Height { get { return fields.Length; } }

        /// <summary>
        /// ゲームオーバー判定
        /// 真ん中の列が一番上まで積みあがったら
        /// </summary>
        /// <value></value>
        public bool IsGameOver {
            get {
                return fields[Width/2][Height-1] != 0;
            }
        }

        private int[][] fields = null;

        public Field(FieldSize size) {
            fields = new int[size.Height][];
            for (var i = 0; i < size.Height; i++) {
                fields[i] = new int[size.Width];
            }
        }

        /// <summary>
        /// x列にジェムを一つ落とす
        /// </summary>
        /// <param name="x">落下列</param>
        /// <param name="gem">落下ジェム</param>
        public void Drop(int x, Gems gem) {
            if (gem == Gems.None) return;
            for (var y = 0; y < Height; y++) {
                if (fields[y][x] == 0) {
                    fields[y][x] = gem.Id;
                    return;
                }
            }
        }

        /// <summary>
        /// 条件を満たしたジェムを消去
        /// 消去後のFieldを生成する
        /// </summary>
        /// <returns></returns>
        public Field ErasedField() {
            return this;
        }

        public override string ToString() {
            var s = fields.SelectMany(x => x)
                .Select(x => (char)('0' + x))
                .ToArray();
            return new string(s);
        }

        // 一時Field 消えるのが無くなるまで
    }
}

