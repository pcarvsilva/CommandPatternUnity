using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

// IngredientDrawer
[CustomPropertyDrawer(typeof(CommandSubscription))]
public class CommandSubscriptionPropertyDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label, true);
        EditorGUI.BeginProperty(position, label, property);
        if (property.isExpanded)
        {
            Type type = typeof(ICommand);
            List<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p)).ToList<Type>();

            List<string> choices = new List<string>();
            foreach(Type t in types)
            {
                choices.Add(t.FullName);
            }
            choices[0] = "None";
            int _choiceIndex = 0;
            try
            {
                string typeName = property.FindPropertyRelative("commandType").stringValue;
                _choiceIndex = choices.IndexOf(typeName);
            }
            catch { }
            if (_choiceIndex <= 0) _choiceIndex = 0;
            _choiceIndex = EditorGUI.Popup(new Rect(position.xMin + 30f, position.yMax - 20f, position.width - 30f, 20f), _choiceIndex, choices.ToArray());
            if (_choiceIndex != 0)
                property.FindPropertyRelative("commandType").stringValue = choices[_choiceIndex];
            else
                property.FindPropertyRelative("commandType").stringValue = null;
        }
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
            return EditorGUI.GetPropertyHeight(property) + 20f;
        return EditorGUI.GetPropertyHeight(property);
    }
}