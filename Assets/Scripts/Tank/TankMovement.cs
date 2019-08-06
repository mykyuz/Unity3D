using UnityEngine;

namespace Complete
{
    public class TankMovement : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // 用来标记操作控制的是哪个坦克(或者理解为坦克编号)
        public float m_Speed = 12f;                 // 坦克移动的速度
        public float m_TurnSpeed = 180f;            // 坦克每秒旋转的角度
        public AudioSource m_MovementAudio;         // 播放声音
        public AudioClip m_EngineIdling;            // 待机时的音频
        public AudioClip m_EngineDriving;           // 移动时的音频
		public float m_PitchRange = 0.2f;           // 音频大小的波动范围


        private string m_MovementAxisName;          // 移动轴的名字
        private string m_TurnAxisName;              // 旋转轴的名字
        private Rigidbody m_Rigidbody;              // 刚体组件
        private float m_MovementInputValue;         // 当前输入的用来控制移动的数值
        private float m_TurnInputValue;             // 输入的旋转值
        private float m_OriginalPitch;              // 初始音频的值


        private void Awake ()
        {
            m_Rigidbody = GetComponent<Rigidbody> ();  //获取组件
        }


        private void OnEnable ()
        {
            // 坦克激活时，设置可以参与物理碰撞
            m_Rigidbody.isKinematic = false;

            // 移动，旋转值归0
            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;
        }


        private void OnDisable ()
        {
            // 坦克禁用时，不参与物理碰撞
            m_Rigidbody.isKinematic = true;
        }


        private void Start ()
        {
            // 根据编号不同，从键盘获取的值不同
            m_MovementAxisName = "Vertical";
            m_TurnAxisName = "Horizontal";

            // 音源的音调设为起始的音调
            m_OriginalPitch = m_MovementAudio.pitch;
        }


        private void Update ()
        {
            // 根据不同的输入给变量赋值
            m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
            m_TurnInputValue = Input.GetAxis (m_TurnAxisName);
            // 播放声音
            EngineAudio();
        }


        private void EngineAudio ()
        {
            // 如果什么按键也没按
            if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f)
            {
                // 如果当前播放的声音是正在移动的音频
                if (m_MovementAudio.clip == m_EngineDriving)
                {
                    // 则把当前的音频设为待机时的音频
                    m_MovementAudio.clip = m_EngineIdling;
                    // 音调在上面定义的区间里面取一个随机值
                    m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play ();  // 播放音频
                }
            }
            else
            {
                // 同上，如果按下了某个键，当前音频是待机的音频
                if (m_MovementAudio.clip == m_EngineIdling)
                {
                    // 则切换音频并播放
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }


        private void FixedUpdate ()
        {
            // 移动，旋转
            Move ();
            Turn ();
        }


        private void Move ()
        {
            // 创建一个向量，用来保存移动的距离
            Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

            // 改变当前物体的位置
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }


        private void Turn ()
        {
            // 每帧输入的值转换成角度值
            float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

            // 旋转的角度
            Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);

            // 应用旋转的角度
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
        }
    }
}