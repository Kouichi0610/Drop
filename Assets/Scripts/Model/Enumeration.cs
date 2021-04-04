using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Model {
    public abstract class Enumeration : IComparable {
        public string Name { get; private set; }
        public int Id { get; private set; }

        protected Enumeration(int id, string name) {
            Name = name;
            Id = id;
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration {
            return typeof(T).GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.DeclaredOnly
                )
                .Select(f => f.GetValue(null))
                .Cast<T>();
        }

        public override bool Equals(object obj) {
            var otherValue = obj as Enumeration;
            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);
            return typeMatches && valueMatches;
        }

        public int CompareTo(object other) {
            return Id.CompareTo(((Enumeration)other).Id);
        }
    }
}
