using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
#endif

namespace Chimera
{
    public class TypeDropdownAttribute : PropertyAttribute
    {
        public Type type;

        public TypeDropdownAttribute(Type parentType)
        {
            type = parentType;
        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(TypeDropdownAttribute))]
    public class TypeDropdownDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var typeDropdownAttribute = attribute as TypeDropdownAttribute;

            var subclassTypes = Assembly
                .GetAssembly(typeDropdownAttribute.type)
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeDropdownAttribute.type));
            
            var options = new List<String>();
            options.Add("None");
            var selected = 0;

            foreach (var t in subclassTypes)
            {
                options.Add(t.ToString());
                if (property.stringValue == t.ToString())
                {
                    selected = options.Count - 1;
                }
            }

            selected = EditorGUI.Popup(position, selected, options.ToArray());
            property.stringValue = selected > 0 ? options[selected] : string.Empty;
        }
    }

#endif
}