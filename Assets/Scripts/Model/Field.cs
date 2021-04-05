using Model.Rules;
using System.Text;
using System;

namespace Model {
    /// <summary>
    /// 落下するジェムを保管するフィールド
    /// 
    /// </summary>
    public sealed class Field : IFieldState {
        public int Width { get { return fields.GetLength(1); } }
        public int Height { get { return fields.GetLength(0); } }

        bool IFieldState.HasGem(int x, int y) {
            return fields[y, x] != 0;
        }
        int IFieldState.PiledHeight(int x) {
            int y = 0;
            for (y = 0; y < Height; y++) {
                if (fields[y, x] == 0) {
                    break;
                }
            }
            return y;
        }


        /// <summary>
        /// ゲームオーバー判定
        /// 真ん中の列が一番上まで積みあがったら
        /// </summary>
        /// <value></value>
        public bool IsGameOver {
            get {
                return fields[Height-1, Width/2] != 0;
            }
        }

        private int[,] fields = null;

        public Field(FieldSize size) {
            fields = new int[size.Height, size.Width];
        }
        // テスト用
        public static Field FromArray(int[,] fields) {
            var w = fields.GetLength(1);
            var h = fields.GetLength(0);

            var res = new Field(new FieldSize(w, h));
            Array.Copy(fields, res.fields, fields.Length);
            return res;
        }

        /// <summary>
        /// x列にジェムを一つ落とす
        /// </summary>
        /// <param name="x">落下列</param>
        /// <param name="gem">落下ジェム</param>
        public void Drop(int x, Gems gem) {
            if (gem == Gems.None) return;
            for (var y = 0; y < Height; y++) {
                if (fields[y, x] == 0) {
                    fields[y, x] = gem.Id;
                    return;
                }
            }
        }

        /// <summary>
        /// 条件を満たしたジェムを消去
        /// 消去後のFieldを生成する
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public Tuple<Field, int> ErasedField(EraseCount count) {
            var erasedCount = 0;

            var f = new EraseField(this.fields);
            while (true) {
                var r = f.Erase(count);
                f = r.Item1;
                var c = r.Item2;
                erasedCount += c;
                if (c == 0) {
                    break;
                }
            }
            var res = Field.FromArray(f.Field);
            return new Tuple<Field, int>(res, erasedCount);
        }

        public override string ToString() {
            var s = new StringBuilder();
            for (var y = 0; y < Height; y++) {
                for (var x = 0; x < Width; x++) {
                    s.AppendFormat("{0}", fields[y,x]);
                }
            }
            return s.ToString();
        }
    }
}

