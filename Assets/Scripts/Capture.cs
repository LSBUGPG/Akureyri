using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Capture : MonoBehaviour
{
    public Camera captureCamera;
    public Camera cam2;
    public RenderTexture photo1;
    public RenderTexture photo2;
    public RenderTexture photo3;
    public RenderTexture photo4;
    public RenderTexture currentPhoto;

    public Animator anim;

    public GameObject CameraFlash;
    public Transform bip_Camera;

    private bool takingPhoto = false;

    void Start()
    {
        photo1.Release();
        photo2.Release();
        photo3.Release();
        photo4.Release();
        currentPhoto.Release();

    }

    void Update()
    {
        if (takingPhoto == false)
        {
            cam2.targetTexture = currentPhoto;
        }
        if (Input.GetKey(KeyCode.E) && !PauseMenu.IsPaused && takingPhoto == false)
        {
            Instantiate(CameraFlash, bip_Camera.position, bip_Camera.rotation, bip_Camera);

            RaycastHit hit;
            anim.Play("Camera_anim");
            if (Physics.SphereCast(captureCamera.transform.position, 5, captureCamera.transform.forward, out hit))
            {
                if (hit.transform.name == "HOF")
                {
                    StartCoroutine(CaptureImage1());
                }
                if (hit.transform.name == "Dolphin")
                {
                    StartCoroutine(CaptureImage2());
                }
                if (hit.transform.name == "Church")
                {
                    StartCoroutine(CaptureImage3());
                }
                if (hit.transform.name == "Airport")
                {
                    StartCoroutine(CaptureImage4());
                }


            }
            StartCoroutine(CaptureImage());
            StartCoroutine(photo());

        }
    }
    IEnumerator CaptureImage()
    {
        cam2.targetTexture = currentPhoto;
        yield return null;
        cam2.targetTexture = null;
    }
    IEnumerator CaptureImage1()
    {
        captureCamera.targetTexture = photo1;
        yield return null;
        captureCamera.targetTexture = null;
    }
    IEnumerator CaptureImage2()
    {
        captureCamera.targetTexture = photo2;
        yield return null;
        captureCamera.targetTexture = null;
    }
    IEnumerator CaptureImage3()
    {
        captureCamera.targetTexture = photo3;
        yield return null;
        captureCamera.targetTexture = null;

    }
    IEnumerator CaptureImage4()
    {
        captureCamera.targetTexture = photo4;
        yield return null;
        captureCamera.targetTexture = null;
    }

    IEnumerator photo()
    {
        takingPhoto = true;
        yield return new WaitForSeconds(4);
        takingPhoto = false;
    }
}
