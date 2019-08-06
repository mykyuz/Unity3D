using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    public class TankShooting : MonoBehaviour
    {
        
        public Rigidbody m_Shell;                   // 子弹的预设体
        public Transform m_FireTransform;           // 开火的位置
        public Slider m_AimSlider;                  // 显示蓄力大小的箭头(滑块)
        public AudioSource m_ShootingAudio;         // 发射时播放的声音
        public AudioClip m_ChargingClip;            // 蓄力时播放的声音片段
        public AudioClip m_FireClip;                // 发射时播放的声音片段
        public float m_MinLaunchForce = 15f;        // 最小的发射力度
        public float m_MaxLaunchForce = 30f;        // 最大的发射力度
        public float m_MaxChargeTime = 0.75f;       // 从最小到最大蓄力所需的时间
        private float m_CurrentLaunchForce;         // 子弹当前发射值
        private float m_ChargeSpeed;                // 蓄力的速度
        private bool m_Fired;                       // 是否发射的标志位，初始为否
        private void OnEnable()
        {
            // 当坦克启用，初始化力的大小和蓄力条的大小
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_AimSlider.value = m_MinLaunchForce;
        }


        private void Start ()
        {
            // 计算出蓄力的速度(长度除以时间)
            m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
        }


        private void Update ()
        {
            // 滑块的值等于力的最小值
            m_AimSlider.value = m_MinLaunchForce;

            // 如果当前的蓄力值大于最大蓄力值并且子弹还没有发射
            if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
            {
                // 则把当前蓄力值赋值为最大蓄力值并发射
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire ();
            }
            // 否则，如果按下了开火键（不蓄力，直接发射）
            else if (Input.GetMouseButtonDown(0))
            {
                // 子弹发射状态设为假(已经发射了)
                m_Fired = false;
                // 当前蓄力值等于最小蓄力值
                m_CurrentLaunchForce = m_MinLaunchForce; 

                // 设置当前声音源为蓄力音源，并播放
                m_ShootingAudio.clip = m_ChargingClip;
                m_ShootingAudio.Play ();
            }
            // 如果开火键一直按着的并且子弹还没有发射出去(蓄力阶段)
            else if (Input.GetMouseButton(0) && !m_Fired)
            {
                // 当前的蓄力值递增(蓄力的速度乘以时间)
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
                // 并相对的设置滑块的值(显示蓄力的箭头)
                m_AimSlider.value = m_CurrentLaunchForce;
            }
            // 如果松开了开火键并且子弹还没发射
            else if (Input.GetMouseButtonUp(0) && !m_Fired)
            {
                // 松开了当然要发射了 
                Fire ();
            }
        }


        private void Fire ()  // 开火的具体实现
        {
            // 是否发射的标志位设为真
            m_Fired = true;

            // 初始化一个子弹实例并得到它身上的刚体组件
            Rigidbody shellInstance = Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;     
            // 为其添加力(方向向前)
            shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward; 

            // 设置音源为炮弹发射的声音并播放
            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play ();

            // 当前蓄力值赋值为最小值
            m_CurrentLaunchForce = m_MinLaunchForce;
        }
    }
}