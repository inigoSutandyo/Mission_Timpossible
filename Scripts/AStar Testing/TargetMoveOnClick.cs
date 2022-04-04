using UnityEngine;

public class TargetMoveOnClick : MonoBehaviour
{
    public LayerMask hitlayers;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, hitlayers))
            {
                this.transform.position = hit.point;
            }
        }
    }
}
