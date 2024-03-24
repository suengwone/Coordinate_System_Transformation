using UnityEngine;

namespace Personal.Study.Conversion
{
    public class CoordinateSystem
    {
        public static Vector3 ConvertCartesian2Spherical(float distance, float azimuth, float inclination)
        {
            Vector3 result;
            inclination *= Mathf.Deg2Rad;
            azimuth *= Mathf.Deg2Rad;
            
            result.x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            result.y = Mathf.Cos(inclination);
            result.z = Mathf.Sin(inclination) * Mathf.Sin(azimuth);

            return result * distance;
        }
    }
}