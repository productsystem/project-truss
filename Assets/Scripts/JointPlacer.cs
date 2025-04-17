using System.Collections.Generic;
using UnityEngine;

public class JointPlacer : MonoBehaviour
{
    public GameObject joint;
    public GameObject rod;
    [SerializeField] private float snapRadius = 0.5f;

    private List<GameObject> joints = new List<GameObject>();
    private GameObject shadowRod;
    private GameObject firstJoint;
    private GameObject endJoint;

    void Update()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(0))
        {
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
                firstJoint = null;
                endJoint = null;
            }
        }

        if(firstJoint != null)
        {
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
    }

    void CreateRod(GameObject first, GameObject second)
    {
        GameObject rodT = Instantiate(rod, (first.transform.position + second.transform.position)/2, Quaternion.identity);
        rodT.GetComponent<LineRenderer>().SetPosition(0, first.transform.position);
        rodT.GetComponent<LineRenderer>().SetPosition(1, second.transform.position);
    }
}
