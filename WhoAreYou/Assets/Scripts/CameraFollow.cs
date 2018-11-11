using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CAMERATARGET
{
    PLAYER,
    PLAYER_MARGIN,
    PLAYER_GROUND
}

public enum CAMERAHOMING
{
    DIRECT,
    LERP,
    SLERP,
    STOP
}

public class CameraFollow : MonoBehaviour {
    [System.Serializable]

    public class Param
    {
        public CAMERATARGET targetType = CAMERATARGET.PLAYER_GROUND;
        public CAMERAHOMING homingType = CAMERAHOMING.LERP;
        public Vector2 margin = new Vector2(2.0f, 3.4f);
        public Vector2 homing = new Vector2(0.1f, 0.2f);

        public bool borderCheck = false;
        public float leftX = 0.0f;
        public float topY = 0.0f;
        public float rightX = 0.0f;

        public float max_xpos = 0.0f;
        public float min_xpos = 0.0f;

        // 카메라 밖으로 빠져나가지 못하게 하는 코드인데 버리고 다른 것을 쓴다.
        //public GameObject borderLeftTop;
        //public GameObject borderRightBottom;

        public bool viewAreaCheck = true;
        public Vector2 viewAreaMinMargin = new Vector2(0.0f, 0.0f);
        public Vector2 viewAreaMaxMargin = new Vector2(0.0f, 2.0f);

        public bool orthographicEnabled = true;
        public float screenOGSize = 5.0f;
        public float screenOGSizeHoming = 0.1f;
        public float screenPSSize = 50.0f;
        public float screenPSSizeHoming = 0.1f;
    }

    public Param param;

    //===캐쉬 =========
    GameObject player;
    Transform playerTrfm;
    PlayerController playerCtrl;

    float screenOGSizeAdd = 0.0f;
    float screenPSSizeAdd = 0.0f;

    //====코드 시작
    void Awake()
    {
        player = PlayerController.GetGameObject();
        playerTrfm = player.transform;
        playerCtrl = player.GetComponent<PlayerController>();
    }

    void LateUpdate()
    {
        float targetX = playerTrfm.position.x;
        float targetY = playerTrfm.position.y;
        float pX = transform.position.x;
        float pY = transform.position.y;
        float screenOGSize = Camera.main.orthographicSize; //여기
        float screenPSSize = Camera.main.fieldOfView; //여기

        // 대상 설정
        switch (param.targetType)
        {
            case CAMERATARGET.PLAYER:
                targetX = playerTrfm.position.x;
                targetY = playerTrfm.position.y;
                break;
            case CAMERATARGET.PLAYER_MARGIN:
                targetX = playerTrfm.position.x + param.margin.x * playerCtrl.dir;


                targetY = playerTrfm.position.y + param.margin.y;
                break;
            case CAMERATARGET.PLAYER_GROUND:
                targetX = playerTrfm.position.x + param.margin.x * playerCtrl.dir;
                targetY = playerCtrl.groundY + param.margin.y;
                break;
        }

        // 카메라 범위 체크, 아직 X밖에 안함
        if (targetX < param.min_xpos || targetX > param.max_xpos)
            param.borderCheck = true;
        else
            param.borderCheck = false;

        // 카메라 이동 한계선
        if (param.borderCheck)
        {
            if (targetX < param.leftX)
                targetX = param.leftX;
            else if (targetX > param.rightX)
                targetX = param.rightX;
            else if (targetY > param.topY)
                targetY = param.topY;
            
        }

        // 플레이어가 카메라 프레임 안에 있는지 검사한다.

        if (param.viewAreaCheck)
        {
            float z = playerTrfm.position.z - transform.position.z;
            Vector3 minMargin = param.viewAreaMinMargin;
            Vector3 maxMargin = param.viewAreaMaxMargin;
            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, z)) - minMargin;
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, z)) - maxMargin;
            if (playerTrfm.position.x < min.x || playerTrfm.position.x > max.x)
                targetX = playerTrfm.position.x;

            if(playerTrfm.position.y < min.y || playerTrfm.position.y > max.y)
            {
                targetY = playerTrfm.position.y;
                playerCtrl.groundY = playerTrfm.position.y;
            }
        }

        // 카메라 이동(호밍)
        switch(param.homingType)
        {
            case CAMERAHOMING.DIRECT:
                pX = targetX;
                pY = targetY;
                screenOGSize = param.screenOGSize;
                screenPSSize = param.screenPSSize;
                break;

            case CAMERAHOMING.LERP:
                pX = Mathf.Lerp(transform.position.x, targetX, param.homing.x);
                pY = Mathf.Lerp(transform.position.y, targetY, param.homing.y);
                screenOGSize = Mathf.Lerp(screenOGSize, param.screenOGSize, param.screenOGSizeHoming);
                screenPSSize = Mathf.Lerp(screenPSSize, param.screenPSSize, param.screenPSSizeHoming);
                break;

            case CAMERAHOMING.SLERP:
                pX = Mathf.SmoothStep(transform.position.x, targetX, param.homing.x);
                pY = Mathf.SmoothStep(transform.position.y, targetY, param.homing.y);
                screenOGSize = Mathf.SmoothStep(screenOGSize, param.screenOGSize, param.screenOGSizeHoming);
                screenPSSize = Mathf.SmoothStep(screenPSSize, param.screenPSSize, param.screenPSSizeHoming);
                break;

            case CAMERAHOMING.STOP:
                break;
        }

        transform.position = new Vector3(pX, pY, transform.position.z);
        Camera.main.orthographic = param.orthographicEnabled;
        Camera.main.orthographicSize = screenOGSize + screenOGSizeAdd;
        Camera.main.fieldOfView = screenPSSize + screenPSSizeAdd;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 2.5f, 10.0f);  //여기 전부
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 30.0f, 100.0f);

        screenOGSizeAdd *= 0.99f;
        screenPSSizeAdd *= 0.99f;
    }

    public void SetCamera(Param cameraPara)
    {
        param = cameraPara;
    }

    public void AddCameraSize(float ogAdd, float psAdd)
    {
        screenOGSizeAdd *= ogAdd;
        screenPSSizeAdd *= psAdd;
    }

    public void SetMaxXpos(int stage_number)
    {
        switch(stage_number)
        {
            case 1:
                param.max_xpos = 42.8f;
                param.min_xpos = 1.7f;
                param.leftX = 0.3f;
                param.rightX = 42.5f;
                param.screenOGSize = 5;
                break;
            case 2:
                param.max_xpos = 103.0f;
                param.rightX = 103.0f;
                break;
            case 3:
                param.max_xpos = 103.0f;
                param.min_xpos = 25.76f;
                param.leftX = 24.4f;
                param.rightX = 105.7f;
                param.screenOGSize = 5;
                break;
            case 4:
                param.rightX = 13.5f;
                param.max_xpos = 13.5f;
                break;
            default:
                break;
        }
        
    }
}
