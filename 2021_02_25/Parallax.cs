using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Parallax : MonoBehaviour
{
    public bool scrolling, parallax;
    public float backgroundSize; //배경 sprite의 사이즈
    public float paralaxSpeed;

    private Transform cameraTransform; // 카메라의 위치 정보 변수
    private Transform[] layers; // background를 관리하는 layer
    private float viewZone = 10; // scroll을 위한 특정 변수값 viewzone
    private int leftIndex; //layer에서 관리하는 background sprite 들중의 가장 왼쪽 sprite를 나타내는 index 
    private int rightIndex; // 가장 오른쪽 배경 sprite index
    private float LastCameraX; //위치 좌표값을 구성

    // Start is called before the first frame update
    void Start()
    {

        cameraTransform = Camera.main.transform; //카메라의 위치 정보 저장
        LastCameraX = cameraTransform.position.x;

        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            layers[i] = transform.GetChild(i);

        leftIndex = 0;
        rightIndex = layers.Length - 1;

    }
    
    private void ScrollLeft()
    {
        
        layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize); //맨 오른쪽에 있는 background를
        // 가장 왼쪽에 있는 background 기준으로 background 크기만큼 이동하도록 지시
        leftIndex = rightIndex; // 가장 오른쪽에 있는 background가 가장 왼쪽으로 이동하기때문에
        rightIndex--;//0,1,2 -> 2,0,1 -> 1,2,0 -> 0,1,2 가운데있는 background는 상관없음 
        if (rightIndex < 0)
            rightIndex = layers.Length - 1;


    }

    private void ScrollRight()
    {
        if (leftIndex == layers.Length)
            leftIndex = 0;
        layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgroundSize); //맨 오른쪽에 있는 background를
        // 가장 왼쪽에 있는 background 기준으로 background 크기만큼 이동하도록 지시
        rightIndex = leftIndex; // 가장 오른쪽에 있는 background가 가장 왼쪽으로 이동하기때문에
        leftIndex++;//0,1,2 -> 1,2,0 -> 2,0,1 -> 0,1,2
        

    }


        // Update is called once per frame
        void Update()
    {
        
        if(parallax)
        { 
            float deltaX = cameraTransform.position.x - LastCameraX;
            transform.position += Vector3.right * (deltaX * paralaxSpeed);
            LastCameraX = cameraTransform.position.x;
        }
        
        if (scrolling)
        {
            if (cameraTransform.position.x < layers[leftIndex].position.x + viewZone)
                ScrollLeft();

            if (cameraTransform.position.x > layers[rightIndex].position.x - viewZone)
                ScrollRight();

        }
    }
}
