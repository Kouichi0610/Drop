using NUnit.Framework;
using System.Text;

namespace Test {
    /// <summary>
    /// フィールド試作品
    /// </summary>
    class Field {
        public int Width { get { return fields[0].Length; } }
        public int Height { get { return fields.Length; } }

        public int DropLine { get { return Width / 2;} }

        private int[][] fields = null;

        public Field(int width, int height) {
            fields = new int[height][];
            for (var i = 0; i < height; i++) {
                fields[i] = new int[width];
            }
        }

        public bool IsGameOver {
            get {
                return fields[DropLine][Height-1] != 0;
            }
        }

        public static Field FromString(int width, int height, byte[] array) {
            var res = new Field(width, height);

            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    var i = x + y*width;
                    res.fields[y][x] = array[i];
                }
            }

            return res;
        }

        public void Drop(int x, int gem) {
            if (gem == 0) return;
            for (var y = 0; y < Height; y++) {
                if (fields[y][x] == 0) {
                    fields[y][x] = gem;
                    return;
                }
            }
        }

        public int EraseCount() {
            var res = 0;
            var mask = new bool[Height][];
            for (var i = 0; i < Height; i++) {
                mask[i] = new bool[Width];
            }


            for (var y = 0; y < Height; y++) {
                for (var x = 0; x < Width; x++) {
                    var gem = fields[y][x];
                    if (gem == 0) continue;
                    if (mask[y][x]) continue;
                    var cnt = count(x, y, gem, mask);
                    if (cnt >= 4) {
                        res += cnt;
                    }
                }
            }
            return res;
        }

        private int count(int x, int y, int gem, bool[][] mask) {
            if (x < 0 || x >= Width) return 0;
            if (y < 0 || y >= Height) return 0;
            if (mask[y][x]) return 0;

            if (fields[y][x] == gem) {
                mask[y][x] = true;
                var res = 0;
                res++;
                res += count(x-1, y, gem, mask);
                res += count(x+1, y, gem, mask);
                res += count(x, y-1, gem, mask);
                res += count(x, y+1, gem, mask);
                return res;
            }
            return 0;
        }

        /// <summary>
        /// 消えるジェムの数を取得
        /// 4以上つなげると消える
        /// @warning fieldsを破壊する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="gem"></param>
        /// <returns></returns>
        public int Count(int x, int y, int gem) {
            if (x < 0 || x >= Width) return 0;
            if (y < 0 || y >= Height) return 0;

            if (fields[y][x] == gem) {
                var res = 0;
                res++;
                fields[y][x] = 0;
                res += Count(x-1, y, gem);
                res += Count(x+1, y, gem);
                res += Count(x, y-1, gem);
                res += Count(x, y+1, gem);
                return res;
            }
            return 0;
        }


        /// <summary>
        /// フィールドの状態を文字列形式で取得
        /// 010
        /// 111 -> 111010
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var s = new StringBuilder();

            for (var y = 0; y < Height; y++) {
                foreach(var x in fields[y]){
                    s.Append("" + x);
                }
            }
            return s.ToString();
        }
    }
    /// <summary>
    /// 基本ルールに関するテスト。プロトタイプ。
    /// </summary>
    [TestFixture]
    public class ProtoType
    {
        [Test]
        public void 上に積み重なっていくこと() {
            var f = new Field(3,2);
            Assert.That(f.Width, Is.EqualTo(3));
            Assert.That(f.Height, Is.EqualTo(2));
            Assert.That(f.ToString(), Is.EqualTo("000000"));
            f.Drop(0, 3);
            f.Drop(1, 5);
            f.Drop(2, 9);
            Assert.That(f.ToString(), Is.EqualTo("359000"));
            f.Drop(1, 8);
            Assert.That(f.ToString(), Is.EqualTo("359080"));
            // 列が埋まっているところに積んでも変化ない事
            f.Drop(1, 15);
            Assert.That(f.ToString(), Is.EqualTo("359080"));
        }
        
        [Test]
        public void 落とすラインが一番上まで埋まるとゲームオーバーになること() {
            var f = fillColumn(0);
            Assert.That(f.IsGameOver, Is.False);
            f = fillColumn(1);
            Assert.That(f.IsGameOver, Is.True);
            f = fillColumn(2);
            Assert.That(f.IsGameOver, Is.False);
        }
        private Field fillColumn(int line) {
            const int height = 2;
            var res = new Field(3, height);
            for (var i = 0; i < height; i++) {
                res.Drop(line, 1);
            }
            return res;
        }
        [Test]
        public void 四つ以上つなげると消える() {
            var f = Field.FromString(4, 4, new byte[]{
                1,1,0,0,
                1,0,0,0,
                1,0,1,0,
                1,0,0,0});

            Assert.That(f.ToString(), Is.EqualTo("1100100010101000"));

            var count = f.Count(0,0,1);
            Assert.That(count, Is.EqualTo(5));
        }

        [Test]
        public void 消えた後のField確認() {
            var f = Field.FromString(4, 6, new byte[]{
                1,1,2,2,
                1,3,2,2,
                1,4,4,4,
                1,1,7,4,
                5,5,5,4,
                1,5,6,4,
                });
            Assert.That(f.ToString(), Is.EqualTo("112213221444117455541564"));
            var res = f.EraseCount();
            Assert.That(res, Is.EqualTo(20));
            // 内部の情報を破壊しないこと
            Assert.That(f.ToString(), Is.EqualTo("112213221444117455541564"));
        }

    }
}
