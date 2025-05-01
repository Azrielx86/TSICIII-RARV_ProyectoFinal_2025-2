using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(DevBoard))]
    public class BoardEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var board = (DevBoard)target;

            if (GUILayout.Button("Detect Pins"))
            {
                var pinsParent = board.transform.Find("PinGroup");
                if (pinsParent is not null)
                {
                    board.pins.Clear();
                    // Bad idea use this xD
                    var pins = pinsParent.GetComponentsInChildren<GpioPin>().OrderBy(p => p.name);
                    board.pins.AddRange(pins);
                    
                    EditorUtility.SetDirty(board);
                    Debug.Log($"Detected {board.pins.Count} pins");
                }
                else
                {
                    Debug.LogWarning("Child object \"PinGroup\" not found");
                }
            }
        }
    }
}