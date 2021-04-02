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

        public void Drop(int x, int gem) {
            if (gem == 0) return;
            for (var y = 0; y < Height; y++) {
                if (fields[y][x] == 0) {
                    fields[y][x] = gem;
                    return;
                }
            }
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
            //var f = new Field(6, 12);
            Assert.Fail("TODO:");
        }

    }
}
