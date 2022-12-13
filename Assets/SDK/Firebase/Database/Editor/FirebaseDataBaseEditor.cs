using UnityEditor;

[CustomEditor(typeof(FireBaseDatabase))]
public class FirebaseDataBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
        base.DrawDefaultInspector();
        FireBaseDatabase myTarget = (FireBaseDatabase)target;
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Status", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Connection", myTarget.IsOnline ? "Online" : "Offline");

        if (myTarget.IsAuthenticated == false)
        {
            EditorGUILayout.LabelField("Authentication", "Not Authenticate");
        }
        else
        {
            EditorGUILayout.LabelField("Auth UID", myTarget.AuthUserInfo.authId);
        }
    }
}
