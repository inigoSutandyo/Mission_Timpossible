using UnityEngine;
public class WallController : MonoBehaviour
{
    private bool isDrop = false;
    private Animator anim;
    private float posY;
    private Transform objTransform;
    private void Awake()
    {
        objTransform = GetComponent<Transform>();
    }


    void Update()
    {
        if (isDrop && objTransform.position.y >= -1)
        {
            objTransform.position = new Vector3(objTransform.position.x, objTransform.position.y - 1.0f * Time.deltaTime, objTransform.position.z);
        }
    }

    public void DropWall()
    {
        isDrop = true;
    }
}
