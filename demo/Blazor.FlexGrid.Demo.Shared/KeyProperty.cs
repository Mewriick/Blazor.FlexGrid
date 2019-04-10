using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.FlexGrid.Demo.Shared
{
    public class KeyProperty : IEquatable<KeyProperty>
    {
        public object Key { get; set; }

        public KeyProperty(object key)
        {
            this.Key = (object)key;
        }

        public KeyProperty(int key)
        {
            this.Key = (object)key;
        }

        public KeyProperty(String key)
        {
            this.Key = (object)key;
        }

        public KeyProperty(DateTime key)
        {
            this.Key = (object)key;
        }

        public bool Equals(KeyProperty other)
        {
            return this.Key == other.Key;
        }

        public override bool Equals(object other)
        {
            return this.Key == ((KeyProperty)other).Key;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}
