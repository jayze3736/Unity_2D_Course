using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
// SpriteRenderer가 없는 게임 오브젝트가 존재하지 않도록 자동으로 컴포넌트를 추가하여 오류를 사전에 방지

public class Tiling : MonoBehaviour
{
    public int offsetX = 2; //the offset so that we don't get any weird errors
    
    // these are used for checking if we need to instantiate stuff
    //오른쪽에 stuff(Buddy)가 생성이 되어있으면 hasARightBuddy는 True "has a right buddy = 이미 오른쪽에 buddy를 갖고 있다."
    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;

    public bool reverseScale = false; //used if the object is not tilable

    private float spriteWidth = 0f; //the width of our element
    private Camera cam;
    private Transform myTransform;

    private void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        //spriteWidth는 sprite의 가로 길이를 의미
        spriteWidth = sRenderer.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        //does it still need buddies?
        if(hasALeftBuddy == false || hasARightBuddy == false)
        {
            //calculate the camera extend(half the width) of what the camera can see in world coordinate
            //cam.orthographicSize는 camera의 수직방향의 반 길이를 의미한다.
            //Screen.width, height는 Player의 screen의 폭, 높이를 의미
            //현재는 카메라가 한개이므로 cam.ortoraphicSize/Screen.height = 1/2이다.
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;
            //카메라 가로길이 반길이

            // 카메라 viewzone의 맨 오른쪽 끝의 x좌표
            //spriteWidth/2는 sprite의 가로의 절반 크기, camerHorizontalExtend는 카메라 viewzone 가로 길이의 절반 
            float edgeVisiblePositionRight = myTransform.position.x + (spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = myTransform.position.x - (spriteWidth / 2) + camHorizontalExtend;

            /*edgeVisiblePosition은 카메라의 viewzone에서 sprite가 벗어나지않도록하는 변위값을 의미한다.
            즉, cam.transform.postion.x 이 edgeVisiblePositionRight일때 viewzone의 오른쪽 끝은 sprite의 오른쪽 끝을 보여줄 것이다. 
            거기서 offsetX를 두어 viewzone에 sprite가 걸쳐지기 전에 객체를 생성한다.
             */
            
            if((cam.transform.position.x >= edgeVisiblePositionRight - offsetX) && hasARightBuddy == false)
            {
                MakeNewBuddy(1); //인자가 1이면 오른쪽으로 객체 생성
                hasARightBuddy = true; //원본의 오른쪽에 객체가 생성되었으니 true로 변경
            }
            else if(cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false)
            {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
    }

    
    void MakeNewBuddy(int rightOrleft)
    {
        // rightOrleft = -1(left) or 1(right)
        //caculating the new position for our new buddy
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrleft, myTransform.position.y, myTransform.position.z);
        
        //new body stored in variable
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation);
        //Instatntiate - spon the clone - parameter: object(생성할 객체), position(생성할 위치), rotation(Quaternion)


        if (reverseScale == true)
        {
            //scale 컴포넌트를 수정, scale 값이 -가 되면 flip되는 효과가 연출됨
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
            //if not tilable let's reverse the x position of sprite to get rid of awkward seams
        }
        newBuddy.parent = myTransform.parent;
        //만약 원본의 parent가 존재하면 newBuddy의 parent로 대입

        if(rightOrleft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
            //복사본 Buddy 기준으로 왼쪽에 객체가 생성되었으므로 true
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}


