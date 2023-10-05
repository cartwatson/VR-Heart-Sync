using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.OpenXR.Input;
using UnityEngine.XR.Interaction.Toolkit;


public class SelectionLogicSquad : MonoBehaviour
{


    [Header("Input Objects")]
    public XRNode xrnode;

    [Header("Selection Cone Settings")]
    public float coneAnlgeMax = 2F;
    public float coneAngleMin = 0.2f;
    public float coneDistance = 10.0f;

    //Posistion and geogrebraic qualities
    public float selectionDistance = 10f;
    public Vector3 centerEndPoint;
    public float coneAngle; //The degree determining how wide the cone is

    //Required objects
    private InputManager inputManager;
    private RadialMenu radialMenu;

    //OpenXR Objects
    private ConeCollider coneCollider;
    private XRGrabInteractable grabbedObject;

    //Collections
    private List<GameObject> selectionCandidates;
    public List<GameObject> objectsInCone;

    //States
    private Heartrate heartRateData;
    private bool isSquadOpen = false;
    private bool useFlashlight = false;


    // Start is called before the first frame update
    void Start()
    {
        coneCollider = GameObject.Find("Right Hand").GetComponent<ConeCollider>();
        heartRateData = GameObject.Find("HeartRate").GetComponent<Heartrate>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        radialMenu = GameObject.Find("RadialMenu").GetComponent<RadialMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        updateConeAngle();
        calculatecenterEndPoint();

        radialMenu.Show(isSquadOpen);

        // Handle 
        if (inputManager.GetRightPrimary())
        {
            onUserSelect();
        }
    }

    private void onUserSelect()
    {
        //Do nothing if no objects are in cone
        if (objectsInCone.Count == 0)
        {
            Debug.Log("User pressed the selection input but nothing was selectable!");
            return;
        }

        else if (objectsInCone.Count > 0)
        {
            Debug.Log("Starting SQUAD");
            StartSquad();
            StartCoroutine(RunSquad());
        }
    }


    private void StartSquad()
    {
        isSquadOpen = true;
        selectionCandidates = objectsInCone.GetRange(0, objectsInCone.Count > 4 ? 4 : objectsInCone.Count);
        //selectionCandidates = objectsInCone.GetRange(0, objectsInCone.Count - 1);
        for (int i = 0; i < selectionCandidates.Count; i ++)
        {
            GameObject obj = selectionCandidates[i];
            Debug.Log("Selection Candidate: " + obj.name);
            
            HighlightObject(obj, i);
        }

        radialMenu.menuOptions = selectionCandidates;
    }

    private void HighlightObject(GameObject obj, int index)
    {
        Outline outline = obj.GetComponent<Outline>();
        outline.OutlineWidth = 2;
        // change highlight color based on order
        switch (index)
        {
            case 0:
                outline.OutlineColor = new Color(0f, 0f, 1f, 1f); // blue
                break;
            case 1:
                outline.OutlineColor = new Color(1f, 0f, 0f, 1f); // red
                break;
            case 2:
                outline.OutlineColor = new Color(0f, 1f, 0f, 1f); // green
                break;
            case 3:
                outline.OutlineColor = new Color(1f, 0.92f, 0.016f, 1f); // yellow
                break;
        }
    }

    private void RemoveHighlight()
    {
        foreach (GameObject obj in selectionCandidates)
        {
            Outline outline = obj.GetComponent<Outline>();
            outline.OutlineWidth = 0; // probably a cleaner way to do this but this works
        }
    }

    private IEnumerator RunSquad()
    {
        // Wait until the user selects something
        yield return new WaitUntil(() => inputManager.GetRightPrimaryRelease());

        Debug.Log("Ending SQUAD " + selectionCandidates);
        Debug.Log("selection cadidates: " + selectionCandidates);
        Debug.Log("radialMenu.GetIndex(): " + radialMenu.GetIndex());
        if (radialMenu.GetIndex() > selectionCandidates.Count) { yield break; }
        GameObject squadWinner = selectionCandidates[radialMenu.GetIndex()];
        Debug.Log("SquadWinner: " + squadWinner.name);

        RemoveHighlight();
        teleportToHand(squadWinner);
        isSquadOpen = false;
        radialMenu.ClearSprites();
    }

