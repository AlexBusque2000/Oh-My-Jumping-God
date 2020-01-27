using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GanchoController : MonoBehaviour
{
    public float ganchoRange;
    public float ganchoAreaRange;
    public float approachSpeed;
    public float cameraMovementSpeed;
    public Vector3 cameraOffset;

    GameObject currentGancho;

    Vector3 endPosition;
    Vector3 startPosition;

    [SerializeField]
    bool isGanchoing;

    float lerpTime;
    float cameraLerpTime;

    int layerMask;

    FirstPersonController firstPersonController;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = ~LayerMask.GetMask("Ethereal");
        firstPersonController = GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectGancho();

        if (isGanchoing)
        {
            firstPersonController.canRotate = false;
            MoveToGancho();
            MoveCamera();
        }
    }

    void DetectGancho()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, ganchoAreaRange, firstPersonController.PlayerCamera.transform.forward, out hit, ganchoRange, layerMask))
        {
            if (hit.collider.gameObject.GetComponent<EngancheController>())
            {
                //Si Detecto otro gancho distinto pero ya tenía uno guardado, desactiva el que tenía guardado
                if (currentGancho != hit.collider.gameObject && currentGancho != null)
                    currentGancho.GetComponent<EngancheController>().isActive = false;


                currentGancho = hit.collider.gameObject;
                hit.collider.gameObject.GetComponent<EngancheController>().isActive = true;
                ActivateGancho();
            }
        }
        else
        {
            if (currentGancho != null)
            currentGancho.GetComponent<EngancheController>().isActive = false;
        }

    }

    //Esta función solo se activa una vez
    void ActivateGancho()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            endPosition = currentGancho.transform.GetChild(0).transform.position;
            startPosition = transform.position;
            transform.LookAt(new Vector3(endPosition.x, transform.position.y, endPosition.z));
            firstPersonController.PlayerCamera.transform.localEulerAngles = new Vector3(30, firstPersonController.PlayerCamera.transform.localEulerAngles.y, firstPersonController.PlayerCamera.transform.localEulerAngles.z);
            lerpTime = 0;
            cameraLerpTime = 0;
            isGanchoing = true;
        }
    }

    void MoveToGancho()
    {
        transform.position = Vector3.Lerp(startPosition, endPosition, lerpTime);

        if (lerpTime < 1)
        {
            lerpTime += approachSpeed * Time.deltaTime;
        }
        else
        {
            firstPersonController.PlayerCamera.transform.localPosition = Vector3.zero;
            firstPersonController.PlayerCamera.transform.localEulerAngles = Vector3.zero;
            firstPersonController.canRotate = true;
            isGanchoing = false;
        }
    }

    void MoveCamera()
    {
        firstPersonController.PlayerCamera.transform.localPosition = Vector3.Lerp(Vector3.zero, cameraOffset, cameraLerpTime);

        if (lerpTime < 0.5f)
        {
            cameraLerpTime += cameraMovementSpeed * Time.deltaTime;
        }
        else
        {
            cameraLerpTime -= cameraMovementSpeed * Time.deltaTime;
        }
    }

    void ClampLerpTimes()
    {
        lerpTime = Mathf.Clamp(lerpTime, 0, 1);
        cameraLerpTime = Mathf.Clamp(cameraLerpTime, 0, 1);
    }
}
