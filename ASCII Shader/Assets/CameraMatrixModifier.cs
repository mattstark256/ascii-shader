using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraMatrixModifier : MonoBehaviour
{
    [SerializeField, Range(0, 2)]
    private float distortAmount = 1.2f;
    [SerializeField, Range(-180, 180)]
    private float rotateSpeed = 30;

    private float distortAngle = 0;


    void Update()
    {
        distortAngle += Time.deltaTime * rotateSpeed;
        
        Matrix4x4 s = Matrix4x4.identity; // Stretch
        Matrix4x4 r = Matrix4x4.identity; // Translate and rotate

        s[0, 0] *= distortAmount;
        s[1, 1] /= distortAmount;

        // Translate it so the centre of rotation is the bottom of the screen
        r[1, 3] = -1f;
        
        r[0, 0] = Mathf.Cos(Mathf.Deg2Rad * distortAngle);
        r[1, 0] = Mathf.Sin(Mathf.Deg2Rad * distortAngle);
        r[0, 1] = -Mathf.Sin(Mathf.Deg2Rad * distortAngle);
        r[1, 1] = Mathf.Cos(Mathf.Deg2Rad * distortAngle);

        Camera cam = GetComponent<Camera>();
        cam.ResetProjectionMatrix();
        cam.projectionMatrix = r * s * r.inverse * cam.projectionMatrix;
    }

    private void OnDisable()
    {
        Camera cam = GetComponent<Camera>();
        cam.ResetProjectionMatrix();
    }
}
