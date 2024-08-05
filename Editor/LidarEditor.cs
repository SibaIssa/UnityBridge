using UnityEngine;
using UnityEditor;
using ProBridge.Tx.Sensor; // Ensure this matches your namespace

[CustomEditor(typeof(Lidar))]
public class LidarEditor : Editor
{
    // SerializedProperty allows handling property changes and enabling the inspector to edit them
    private SerializedProperty lidarType;
    private SerializedProperty height;
    private SerializedProperty width;
    private SerializedProperty angleMin;
    private SerializedProperty angleMax;
    private SerializedProperty angleIncrement;
    private SerializedProperty timeIncrement;
    private SerializedProperty scanTime;

    private void OnEnable()
    {
        // Cache the SerializedProperty objects
        lidarType = serializedObject.FindProperty("lidarType");
        height = serializedObject.FindProperty("height");
        width = serializedObject.FindProperty("width");
        angleMin = serializedObject.FindProperty("angleMin");
        angleMax = serializedObject.FindProperty("angleMax");
        angleIncrement = serializedObject.FindProperty("angleIncrement");
        timeIncrement = serializedObject.FindProperty("timeIncrement");
        scanTime = serializedObject.FindProperty("scanTime");
    }

    public override void OnInspectorGUI()
    {
        // Update serialized object representation
        serializedObject.Update();

        // Draw all properties except those we want to conditionally show
        DrawPropertiesExcluding(serializedObject, "lidarType", "height", "width", "angleMin", "angleMax", "angleIncrement", "timeIncrement", "scanTime");

        // Draw lidarType enum field
        EditorGUILayout.PropertyField(lidarType);

        // Cast lidarType to LidarType enum
        Lidar.LidarType type = (Lidar.LidarType)lidarType.enumValueIndex;

        // Display fields based on the selected LidarType
        switch (type)
        {
            case Lidar.LidarType.PointCloud2:
                // Draw fields specific to PointCloud2
                EditorGUILayout.PropertyField(height);
                EditorGUILayout.PropertyField(width);
                break;
            case Lidar.LidarType.LaserScan:
                // Draw fields specific to LaserScan
                EditorGUILayout.PropertyField(angleMin);
                EditorGUILayout.PropertyField(angleMax);
                EditorGUILayout.PropertyField(angleIncrement);
                EditorGUILayout.PropertyField(timeIncrement);
                EditorGUILayout.PropertyField(scanTime);
                break;
        }

        // Apply changes to the serialized object
        serializedObject.ApplyModifiedProperties();
    }
}
