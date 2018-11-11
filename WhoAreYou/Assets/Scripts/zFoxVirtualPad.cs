using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum zFOXVPAD_BUTTON
{
    NON,
    DOWN,
    HOLE,
    UP
}

public enum zFOXVPAD_SLIDPADVALUEMODE
{
    PAD_XY_SCREEN_WH,
    PAD_XY_SCREEN_WW,
    PAD_XY_SCREEN_HH,
}

public class zFoxVirtualPad : MonoBehaviour {

    // 외부 파라미터
    public GameObject inventory;
    public float padSensitive = 25.0f;

    public zFOXVPAD_SLIDPADVALUEMODE padValMode = zFOXVPAD_SLIDPADVALUEMODE.PAD_XY_SCREEN_WW;
    public float horizontalStartVal = 0.05f;
    public float verticalStartVal = 0.05f;

    public bool autoLayout = false;
    public bool autoLayoutUpdate = false;
    public Vector2 autoLayoutPOS_SlidePad = new Vector2(0.7f, 0.5f);
    public Vector2 autoLayoutPOS_ButtonA = new Vector2(0.5f, 0.5f);
    public Vector2 autoLayoutPOS_ButtonB = new Vector2(0.8f, 0.5f);
    public Vector2 autoLayoutPOS_invenIcon = new Vector2(1.0f, 0.0f);
    public Vector2 autoLayoutPOS_dialogue = new Vector2(1.0f, 0.5f);

    [Header("------Debug-------")]
    public float horizontal = 0.0f;
    public float vertical = 0.0f;

    public zFOXVPAD_BUTTON buttonA = zFOXVPAD_BUTTON.NON;
    public zFOXVPAD_BUTTON buttonB = zFOXVPAD_BUTTON.NON;
    public zFOXVPAD_BUTTON invenIcon = zFOXVPAD_BUTTON.NON;
    public zFOXVPAD_BUTTON dialogue = zFOXVPAD_BUTTON.NON;

    //내부
    Camera uicam;
    SpriteRenderer sprSlidePad;
    SpriteRenderer sprSlidePadBack;
    SpriteRenderer sprButtonA;
    SpriteRenderer sprButtonB;
    SpriteRenderer sprinvenIcon;
    SpriteRenderer sprdialogue;

    Animator animator;

    int buttonAindex = -1;
    int buttonBindex = -1;
    int invenIconindex = -1;
    int dialogueindex = -1;
    bool buttonAHit = false;
    bool buttonBHit = false;
    bool invenIconHit = false;
    bool dialogueHit = false;

    public bool movPadEnable = false;
    Vector2 movSt = Vector2.zero;
    public Vector2 mov = Vector2.zero;
    bool movEnable = false;

    // Use this for initialization
    void Awake () {
        uicam = GameObject.Find("FUIPadCamera").GetComponent<Camera>() as Camera;
        sprSlidePad = GameObject.Find("SlidePad").GetComponent<SpriteRenderer>() as SpriteRenderer;
        sprSlidePadBack = GameObject.Find("SlidePadBack").GetComponent<SpriteRenderer>() as SpriteRenderer;
        sprButtonA = GameObject.Find("Button_A").GetComponent<SpriteRenderer>() as SpriteRenderer;
        sprButtonB = GameObject.Find("Button_B").GetComponent<SpriteRenderer>() as SpriteRenderer;
        sprinvenIcon = GameObject.Find("inven_Icon").GetComponent<SpriteRenderer>() as SpriteRenderer;
        sprdialogue = GameObject.Find("dialoguePanal").GetComponent<SpriteRenderer>() as SpriteRenderer;
        animator = GetComponent<Animator>();

        RunAutoLayout();
    }

    void RunAutoLayout()
    {
        if(autoLayout)
        {
            Vector3 scPos = uicam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f)) - uicam.transform.position;
            Vector3 posPad = new Vector3(-scPos.x * autoLayoutPOS_SlidePad.x, -scPos.y * autoLayoutPOS_SlidePad.y, 0.0f);
            sprSlidePadBack.transform.localPosition = posPad;
            Vector3 posBtnA = new Vector3(scPos.x * autoLayoutPOS_ButtonA.x, -scPos.y * autoLayoutPOS_ButtonA.y, 0.0f);
            sprButtonA.transform.localPosition = posBtnA;
            Vector3 posBtnB = new Vector3(scPos.x * autoLayoutPOS_ButtonB.x, -scPos.y * autoLayoutPOS_ButtonB.y, 0.0f);
            sprButtonB.transform.localPosition = posBtnB;
            Vector3 posBtnInven = new Vector3(scPos.x * autoLayoutPOS_invenIcon.x, -scPos.y * autoLayoutPOS_invenIcon.y, 0.0f);
            sprinvenIcon.transform.localPosition = posBtnInven;
            Vector3 posBtnDialogue = new Vector3(scPos.x * autoLayoutPOS_dialogue.x, -scPos.y * autoLayoutPOS_dialogue.y, 0.0f);
            sprdialogue.transform.localPosition = posBtnDialogue;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (autoLayoutUpdate)
            RunAutoLayout();


