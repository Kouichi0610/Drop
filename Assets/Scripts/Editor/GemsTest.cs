using NUnit.Framework;
using Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Test {
    [TestFixture]
    public class GemsTest {
        [Test]
        public void GemsAll() {
            var gems = Gems.GetAll();

            var count = gems.Count();
            Assert.That(count, Is.EqualTo(4));
        }
    }
}

