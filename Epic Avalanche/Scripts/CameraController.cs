using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    
    // to turn toward target
    public float turnSpeed = 0.01f;
    Quaternion rotationGoal;
    Vector3 direction;
    public Transform[] cameraTargets;
    private Transform target;
    private int currentTargetNum = 0;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject[] attachedCameras;

    private void Start()
    {
        if(cameraTargets.Length > 0)
        {
        target = cameraTargets[currentTargetNum];
        }
    }
    

    // to turn toward target

    private void Update()
    {
        if (target != null)
        {
            TurnCamera();
        }
    }

    private void TurnCamera()
    {
        direction = (target.position - mainCamera.transform.position).normalized;
        rotationGoal = Quaternion.LookRotation(direction);
     //   thisCamera.transform.rotation = Quaternion.Slerp(thisCamera.transform.rotation, rotationGoal, turnSpeed);
        for(int i = 0; i < attachedCameras.Length; i++)
        {
            attachedCameras[i].transform.rotation = Quaternion.Slerp(attachedCameras[i].transform.rotation, rotationGoal, turnSpeed);
        }
    }

    public void UpdateTarget ()
    {
        if(cameraTargets.Length <= currentTargetNum)
        {
            currentTargetNum = 0;
        }
        else
        {
            currentTargetNum++;
        }
        target = cameraTargets[currentTargetNum];
    }




    // to look at target with offest

    //private void LateUpdate()
    //{
    //    Vector3 desiredPosition = target.position + offSet;
    //    Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    //    transform.position = smoothedPosition;

    //    transform.LookAt(target);
    //}



    // to move camera to different position

    //IEnumerator switchCamera()
    //{
    //    var animSpeed = 0.5f;

    //    Vector3 pos = main_Camera.transform.position;
    //    Quaternion rot = main_Camera.transform.rotation;

    //    float progress = 0.0f;  //This value is used for LERP

    //    while (progress < 1.0f)
    //    {
    //        main_Camera.transform.position = Vector3.Lerp(pos, second_Camera.transform.position, progress);
    //        main_Camera.transform.rotation = Quaternion.Lerp(rot, second_Camera.transform.rotation, progress);
    //        yield return new WaitForEndOfFrame();
    //        progress += Time.deltaTime * animSpeed;
    //    }

    //    //Set final transform
    //    main_Camera.transform.position = second_Camera.transform.position;
    //    main_Camera.transform.rotation = second_Camera.transform.rotation;
    //}
}
