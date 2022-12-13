using UnityEditor;

[CustomEditor(typeof(MasterInfo))]
public class AppScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
        base.DrawDefaultInspector();
        EditorGUI.EndDisabledGroup();
    }
}
