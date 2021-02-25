using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    /* 1. 난수를 이용하여 카메라의 위치를 주기마다 임의로 변경하여 카메라가 흔들리는 듯한 효과를 발생
     * 2. Random.value
     * 3. Invoke(M, f): M(Method)를 f초 뒤에 실행
     * 4. InvokeReapeating(M, f, t): M을 f초 뒤에 주기 t마다 반복하여 실행
     * 5. BeginShake와 StopShake로 Shake 기능을 나누어 메소드로 구현
     * 6. Camera를 EmptyObject의 child로 설정하고 CameraFollow.cs를 Emptyobject에 부착하여 EmptyObject가 위치를 변경하고
     * 이에 상대적(Relative)으로 Camera가 움직이므로 Camera의 원위치를 (0,0,0)로 표현하는 것이 가능
     * 7. InvokeReapeating이 필요한 이유: BeginShake로 카메라의 위치를 난수로 조정할때 한번 조정하면 흔들리는 효과를 얻지못하고
     * 그냥 카메라가 임의 방향으로 움직이는 것으로 보일 것이다. 따라서 시간 t동안 반복적으로 실행해야 흔들리는 효과를 얻을 수 있음
     * 8. 자신이 자식 오브젝트이고 이 오브젝트의 위치를 변경할때는 transform.position이 아닌, transform.localposition 사용
     * 9. 부모 P 오브젝트, 자식 C 오브젝트가 존재할때, Inspector에서 P의 위치가 (1,2,3)이고 
     * 자식 C가 (0,0,0)일때 자식 C의 Position은 P의 위치인 (1,2,3)을 나타내며, 자식 C의 localPosition은
     * (0,0,0)을 나타낸다. 단, C의 Position을 직접 수정하면 더이상 P의 Position과 동일하지않다.
     * 결론적으로 position은 world 좌표상의 절대 좌표를 의미하고, localposition은 부모기준으로 자식 좌표를
     * 상대적으로 나타낸 좌표를 의미한다.
     */

    //변수
    public Camera mainCam;
    //흔들리는 정도, 0이면 disable
    public float shakeAmt = 0.01f;
    //흔들리는 시간
    public float length = 2f;


    // Start is called before the first frame update
    void Awake()
    {
        if(mainCam == null)
        {
            Debug.LogError("mainCam not selected");
            mainCam = Camera.main;
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Shake(0.1f, 0.2f);
            
        }


    }

    void Shake(float Amt, float length)
    {
        shakeAmt = Amt;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", length);


    }

    void BeginShake()
    {
        if(shakeAmt > 0)
        {
            Vector3 campos = mainCam.transform.position;

            // 둘은 같은 명령 같지만 매번 Random.value 호출마다 값이 다르므로 코드는 같지만 결과값은 다른 명령줄
            float rdmCamposX = Random.value * shakeAmt * 2f - shakeAmt;
            float rdmCamposY = Random.value * shakeAmt * 2f - shakeAmt;

            campos.x += rdmCamposX;
            campos.y += rdmCamposY;

            //MainCamera의 position은 부모 오브젝트인 "Camera"의 위치가 항상 아님에 주의
            //Default 상태로는 같지만, Child의 Position을 직접 수정할 수 있음.
            //Parent의 이동에 따라 Child가 같이 이동할 뿐임에 유의
            mainCam.transform.position = campos;
        }
        

    }

    void StopShake()
    { 
        //원래 자리로 돌아감, 이때 자식인 MainCamera의 localPosition이 (0,0,0)일때 부모 Position과 같아지므로
        //target을 주시하고있는 Camera(부모)의 Position으로 돌아감을 의미한다.
        mainCam.transform.localPosition = Vector3.zero;

        //주기적으로 호출되는 BeginShake를 취소 
        CancelInvoke("BeginShake");

    }
}
