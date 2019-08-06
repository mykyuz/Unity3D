using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;                 // 相机移动和聚焦所用的时间
    public float m_ScreenEdgeBuffer = 4f;           // 相机视口边缘缓冲范围
    public float m_MinSize = 6.5f;                  // 最小焦距值
    [HideInInspector]
    public Transform[] m_Targets; // 相机所要显示的所有目标


    private Camera m_Camera;                        // 摄像机
    private float m_ZoomSpeed;                      // 视口缩放时的速度
    private Vector3 m_MoveVelocity;                 // 摄像机跟踪移动时的速度
    private Vector3 m_DesiredPosition;              // 摄像机要跟随的位置(2个或多个坦克的中间位置)


    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>(); // 获取组件
    }

    private void FixedUpdate()
    {
        // 移动摄像机更随目标
        Move();

        // 缩放摄像机视口以包含所有目标
        Zoom();
    }

    private void Move()
    {
        // 求出目标物体的中间位置
        FindAveragePosition();

        // 在0.2s内平滑的从当前位置移动到目标位置
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }

    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        // 遍历所有的需要计算的物体，求出它们的位置之和
        for (int i = 0; i < m_Targets.Length; i++)
        {
            // 如果这个物体没有激活，则跳过这个遍历下一个
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            // 位置之和
            averagePos += m_Targets[i].position;
            numTargets++; // 目标物体的数量
        }

        // 如果目标的数量大于0，则计算出中间位置
        if (numTargets > 0)
            averagePos /= numTargets;

        // 保持Y轴(高度)不变
        averagePos.y = transform.position.y;

        // 把计算出的位置赋值给目标位置
        m_DesiredPosition = averagePos;
    }


    private void Zoom()
    {
        // 计算出要焦距值，并0.2s内从当前焦距值平滑过度到目标焦距值
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }


    private float FindRequiredSize()
    {
        // 把目标(中间)位置从世界坐标转换为本地坐标
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);       
        
        // 焦距值初始为0
        float size = 0f;

        // 遍历目标
        for (int i = 0; i < m_Targets.Length; i++)
        {
            // 如果没有激活的则跳过
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            // 否则，获取当前物体的坐标，并由世界坐标转换为本地坐标
            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

            // 根据本地坐标，计算出物体位置到中间位置的差值，得到目标位置向量
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            // 坦克相对于中间位置的上下方位（Y轴），取得离中间位置最远的那个值
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            // 坦克位置相对于中间位置的左右方位（X轴），计算size（size=distance/aspect）值，
            // 和上面计算的左右值比较，取它们中的最大的那个值
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
        }

        // 然后在加上屏幕边缘的缓冲区大小
        size += m_ScreenEdgeBuffer;

        // 如果2个坦克靠的太近，小于最小焦距值，则把焦距设为上面定义的最小焦距值
        size = Mathf.Max(size, m_MinSize);

        return size; // 返回这个焦距值
    }


    public void SetStartPositionAndSize()
    {
        // 计算中间位置值
        FindAveragePosition();

        // 设置相机位置位于这个中间值位置
        transform.position = m_DesiredPosition;

        // 设置最佳焦距值（包含所有的目标物体）
        m_Camera.orthographicSize = FindRequiredSize();
    }
}