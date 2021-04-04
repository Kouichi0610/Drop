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
            var f = new Field(new FieldSize(3,2));
            f.Drop(1, Gems.Green);
            Assert.That(f.IsGameOver, Is.False);
            f.Drop(1, Gems.Green);
            Assert.That(f.IsGameOver, Is.True);
        }

        [Test]
        public void EraseField() {
            var array = new int[,] { {0, 1}, {2, 3}, {4, 5}};
            Assert.That(array[1,1], Is.EqualTo(3));
            Assert.That(array.GetLength(0), Is.EqualTo(3));
            Assert.That(array.GetLength(1), Is.EqualTo(2));
            var array2 = new int[array.GetLength(0), array.GetLength(1)];
            Array.Copy(array, array2, array.Length);
            

            Assert.That(array[1,1], Is.EqualTo(3));
            
        }

    }
}

