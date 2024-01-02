using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderMovement : MonoBehaviour
{

    [SerializeField] private float desiredSpeed;
    [SerializeField] private float maximumDrag;
    [SerializeField] private float forceConstant;

    Rigidbody rigidbody;
    BumpSounds bumpsounds;

    private float jumpSpeed;
    private float playerDistance = 50f;
    public bool activate;
    public bool isHorizontal;

private void Awake()
{
        bumpsounds = GetComponent<BumpSounds>();
        rigidbody = GetComponent<Rigidbody>();
        Vector3 right = transform.up;
        Vector3 up = Vector3.up;
        Vector3 forward = Vector3.Cross(right, up);
}
    

    public void Roll(Vector3 direction)
    {
        rigidbody.drag = Mathf.Lerp(maximumDrag, 0, direction.magnitude);
        float forceMultiplier = Mathf.Clamp01((desiredSpeed - rigidbody.velocity.magnitude) / desiredSpeed);
        rigidbody.AddForce(direction * (forceMultiplier * Time.deltaTime * forceConstant));
    }

    public IEnumerator LocateSide(Ball ball)
    {
        while ((activate == true || bumpsounds .isGiant) && ball.isAlive && !ball.win)
        {
            Vector3 right = transform.up;
            Vector3 up = Vector3.up;
            Vector3 forward = Vector3.Cross(right, up);
            Vector3 toTarget = (ball.gameObject.transform.position - transform.position).normalized;

            if ((Physics.Raycast(transform.position, -Vector3.up, transform.localScale.z)))
            {
                InvokeRepeating ("CheckHorizontal", 0f, 1f);
                if(isHorizontal)
                {
                    if (Vector3.Dot(toTarget, forward) > 0)
                    {
                        Roll(forward);
                    }
                    else
                    {
                        Roll(-forward);
                    }  
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void CheckHorizontal()
    {
        float actualAngleZ = ActualAngle.Repeat(transform.rotation.eulerAngles.z, 360);
        float actualAngleX = ActualAngle.Repeat(transform.rotation.eulerAngles.x, 360);
        if (((actualAngleZ < 93 && actualAngleZ > 87) || (actualAngleX < 93 && actualAngleX > 87)) || ((actualAngleZ < 273 && actualAngleZ > 267) || (actualAngleX < 273 && actualAngleX > 267)))
        {
            isHorizontal = true;
        }
        else
        {
            isHorizontal = false;
        }
    }

}
