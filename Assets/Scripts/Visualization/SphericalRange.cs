using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Personal.Study.Visualization
{
    [Serializable]
    public class SphericalRange : Editor
    {
        private Vector3 initPoint;
        public Vector3 InitPoint
        {
            get => initPoint;
            set
            {
                if(initPoint == value)
                    return;
                initPoint = value;
                ChangeProperty();
            }
        }

        private (float min, float max) inclination;
        public int InclinationGap
        {
            get => (int)(MaxInclination - MinInclination);
        }
        public float[] GetInclinationArray()
            => Enumerable
                .Range((int)MinInclination, InclinationGap)
                .Select(x => (float)x)
                .ToArray();
        public float MinInclination
        {
            get => inclination.min;
            set
            {
                if(value == inclination.min)
                    return;
                else if(inclination.max <= value)
                    inclination.min = inclination.max;
                else if(value < -180)
                    inclination.min = -180;
                else
                    inclination.min = value;
                
                ChangeProperty();
            }
        }
        public float MaxInclination
        {
            get => inclination.max;
            set
            {
                if(value == inclination.max)
                    return;
                else if(inclination.min >= value)
                    inclination.max = inclination.min;
                else if(value > 180)
                    inclination.max = 180;
                else
                    inclination.max = value;

                ChangeProperty();
            }
        }

        private (float min, float max) azimuth;
        public int AzimuthGap
        {
            get => (int)(MaxAzimuth - MinAzimuth);
        }
        public float[] GetAzimuthArray()
            => Enumerable
                .Range((int)MinAzimuth, AzimuthGap)
                .Select(x => (float)x)
                .ToArray();
        public float MinAzimuth
        {
            get => azimuth.min;
            set
            {
                if(value == azimuth.min)
                    return;
                else if(azimuth.max <= value)
                    azimuth.min = azimuth.max;
                else if(value < -180)
                    azimuth.min = -180;
                else
                    azimuth.min = value;

                ChangeProperty();
            }
        }
        public float MaxAzimuth
        {
            get => azimuth.max;
            set
            {
                if(value == azimuth.max)
                    return;
                else if(azimuth.min >= value)
                    azimuth.max = azimuth.min;
                else if(value > 180)
                    azimuth.max = 180;
                else
                    azimuth.max = value;

                ChangeProperty();
            }
        }

        private float distance;
        public float Distance
        {
            get => distance;
            set 
            {
                if(distance == value || value <= 0)
                    return;
                else
                    distance = value;
                ChangeProperty();
            }
        }

        private Action PropertyChange;
        public void SetEventOnChangeProperty(Action onChange)
            => PropertyChange = onChange;

        private void ChangeProperty()
            => PropertyChange?.Invoke();

    }
}