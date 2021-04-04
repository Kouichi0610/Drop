using System.Collections;
using System.Collections.Generic;
using System;

namespace Model {
    public sealed class Gems : Enumeration {
        public static Gems None = new Gems(0, "空");
        public static Gems Red = new Gems(1, "赤");
        public static Gems Green = new Gems(2, "緑");
        public static Gems Blue = new Gems(3, "青");

        public static IEnumerable<Gems> GetAll() {
            return GetAll<Gems>();
        }

        private Gems(int id, string name) 
         : base(id, name) {
        }
    }
}
