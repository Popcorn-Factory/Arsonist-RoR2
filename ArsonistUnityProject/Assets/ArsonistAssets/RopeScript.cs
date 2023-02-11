using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    Transform ropeArmatureRoot;
    Transform ropeStart;
    Transform ropeEnd;

    Rigidbody ropeStartBody;
    Rigidbody ropeEndBody;

    [SerializeField]
    Transform root;
    [SerializeField]
    Transform end;

    // Start is called before the first frame update
    void Start()
    {
        //Get Heirarchy and initialize all children in armature with character joint, collider and rigidbody
        ropeArmatureRoot = this.gameObject.transform.GetChild(0);

        RecursivelyAddRopeSections(null, ropeArmatureRoot, true);

        ropeStartBody = ropeStart.GetComponent<Rigidbody>();
        ropeEndBody = ropeEnd.GetComponent<Rigidbody>();
    }

    void RecursivelyAddRopeSections(Transform previousSection, Transform parentSection, bool ignoreJoint)
    {
        Transform childObj = default;
        if(parentSection.childCount > 0)
        {
            childObj = parentSection.GetChild(0);
        }

        //Else we have hit the end.

        if(ignoreJoint)
        {
            RecursivelyAddRopeSections(parentSection, childObj, false);
            childObj.GetComponent<Rigidbody>().useGravity = false;
            ropeStart = childObj;
            return;
        }
        
        if(previousSection && previousSection?.gameObject.GetComponent<Rigidbody>())
        {
            HingeJoint charJoint = parentSection.gameObject.AddComponent<HingeJoint>();
            Rigidbody rigidbody = parentSection.gameObject.GetComponent<Rigidbody>();
            rigidbody.useGravity = true;
            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            charJoint.connectedBody = previousSection.gameObject.GetComponent<Rigidbody>();
        }
        if(!parentSection.gameObject.GetComponent<Rigidbody>())
        {
            Debug.Log("Bruh");
            Rigidbody rigidbody = parentSection.gameObject.AddComponent<Rigidbody>();
            rigidbody.useGravity = true;
            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }
        // CapsuleCollider collider = parentSection.gameObject.AddComponent<CapsuleCollider>();
        // collider.height = 0.01f;
        // collider.radius = 0.01f;
        

        if(childObj)
        {
            RecursivelyAddRopeSections(parentSection, childObj, false);
            return;   
        }

        //Exit if child segment doesn't exist.
        //This also means we've hit the end.
        ropeEnd = parentSection;
        return;
    }

    // Update is called once per frame
    void Update()
    {
        ropeStart.position = root.position;
        ropeStart.rotation = root.rotation;
        ropeEnd.position = end.position;
        ropeEnd.rotation = end.rotation;
        ropeEndBody.velocity = new Vector3(0,0,0);
        ropeStartBody.velocity = new Vector3(0,0,0);
    }
}
