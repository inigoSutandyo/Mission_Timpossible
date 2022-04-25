using UnityEngine;
public class WallController : MonoBehaviour
{
    [SerializeField] private bool isDrop = false;

    [SerializeField] private GameObject Pivot1, Pivot2;

    private float speed = 10f;

    void Update()
    {
        if (isDrop)
        {
          
            var step = speed * Time.deltaTime;
            Quaternion target1 = Quaternion.Euler(0, -70, 0);
            Quaternion target2 = Quaternion.Euler(0, 70, 0);
            Pivot1.transform.localRotation = Quaternion.RotateTowards(Pivot1.transform.localRotation, target1, step);


            Pivot2.transform.localRotation = Quaternion.RotateTowards(Pivot2.transform.localRotation, target2, step);
        }
    }

    public void DropWall()
    {
        isDrop = true;
    }
}
