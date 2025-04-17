using System.Collections.Generic;
using UnityEngine;

public class JointPlacer : MonoBehaviour
{
    public GameObject joint;
    public GameObject rod;
    [SerializeField] private float snapRadius = 0.5f;
    public ForceInput forceInput;
    public bool canPlace;

    private List<GameObject> joints = new List<GameObject>();
    private GameObject shadowRod;
    private GameObject firstJoint;
    private GameObject endJoint;

    void Awake()
    {
        canPlace = true;
    }

    void Update()
    {
        
        if(Input.GetMouseButtonDown(0) && canPlace)
        {
            PlaceJoints();
        }

        if(Input.GetMouseButtonDown(1))
        {
            canPlace = false;
            AddForces();
            
        }

        if(firstJoint != null)
        {
            ShadowRod();
        }
    }

    void AddForces()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject newJoint = null;
        foreach(GameObject j in joints)
        {
            if(Vector2.Distance(j.transform.position, pos) < snapRadius)
            {
                newJoint = j;
                break;
            }
        }
        if(newJoint != null)
        {
            forceInput.Show(Input.mousePosition, newJoint);
        }
    }

    public void DrawForceArrow(GameObject joint, Vector2 dir)
    {
        GameObject arrow = Instantiate(rod, rod.transform.position, Quaternion.identity);
        LineRenderer arrowLine = arrow.GetComponent<LineRenderer>();
        arrowLine.startColor = Color.blue;
        arrowLine.endColor = Color.blue;
        arrowLine.startWidth = 0.2f;
        arrowLine.SetPosition(0, joint.transform.position);
        arrowLine.SetPosition(1, joint.transform.position + (Vector3)dir);
    }

    void PlaceJoints()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject newJoint = null;
        foreach(GameObject j in joints)
        {
            if(Vector2.Distance(j.transform.position, pos) < snapRadius)
            {
                newJoint = j;
                break;
            }
        }
        if(newJoint == null)
        {
            newJoint = Instantiate(joint,pos,Quaternion.identity);
        }
        joints.Add(newJoint);
        if(firstJoint == null)
        {
            firstJoint = newJoint;
        }
        else
        {
            endJoint = newJoint;
            CreateRod(firstJoint, endJoint);
            Destroy(shadowRod);
            firstJoint = null;
            endJoint = null;
        }
    }

    void ShadowRod()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(shadowRod != null)
        {
            shadowRod.GetComponent<LineRenderer>().SetPosition(0, firstJoint.transform.position);
            shadowRod.GetComponent<LineRenderer>().SetPosition(1, pos);
        }
        else
        {
            shadowRod = Instantiate(rod, firstJoint.transform.position, Quaternion.identity);
        }
    }

    void CreateRod(GameObject first, GameObject second)
    {
        GameObject rodT = Instantiate(rod, (first.transform.position + second.transform.position)/2, Quaternion.identity);
        rodT.GetComponent<LineRenderer>().SetPosition(0, first.transform.position);
        rodT.GetComponent<LineRenderer>().SetPosition(1, second.transform.position);
    }
}