        // -- Button -------------
        if (buttonA == zFOXVPAD_BUTTON.UP)
        {
            buttonA = zFOXVPAD_BUTTON.NON;
            buttonAindex = -1;
        }
        if (buttonB == zFOXVPAD_BUTTON.UP)
        {
            buttonB = zFOXVPAD_BUTTON.NON;
            buttonBindex = -1;
        }
        if (invenIcon == zFOXVPAD_BUTTON.UP)
        {
            invenIcon = zFOXVPAD_BUTTON.NON;
            invenIconindex = -1;
        }
        if (dialogue == zFOXVPAD_BUTTON.UP)
        {
            dialogue = zFOXVPAD_BUTTON.NON;
            dialogueindex = -1;
        }

        buttonAHit = false;
        buttonBHit = false;
        invenIconHit = false;
        dialogueHit = false;

        if (Input.touchCount > 0)
        {
            bool objectTouched = false;
            for(int i = 0; i < Input.touchCount; i++)
            {
                Ray ray = uicam.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;

                // GUI 레이어마스크 처리
                if(Physics.Raycast (ray, out hit, Mathf.Infinity))
                {
                    TouchPhase tp = Input.GetTouch(i).phase;
                    if(tp == TouchPhase.Began)
                    {
                        CheckButtonDown(hit, i);
                        objectTouched = true;
                    }
                    else if(tp == TouchPhase.Moved || tp == TouchPhase.Stationary)
                    {
                        CheckButtonMove(hit, i);
                        objectTouched = true;
                    }
                    else if(tp == TouchPhase.Ended || tp == TouchPhase.Canceled)
                    {
                        CheckButtonUp(hit, i);
                        objectTouched = true;
                    }
                }
            }

            if (!objectTouched)

                CheckButtonNon();
        }
        else
        {
            Ray ray = uicam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // GUI 레이어마스크 검사
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (Input.GetMouseButtonDown(0))
                    CheckButtonDown(hit, 0);
                else if (Input.GetMouseButton(0))
                    CheckButtonMove(hit, 0);
                else if (Input.GetMouseButtonUp(0))
                    CheckButtonUp(hit, 0);
                else
                    CheckButtonNon();
            }
            else
                CheckButtonNon();
        }

        movEnable = false;
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (i != buttonAindex && i != buttonBindex)
                {
                    TouchPhase tp = Input.GetTouch(i).phase;
                    if (tp == TouchPhase.Began)
                    {
                        if (CheckSlidePadDown(Input.GetTouch(i).position))
                            break;
                    }
                    else if (tp == TouchPhase.Moved || tp == TouchPhase.Stationary)
                    {
                        if (CheckSlidePadMove(Input.GetTouch(i).position))
                            break;
                    }
                    else if (tp == TouchPhase.Ended || tp == TouchPhase.Canceled)
                    {
                        CheckSlidePadUp();
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Input.mousePosition.x / Screen.width < 0.5f)
                    CheckSlidePadDown((Vector2)Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                CheckSlidePadMove((Vector2)Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                CheckSlidePadUp();
            }
        }

        if(movEnable == false)
        {
            movPadEnable = false;
            mov = Vector2.zero;
        }

        switch(padValMode)
        {
            case zFOXVPAD_SLIDPADVALUEMODE.PAD_XY_SCREEN_WH:
                horizontal = mov.x * padSensitive / Screen.width;
                vertical = mov.y * padSensitive / Screen.height;
                break;

            case zFOXVPAD_SLIDPADVALUEMODE.PAD_XY_SCREEN_WW:
                horizontal = mov.x * padSensitive / Screen.width;
                vertical = mov.y * padSensitive / Screen.width;
                break;

            case zFOXVPAD_SLIDPADVALUEMODE.PAD_XY_SCREEN_HH:
                horizontal = mov.x * padSensitive / Screen.height;
                vertical = mov.y * padSensitive / Screen.height;
                break;
        }


        if (horizontal < -1.0f) horizontal = -1.0f;
        if (horizontal > 1.0f) horizontal = 1.0f;
        if (vertical < -1.0f) vertical = -1.0f;
        if (vertical > 1.0f) vertical = 1.0f;

        if (Mathf.Abs(horizontal) < horizontalStartVal) horizontal = 0.0f;
        if (Mathf.Abs(vertical) < verticalStartVal) vertical = 0.0f;

        // pos로 Slidepad의 움직이는 범위를 조정할 수 있다.

        Vector3 pos = new Vector3(horizontal / (padSensitive / 10.0f), vertical / (padSensitive / 10.0f), 0.0f);
        sprSlidePad.transform.localPosition = pos;
    }

    void CheckButtonDown(RaycastHit hit, int i)
    {
        if (hit.collider.gameObject == sprButtonA.gameObject)
        {
            buttonA = zFOXVPAD_BUTTON.DOWN;
            buttonAindex = i;
            buttonAHit = true;
            sprButtonA.color = new Color(1.0f, 1.0f, 1.0f);
        }
        else if (hit.collider.gameObject == sprButtonB.gameObject)
        {
            buttonB = zFOXVPAD_BUTTON.DOWN;
            buttonBindex = i;
            buttonBHit = true;
            sprButtonB.color = new Color(1.0f, 1.0f, 1.0f);
        }
        else if (hit.collider.gameObject == sprinvenIcon.gameObject)
        {
            invenIcon = zFOXVPAD_BUTTON.DOWN;
            invenIconindex = i;
            invenIconHit = true;
            sprinvenIcon.color = new Color(1.0f, 1.0f, 1.0f);
            inventory.SetActive(true);
            Time.timeScale = 0;
        }
        else if (hit.collider.gameObject == sprdialogue.gameObject)
        {
            dialogue = zFOXVPAD_BUTTON.DOWN;
            dialogueindex = i;
            dialogueHit = true;
            sprdialogue.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }
    }

    void CheckButtonMove(RaycastHit hit, int i)
    {
        if (hit.collider.gameObject == sprButtonA.gameObject)
        {
            buttonA = zFOXVPAD_BUTTON.HOLE;
            buttonAindex = i;
            buttonAHit = true;
        }
        else if (hit.collider.gameObject == sprButtonB.gameObject)
        {
            buttonB = zFOXVPAD_BUTTON.HOLE;
            buttonBindex = i;
            buttonBHit = true;
        }
        else if (hit.collider.gameObject == sprinvenIcon.gameObject)
        {
            invenIcon = zFOXVPAD_BUTTON.HOLE;
            invenIconindex = i;
            invenIconHit = true;
        }
        else if (hit.collider.gameObject == sprdialogue.gameObject)
        {
            dialogue = zFOXVPAD_BUTTON.HOLE;
            dialogueindex = i;
            dialogueHit = true;
        }
    }

    void CheckButtonUp(RaycastHit hit, int i)
    {
        if (hit.collider.gameObject == sprButtonA.gameObject)
        {
            buttonA = zFOXVPAD_BUTTON.UP;
            buttonAindex = i;
            sprButtonA.color = new Color(0.3f, 0.3f, 0.3f);
        }
        else if (hit.collider.gameObject == sprButtonB.gameObject)
        {
            buttonB = zFOXVPAD_BUTTON.UP;
            buttonBindex = i;
            sprButtonB.color = new Color(0.3f, 0.3f, 0.3f);
        }
        else if (hit.collider.gameObject == sprinvenIcon.gameObject)
        {
            invenIcon = zFOXVPAD_BUTTON.UP;
            invenIconindex = i;
            sprinvenIcon.color = new Color(0.3f, 0.3f, 0.3f);
            inventory.SetActive(false);

            Time.timeScale = 1;
        }
        else if (hit.collider.gameObject == sprdialogue.gameObject)
        {
            dialogue = zFOXVPAD_BUTTON.UP;
            dialogueindex = i;
            sprdialogue.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
    }

    void CheckButtonNon()
    {
        if (!buttonAHit)
        {
            buttonA = zFOXVPAD_BUTTON.NON;
            buttonAindex = -1;
            sprButtonA.color = new Color(0.3f, 0.3f, 0.3f);
        }

        if (!buttonBHit)
        {
            buttonB = zFOXVPAD_BUTTON.NON;
            buttonBindex = -1;
            sprButtonB.color = new Color(0.3f, 0.3f, 0.3f);
        }

        if (!invenIconHit)
        {
            invenIcon = zFOXVPAD_BUTTON.NON;
            invenIconindex = -1;
            sprinvenIcon.color = new Color(0.3f, 0.3f, 0.3f);
        }

        if (!dialogueHit)
        {
            dialogue = zFOXVPAD_BUTTON.NON;
            dialogueindex = -1;
        }
    }

    bool CheckSlidePadDown(Vector2 posTouch)
    {
        if (posTouch.x / Screen.width < 0.5f)
        {
            movPadEnable = true;
            movEnable = true;
            movSt = posTouch;
            Vector3 vec3 = uicam.ScreenToWorldPoint(posTouch);
            vec3.z = sprSlidePad.transform.position.z;
            sprSlidePadBack.transform.position = vec3;

            sprSlidePad.color = new Color(1.0f, 1.0f, 1.0f);
            return true;
        }
        return false;
    }

    bool CheckSlidePadMove(Vector2 posTouch)
    {
        if (movPadEnable)
        {
            movEnable = true;
            mov = posTouch - movSt;
            sprSlidePad.color = new Color(1.0f, 1.0f, 1.0f);
            return true;
        }
        return false;
    }

    void CheckSlidePadUp()
    {
        sprSlidePad.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
    }
}
