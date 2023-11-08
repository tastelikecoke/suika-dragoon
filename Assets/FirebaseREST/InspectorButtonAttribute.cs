using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FirebaseREST
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InspectorButtonAttribute : System.Attribute
    {
        public string label;
        public bool drawAbove;

        public InspectorButtonAttribute() : this(null) { }

        public InspectorButtonAttribute(string label, bool drawAbove = false)
        {
            this.label = label;
            this.drawAbove = drawAbove;
        }
    }

}
