using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throw01 : MonoBehaviour
{
    public GameObject ballPro;
    public float initialSpeed = 20f; // 初始速度
    public float angle = 45f; // 抛出的角度
    public float gravity = 9.81f; // 重力加速度
    private bool chuqu = false;
    private GameObject ball=null;
    public float stopTime = 4f; // 物体静止时间
    private Coroutine stopCoroutine;
    public float speed = 500f; // 物体返回速度
    public HoloKit.iOS.HandGestureRecognitionManager gestureManager; 

    public GameObject imageUI1; // 第一个图像 UI纸去
    public GameObject imageUI2; // 第二个图像 UI纸来

    void Start()
    {
        ball = Instantiate(ballPro, transform.position+transform.forward*0.5f, Quaternion.identity);
        ball.transform.Rotate(-90, -180, -90);
        
        ball.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.useGravity = false;

        imageUI1.SetActive(false);
        imageUI2.SetActive(false);
    }

    private Vector3 targetPosition;

void Update()
{    
    //if (Input.GetMouseButtonDown(0))
	if (gestureManager.HandGesture == HoloKit.iOS.HandGesture.Pinched)
    {
        CreateBall();
        if (!imageUI1.activeSelf)
            {
                // 如果不可见，显示第一个图像 UI，隐藏第二个图像 UI
                imageUI1.SetActive(true);
                imageUI2.SetActive(false);
            }
    }
    //else if (Input.GetMouseButtonDown(1))
	else if (gestureManager.HandGesture == HoloKit.iOS.HandGesture.Five )
    {
        Debug.Log("Right mouse button pressed");
        chuqu = true; // 标记为开始返回状态
    }

    if (chuqu) // 在 chuqu 为 true 时持续调用 BackBall() 方法
    {
        BackBall();
        if (!imageUI2.activeSelf)
        {
                // 如果第一个图像 UI 可见，隐藏它并显示第二个图像 UI
                imageUI1.SetActive(false);
                imageUI2.SetActive(true);
            }
        
    }
}

    private void CreateBall()
    {   
        if (ball != null && !chuqu)
        {   
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.useGravity = true;
            Vector3 clickPosition = transform.position;
            clickPosition.z += 5f;
            //clickPosition.x += 1f;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(clickPosition);
            
            Vector3 velocity = CalculateVelocity(transform.position, clickPosition, initialSpeed, angle);
            rb.velocity = velocity;
            //chuqu = true;
            if (stopCoroutine != null)
            {
                StopCoroutine(stopCoroutine);
            }
            stopCoroutine = StartCoroutine(StopBallAfterDelay());
        }
    }

    private Vector3 CalculateVelocity(Vector3 origin, Vector3 target, float speed, float angle)
    {
        Vector3 direction = target - origin;
        direction.Normalize();

        float horizontalSpeed = Mathf.Cos(angle * Mathf.Deg2Rad) * speed;
        float verticalSpeed = Mathf.Sin(angle * Mathf.Deg2Rad) * speed;

        Vector3 horizontalVelocity = direction * horizontalSpeed;
        Vector3 verticalVelocity = Vector3.up * verticalSpeed;

        return horizontalVelocity + verticalVelocity;
    }

    private IEnumerator StopBallAfterDelay()
    {
        yield return new WaitForSeconds(stopTime);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
    }

    // 声明一个变量来存储物体的目标位置
private Vector3 target;

// 修改 BackBall() 方法
private void BackBall()
{
    if (ball != null)
    {
        chuqu = true;
        Debug.Log("BackBall() called");

        // 设置目标位置为当前鼠标点击的位置
        Vector3 clickPosition = transform.position;
        clickPosition.z = 0.5f;
        target = Camera.main.ScreenToWorldPoint(clickPosition);
        
        Debug.Log("Target position: " + target);

        // 将物体朝目标位置移动
        ball.transform.position = Vector3.MoveTowards(ball.transform.position, clickPosition, speed * Time.deltaTime*100);
        
        Debug.Log("Current position: " + ball.transform.position);

        // 如果物体已经到达目标位置，重置 chuqu 标志
        if (Vector3.Distance(ball.transform.position, target) < 1f)
        {
            chuqu = false;
            Debug.Log("Reached target position!");
        }
    }
}

}
