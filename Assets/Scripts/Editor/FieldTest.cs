using NUnit.Framework;
using Model;
using Model.Rules;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Test {
    [TestFixture]
    public class FieldTest {
        [Test]
        public void 生成() {
            var f = new Field(new FieldSize(3,2));
            Assert.That(f.Width, Is.EqualTo(3));
            Assert.That(f.Height, Is.EqualTo(2));

            Assert.That(f.ToString(), Is.EqualTo("000000"));
        }
        [Test]
        public void ドロップ() {
            var f = new Field(new FieldSize(3,2));
            f.Drop(0, Gems.Red);
            f.Drop(0, Gems.Blue);
            f.Drop(2, Gems.Green);
            Assert.That(f.ToString(), Is.EqualTo("102300"));
        }
        [Test]
        public void ゲームオーバー判定() {
            var f = new Field(new FieldSize(5,2));
            f.Drop(2, Gems.Green);
            Assert.That(f.IsGameOver, Is.False);
            f.Drop(2, Gems.Green);
            Assert.That(f.IsGameOver, Is.True);
        }

        [Test]
        public void EraseField() {
            var f = new EraseField(new int[,]{
                {1, 1, 2, 2},
                {1, 0, 2, 7},
                {1, 3, 2, 8},
                {1, 4, 5, 9},
                {1, 1, 6, 9},
            });

            var c = new EraseCount(4);

            var res = f.Erase(c);

            var erasedField = res.Item1;
            var erasedCount = res.Item2;
            // 1だけ消えること
            Assert.That(erasedCount, Is.EqualTo(7));
            Assert.That(erasedField.ToString(), Is.EqualTo("00220027032804590069"));
        }

        [Test]
        public void EraseField_消せなかった場合() {
            var f = new EraseField(new int[,]{
                {1, 1, 2, 2},
                {1, 0, 2, 3},
                {4, 4, 3, 3},
                {1, 4, 5, 9},
                {1, 1, 6, 9},
            });
            var c = new EraseCount(4);
            var res = f.Erase(c);
            var erasedField = res.Item1;
            var erasedCount = res.Item2;
            Assert.That(erasedCount, Is.EqualTo(0));
            Assert.That(erasedField.ToString(), Is.EqualTo("11221023443314591169"));
        }

        [Test]
        public void StackFlowチェック() {
            const int len = 20;
            var fld = new int[len, len];
            for (var i = 0; i < len; i++) {
                for (var j = 0; j < len; j++) {
                    fld[i,j] = 1;
                }
            }
            var f = new EraseField(fld);
            var c = new EraseCount(len*len);
            var erasedCount = f.Erase(c).Item2;
            Assert.That(erasedCount, Is.EqualTo(len*len));
        }

        [Test]
        public void Field消去() {
            var before = Field.FromArray(new int[,]{
                {4,4,4,1},
                {2,3,4,1},
                {3,3,3,1},
                {2,7,7,5},
                {1,7,7,6},
            });
            Assert.That(before.ToString(), Is.EqualTo("44412341333127751776"));
            var c = new EraseCount(4);

            var res = before.ErasedField(c);

            var after = res.Item1;
            var erasedCount = res.Item2;

            // 揃ったジェムが消えていること
            Assert.That(erasedCount, Is.EqualTo(12));
            Assert.That(after.ToString(),  Is.EqualTo("00012001000120051006"));

            // 元インスタンスに影響が無いこと
            Assert.That(before.ToString(), Is.EqualTo("44412341333127751776"));
        }
    }
}

