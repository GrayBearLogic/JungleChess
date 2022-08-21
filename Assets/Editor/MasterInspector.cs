using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Master))]
public class MasterInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var master = target as Master;
        
        if (master && master.game != null)
        {
            GUILayout.Label(master.game.Board.ToString());
        }
    }
}