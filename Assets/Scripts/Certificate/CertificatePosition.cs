
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class CertificatePosition : MonoBehaviour
{
    public RectTransform[] objects;
    public RectTransform[] snapPoints;
    public Dictionary<RectTransform, RectTransform> snapPointOccupancy = new();
    //private readonly float overlapThreshold = 0.1f;

    void Start()
    {
        if (objects.Length == 0) return;

        // Randomize the object positions
        Vector3[] positions = new Vector3[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            positions[i] = objects[i].localPosition;
        }

        for (int i = 0; i < positions.Length; i++)
        {
            int randomIndex = Random.Range(i, objects.Length);
            Vector3 temp = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].localPosition = positions[i];
        }

        // To let the grid layout of the snap point already been placed
        Canvas.ForceUpdateCanvases();

        // Initialize snap point occupancy
        foreach (RectTransform snapPoint in snapPoints)
        {
            snapPointOccupancy[snapPoint] = null;
        }

        foreach (var snapPoint in snapPoints)
        {
            foreach (var obj in objects)
            {
                if (snapPoint.position == obj.position)
                {
                    snapPointOccupancy[snapPoint] = obj;
                    break;
                }
            }
        }

        /*
        foreach (var entry in snapPointOccupancy)
        {
            Debug.Log($"Snap point {entry.Key.name} initially occupied by {(entry.Value != null ? entry.Value.name : "None")}");
        }
        */
    }
}
