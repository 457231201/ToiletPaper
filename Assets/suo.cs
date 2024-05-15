using UnityEngine;

public class ShrinkAndReset : MonoBehaviour
{
    public float minScaleXY = 0.1f; // 物体缩小到的最小 x 和 y 轴缩放值
    public float shrinkSpeed = 0.1f; // 缩放速度

    private Vector3 initialScale;

    void Start()
    {
        // 记录初始缩放值
        initialScale = transform.localScale;
    }

    void Update()
    {
        // 计算当前缩放值
        float currentScaleX = Mathf.Max(minScaleXY, transform.localScale.x - shrinkSpeed * Time.deltaTime);
        float currentScaleY = Mathf.Max(minScaleXY, transform.localScale.y - shrinkSpeed * Time.deltaTime);

        // 更新物体的缩放
        transform.localScale = new Vector3(currentScaleX, currentScaleY, transform.localScale.z);

        // 输出调试信息
        Debug.Log("Current Scale X: " + currentScaleX + ", Current Scale Y: " + currentScaleY);
    }
}
