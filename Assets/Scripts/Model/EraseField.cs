using System;
using System.Text;
using Model.Rules;

namespace Model {
    /// <summary>
    /// Field消去判定用の一時クラス
    /// </summary>
    public sealed class EraseField {
        public int[,] Field { get { return fields; } }
        
        private int width { get; set; }
        private int height { get; set; }
        private int[,] fields = null;

        public EraseField(int[,] src) {
            width = src.GetLength(1);
            height = src.GetLength(0);
            fields = new int[height, width];
            Array.Copy(src, fields, src.Length);
        }

        /// <summary>
        /// 消去できる箇所を一か所だけ消去、
        /// 消した後のEraseFieldと消したジェムの数を返す
        /// </summary>
        /// <param name="count">揃うために必要な数</param>
        /// <returns></returns>
        public Tuple<EraseField, int> Erase(EraseCount count) {
            var mask = new bool[height, width];
            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    var gem = fields[y, x];
                    if (gem == 0) continue;
                    if (mask[y, x]) continue;
                    var cnt = eraseCount(x, y, gem, mask);
                    if (cnt >= count.Count) {
                        erase(x, y, gem);
                        return new Tuple<EraseField, int>(this, cnt);
                    }
                }
            }
            return new Tuple<EraseField, int>(this, 0);
        }

        private int eraseCount(int x, int y, int gem, bool[,] mask) {
            if (x < 0 || x >= width) return 0;
            if (y < 0 || y >= height) return 0;

            if (fields[y, x] != gem) return 0;
            if (mask[y,x]) return 0;

            mask[y,x] = true;
            var res = 0;
            res++;
            res += eraseCount(x-1, y, gem, mask);
            res += eraseCount(x+1, y, gem, mask);
            res += eraseCount(x  , y-1, gem, mask);
            res += eraseCount(x  , y+1, gem, mask);
            return res;
        }

        private void erase(int x, int y, int gem) {
            if (x < 0 || x >= width) return;
            if (y < 0 || y >= height) return;
            if (fields[y, x] != gem) return;

            fields[y, x] = 0;
            erase(x-1, y, gem);
            erase(x+1, y, gem);
            erase(x  , y-1, gem);
            erase(x  , y+1, gem);
        }

        public override string ToString() {
            var s = new StringBuilder();
            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    s.AppendFormat("{0}", fields[y,x]);
                }
            }
            return s.ToString();
        }
    }
}