    private void teleportToHand(GameObject obj)
    {
        Debug.Log("Teleporting " + obj.name + " to hand.");
        obj.transform.position = radialMenu.transform.position;
    }


    private void updateConeAngle()
    {
        //Calculate coneAngle
        float step = (coneAnlgeMax - coneAngleMin) / 100;
        coneAngle = coneAngleMin + (heartRateData.fatigueValue * 10);
        coneAngle = Mathf.Clamp(coneAngle, coneAngleMin, coneAnlgeMax);

        // Destroy(coneCollider);
        // coneCollider = new ConeCollider(selectionDistance, coneAngle);
        coneCollider.angle = coneAngle;
        coneCollider.distance = selectionDistance;
        // Replace collider object
    }

    private void OnTriggerEnter(Collider candidate)
    {
        if (candidate.GetComponent<XRGrabInteractable>() != null)
        {
            Debug.Log(candidate.gameObject.name + "entered the cone.");
            objectsInCone.Add(candidate.gameObject);
        }
    }

    private void OnTriggerExit(Collider candidate)
    {
        if (candidate.GetComponent<XRGrabInteractable>() != null)
        {
            Debug.Log(candidate.gameObject.name + " exited the cone.");
            objectsInCone.Remove(candidate.gameObject);
        }
    }

    private GameObject selectClosest()
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;

        foreach (GameObject gameObject in objectsInCone)
        {
            Vector3 objectPosition = gameObject.transform.position;
            float distance = distanceToLineSegment(objectPosition, transform.position, centerEndPoint);
            if (distance < closestDistance)
            {
                closestObject = gameObject;
                closestDistance = distance;
            }
            Debug.Log("CLosest Object: " + closestObject.name);
        }
        return closestObject;
    }

    private void calculatecenterEndPoint()
    {
        centerEndPoint = transform.position + (transform.up * -1 * selectionDistance);

        //If raycast hits a gameObject with the "Barrier" tag, shorten length to that point
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, selectionDistance))
        {
            if (hit.collider.gameObject.CompareTag("Barrier"))
            {
                centerEndPoint = hit.point;
            }
        }
    }

    // Calculate the distance from a game object to a line segment
    private float distanceToLineSegment(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        // Calculate the direction of the line segment
        Vector3 lineDirection = lineEnd - lineStart;

        // Project the point onto the line segment
        Vector3 pointOnLine = projectPointOnLineSegment(point, lineStart, lineEnd);

        // Calculate the distance between the point and the projected point on the line
        return Vector3.Distance(point, pointOnLine);
    }

    // Calculate the closest point on a line segment to a given point
    private Vector3 projectPointOnLineSegment(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 lineDirection = lineEnd - lineStart;

        // Calculate the vector from the start of the line to the point
        Vector3 pointVector = point - lineStart;

        // Project the point vector onto the line direction vector
        float projection = Vector3.Dot(pointVector, lineDirection.normalized);
        Vector3 projectedPoint = lineStart + (lineDirection.normalized * projection);

        // Clamp the projected point to the line segment
        projectedPoint = clampPointOnLineSegment(projectedPoint, lineStart, lineEnd);

        return projectedPoint;
    }

    // Clamp a point to a line segment
    private Vector3 clampPointOnLineSegment(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 lineDirection = lineEnd - lineStart;

        // Project the point onto the line direction vector
        float projection = Vector3.Dot(point - lineStart, lineDirection) / lineDirection.sqrMagnitude;
        projection = Mathf.Clamp01(projection);

        // Calculate the closest point on the line segment to the projected point
        Vector3 closestPoint = lineStart + projection * lineDirection;

        return closestPoint;
    }
}
