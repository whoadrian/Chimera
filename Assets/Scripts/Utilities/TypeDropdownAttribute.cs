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
    /// <summary>
    /// Attribute for displaying a dropdown with children inheriting from a specified class type. Use on a string type.
    /// </summary>
    public class TypeDropdownAttribute : PropertyAttribute
    {
        public Type type;

        public TypeDropdownAttribute(Type parentType)
        {
            type = parentType;
        }
    }

#if UNITY_EDITOR

    /// <summary>
    /// Drawer for the [TypeDropdown] attribute. Uses reflection to find children classes of a specified type, stores them as string.
    /// </summary>
    [CustomPropertyDrawer(typeof(TypeDropdownAttribute))]
    public class TypeDropdownDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var typeDropdownAttribute = attribute as TypeDropdownAttribute;

            // Get children of type
            var subclassTypes = Assembly
                .GetAssembly(typeDropdownAttribute.type)
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeDropdownAttribute.type));
            
            // Build dropdown options, starting with "None"
            var options = new List<String>();
            options.Add("None");
            var selected = 0;

            // Check current selected index
            foreach (var t in subclassTypes)
            {
                options.Add(t.ToString());
                if (property.stringValue == t.ToString())
                {
                    selected = options.Count - 1;
                }
            }

            // Draw dropdown
            selected = EditorGUI.Popup(position, selected, options.ToArray());
            
            // Set dropdown data
            property.stringValue = selected > 0 ? options[selected] : string.Empty;
        }
    }

#endif
}