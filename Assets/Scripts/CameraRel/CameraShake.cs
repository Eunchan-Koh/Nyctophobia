using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool flipHorizontal;
    public bool flipVertical;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            Shake();
        }  
        if(Input.GetKeyDown(KeyCode.R)){
            Flip();
        }
    }

    void Shake(){
        StopCoroutine(ShakeCamera());
        transform.localPosition = Vector3.zero;
        StartCoroutine(ShakeCamera());
    }
    void Flip(){
        StartCoroutine(FlipCamera());
    }

    IEnumerator FlipCamera(){
        // Vector3 scale = new Vector3(flipHorizontal ? -1 : 1, 1, 1);
        // Vector3 scale = new Vector3(1, flipVertical ? -1 : 1, 1);
        // Camera.main.projectionMatrix = Camera.main.projectionMatrix * Matrix4x4.Scale(scale);

        Vector3 scale = new Vector3(1, -1, 1);
        Camera.main.projectionMatrix = Camera.main.projectionMatrix * Matrix4x4.Scale(scale);
        yield return new WaitForSeconds(0.05f);
        Camera.main.projectionMatrix = Camera.main.projectionMatrix * Matrix4x4.Scale(scale);
    }

    IEnumerator ShakeCamera(){
        Vector3 ran;
        for(int i = 0; i < 6; i++){
            ran = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),0);
            transform.localPosition = ran;
            yield return new WaitForSeconds(0.05f);
            // transform.position = originalPos - ran;
            // yield return new WaitForSeconds(0.05f);
        }
        transform.localPosition = Vector3.zero;
        
    }
}
