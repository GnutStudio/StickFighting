using System;
using UnityEngine;

namespace Puppy.Engine.UnityInspector {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SpriteFieldAttribute : PropertyAttribute {
        public readonly float size;

        public SpriteFieldAttribute(float size = 64f) {
            this.size = size;
        }
    }
}