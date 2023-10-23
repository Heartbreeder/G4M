using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
public class DepthOfFieldController : MonoBehaviour
{
    public Volume vol;
    public float focusSpeed = 5f;
    public float maxFocusDistance = 100f;
    DepthOfField dof;
    Ray raycast;
    RaycastHit hit;
    bool isHit;
    float hitDistance;
    // Start is called before the first frame update
    void Start()
    {
        
        vol.sharedProfile.TryGet(out dof);
    }

    // Update is called once per frame
    void Update()
    {
        raycast = new Ray(transform.position, transform.forward * maxFocusDistance);
        isHit = false;
        if (Physics.Raycast(raycast, out hit, 100f))
        {
            isHit = true;
            hitDistance = Vector3.Distance(transform.position, hit.point);
        }
        else {
            if (hitDistance < maxFocusDistance) {
                hitDistance++;
            }
        }
        SetFocus();
    }

    private void OnDrawGizmos()
    {
        if (isHit) {
            Gizmos.DrawSphere(hit.point, 0.1f);
            Debug.DrawRay(transform.position, transform.forward * Vector3.Distance(transform.position, hit.point));
        }
        else {
            Debug.DrawRay(transform.position, transform.forward * 100f);
        }
    }

    private void SetFocus() {
        dof.focusDistance.value = Mathf.Lerp(dof.focusDistance.value, hitDistance, Time.deltaTime * focusSpeed);
    }
}
