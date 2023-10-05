using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//Attach this class to lights to make them pulse with users HR and change color depending on what zone they are in
public class SelectionVisualizer : MonoBehaviour
{
    public Heartrate hrData;

    private SelectionLogicSquad selectionLogic;

    // public List<LineRenderer> edgeLines; //Count should be 5
    public LineRenderer[] edgeLines;
    public Material edgeMaterial;
    
    public Color notFatiguedColor;
    public Color isFatiguedColor;

    void Start()
    {
        selectionLogic = GameObject.Find("Right Hand").GetComponent<SelectionLogicSquad>();
        // hrData = GetComponent<Heartrate>();
        InitializeEdgeLines(5);
    }

    void Update()
    {
        //TODO: make lines pulse with HR
        // myLight.intensity = Mathf.PingPong(Time.time, hrData.heartRate);

        //TODO: set color using lerp based on HR value
        // blue for low, red for high
        renderRays(5);
    }

    private void InitializeEdgeLines(int supportVectorCount)
    {
        edgeLines = new LineRenderer[supportVectorCount];

        for (int i = 0; i < supportVectorCount; i++)
        {
            GameObject lineObject = new GameObject("EdgeLine_" + i);
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.material = edgeMaterial;
            lineRenderer.startWidth = 0.025f;
            lineRenderer.endWidth = 0.025f;
            lineRenderer.positionCount = 2;
            edgeLines[i] = lineRenderer;
        }
    }

    private void renderRays(int supportVectorCount)
    {
        // Define the layermask - this is primarly just to ignore the OpenXR raycast object
        int layerMask = ~(1 << LayerMask.NameToLayer("Exclude From Cone Raycast"));

        //Calculate the values of the endpoints of side rays
        for (int i = 0; i < supportVectorCount; i++)
        {
            float currentAngle = i * (360f / supportVectorCount);
            float radius = selectionLogic.selectionDistance * Mathf.Tan(selectionLogic.coneAngle * Mathf.Deg2Rad);
            float x = radius * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
            float y = radius * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

            Vector3 localCirclePoint = new Vector3(x, y, selectionLogic.selectionDistance);
            Vector3 worldCirclePoint = transform.TransformPoint(localCirclePoint);
            // Debug.Log("circlePoint Position for " + i + ": " + worldCirclePoint);

            edgeLines[i].SetPosition(0, transform.position);

            //If the ray hits a collider, limit the length to that
            RaycastHit hit;
            if (Physics.Raycast(transform.position, worldCirclePoint - transform.position, out hit, selectionLogic.selectionDistance, layerMask))
            {
                // If there's a collider, set the end point to the intersection point
                edgeLines[i].SetPosition(1, hit.point);
            }
            else
            {
                // If there's no collider, set the end point to the calculated worldCirclePoint
                edgeLines[i].SetPosition(1, worldCirclePoint);
            }
        }
    }
}
