using UnityEngine;

namespace Personal.Study.Conversion
{
    public class CoordinateSystem
    {
        public Vector3 ConvertCartesian2Spherical(float distance, float azimuth, float inclination)
        {
            inclination = 90 - inclination;
            inclination *= Mathf.Deg2Rad;
            azimuth *= Mathf.Deg2Rad;
            float X = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float Y = Mathf.Cos(inclination);
            float Z = Mathf.Sin(inclination) * Mathf.Sin(azimuth);

            return new Vector3(X, Y, Z) * distance;
        }
    }
}