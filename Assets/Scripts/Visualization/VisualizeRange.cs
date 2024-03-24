using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine.Jobs;

namespace Personal.Study.Visualization
{
    using Conversion;
    using Unity.Burst;

    [RequireComponent(typeof(Transform))]
    public class VisualizeRange : MonoBehaviour
    {
        [HideInInspector]
        public SphericalRange sphericalRange;
        private ObjectPool<Transform> objectPool;
        private List<Transform> visualObjects;
        private bool isProcess = false;
        private GameObject root;

        private void Awake()
        {
            sphericalRange = new ();
            sphericalRange.InitPoint = this.transform.position;
            sphericalRange.SetEventOnChangeProperty(UpdatePositionOnJob);
        }

        private void Start()
        {
            root = new GameObject("Root");
            objectPool = new (100);
            visualObjects = objectPool.GetObject(100);
            visualObjects.ForEach(x => x.SetParent(root.transform));
        }

        public void Visualize()
        {
            System.Diagnostics.Stopwatch sw = new ();
            sw.Start();
            
            int totalCount = (sphericalRange.InclinationGap + 1) * (sphericalRange.AzimuthGap + 1);

            CheckObjectCount(totalCount);

            int idx = 0;
            for(int i = 0; i <= sphericalRange.InclinationGap; i++)
            {
                for(int j = 0; j <= sphericalRange.AzimuthGap; j++)
                {
                    visualObjects[idx].transform.position = CoordinateSystem.ConvertCartesian2Spherical(sphericalRange.Distance, j, i);
                    idx++;
                }
            }

            sw.Stop();
            Debug.Log($"Normal Process Time : {sw.ElapsedMilliseconds}");
        }

        private void UpdatePositionOnJob()
        {
            if(isProcess) return;
            isProcess = true;
            
            System.Diagnostics.Stopwatch sw = new ();
            sw.Start();

            var azimuths = new NativeArray<float>(sphericalRange.GetAzimuthArray(), Allocator.TempJob);
            var inclinations = new NativeArray<float>(sphericalRange.GetInclinationArray(), Allocator.TempJob);

            int totalCount = azimuths.Length * inclinations.Length;

            CheckObjectCount(totalCount);
            
            TransformAccessArray transformAccessArray = new (visualObjects.ToArray());

            var jobHandle = new UpdatePosition
            {
                distance = sphericalRange.Distance,
                azimuths = azimuths,
                inclinations = inclinations,
            }.Schedule(transformAccessArray);


            jobHandle.Complete();
            isProcess = false;

            sw.Stop();
            Debug.Log($"Normal Process Time : {sw.ElapsedMilliseconds}");
        }

        private void CheckObjectCount(int totalCount)
        {
            if (totalCount > visualObjects.Count)
            {
                var addedObjects = objectPool.GetObject(totalCount - visualObjects.Count);
                addedObjects.ForEach(x => x.SetParent(root.transform));
                visualObjects.AddRange(addedObjects);
            }
            else if (totalCount < visualObjects.Count)
            {
                int remainCount = visualObjects.Count - totalCount;
                var returnList = visualObjects.Take(remainCount);
                objectPool.ReturnObject(ref returnList);

                visualObjects.RemoveRange(0, remainCount);
            }

            visualObjects.ForEach(x => x.gameObject.SetActive(true));
        }
    }
    
    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    public struct UpdatePosition : IJobParallelForTransform
    {
        [ReadOnly]
        public float distance;
        [ReadOnly]
        public NativeArray<float> azimuths, inclinations;

        public void Execute(int index, TransformAccess transform)
        {
            Vector3 newPosition;

            float inclination = Mathf.Deg2Rad * (inclinations[index % inclinations.Length]);
            float azimuth = Mathf.Deg2Rad * (azimuths[index / inclinations.Length]);

            newPosition.x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            newPosition.y = Mathf.Cos(inclination);
            newPosition.z = Mathf.Sin(inclination) * Mathf.Sin(azimuth);

            transform.position = newPosition * distance;
        }
    }

}