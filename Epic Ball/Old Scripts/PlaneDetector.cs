using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneDetector : MonoBehaviour
{

    [SerializeField] private bool isBottom = false;

    private MeshRenderer mesh;
    private MeshRenderer mesh2;
    private MeshCollider meshCollider;
    private GameObject player;
    private CapsuleCollider playerCap;
    private GameObject cameraTarget;
    private bool isAbove;

private void Awake() {
        meshCollider = GetComponent<MeshCollider>();
}

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        cameraTarget = player.transform.GetChild(0).gameObject;
        mesh = GetComponent<MeshRenderer>();
        mesh2 = GetComponentsInChildren<MeshRenderer>()[1];
        playerCap = player.GetComponent<CapsuleCollider>();
        CheckHeight(player);
        if (isAbove)
        {
            mesh.enabled = true;
        }
        else
        {
            mesh.enabled = false;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != cameraTarget)
        {
            CheckHeight(other.gameObject);
                if (other == playerCap)
            {
                    SetRender(isAbove);
                }
            if (isBottom && !isAbove)
            {
                if (other == playerCap)
                {
                    FindObjectOfType<PlayerPrefsController>().pauseLock = true;
                    other.gameObject.GetComponent<Ball> ().isAlive = false;
                    StartCoroutine (GameObject.FindWithTag("Goal").GetComponent<Goal>().HardRestartGame(3f));
                }
                else if (other.gameObject != cameraTarget && other.gameObject != player)
                {
                    Destroy(other.gameObject, 10f);
                }
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != cameraTarget)
        {
            CheckHeightEnter(other.gameObject);
            if (other == playerCap)
            {
                SetRender(isAbove);
            }

        }
    }


    public void CheckHeightEnter(GameObject fallingObject)
    {
        if (fallingObject.transform.position.y  +.5f < transform.position.y)
        {
            isAbove = false;
        }
        else
        {
            isAbove = true;
        }
    }

    public void CheckHeight(GameObject fallingObject)
    {
        if (fallingObject.transform.position.y < transform.position.y)
        {
            isAbove = false;
        }
        else
        {
            isAbove = true;
        }
    }

    public void SetRender(bool above)
    {
        if (!above)
        {
            mesh.enabled = false;
            mesh2.enabled = true;
        }
        else if (above)
        {
            mesh.enabled = true;
            mesh2.enabled = false;
        }
    }

    public bool IsAbove
    {
        get { return isAbove; }
        set { isAbove = value; }
    }

}
