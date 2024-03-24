using UnityEditor;
using UnityEngine;

namespace Personal.Study.CustormGUI
{
    using Visualization;

    [CustomEditor(typeof(VisualizeRange))]
    public class SphericalRangeGUI : Editor
    {
        private const string MinInclinationKey = "Min Inclination";
        private const string MaxInclinationKey = "Max Inclination";
        private const string MinAzimuthKey = "Min Azimuth";
        private const string MaxAzimuthKey = "Max Azimuth";
        private const string DistanceKey = "Distance";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            VisualizeRange visualization = (VisualizeRange)target;
            SphericalRange sphericalRange = visualization.sphericalRange;

            if(sphericalRange == null)
                return;

            sphericalRange.MinInclination = EditorGUILayout.IntField(MinInclinationKey, (int)sphericalRange.MinInclination);
            sphericalRange.MaxInclination = EditorGUILayout.IntField(MaxInclinationKey, (int)sphericalRange.MaxInclination);
            sphericalRange.MinAzimuth = EditorGUILayout.IntField(MinAzimuthKey, (int)sphericalRange.MinAzimuth);
            sphericalRange.MaxAzimuth = EditorGUILayout.IntField(MaxAzimuthKey, (int)sphericalRange.MaxAzimuth);
            sphericalRange.Distance = EditorGUILayout.IntField(DistanceKey, (int)sphericalRange.Distance);
            
            if(GUILayout.Button("Caculate Positions"))
            {
                visualization.Visualize();
            }
        }
    }
}
