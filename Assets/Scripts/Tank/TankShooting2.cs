using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    public class TankShooting2 : MonoBehaviour
    {
        private Transform playertrans;
        public Rigidbody m_Shell;                   // 子弹的预设体
        public Transform m_FireTransform;           // 开火的位置
        public AudioSource m_ShootingAudio;         // 发射时播放的声音
        public AudioClip m_FireClip;                // 发射时播放的声音片段
        public float m_MinLaunchForce = 20f;        // 发射力度
        private float m_CurrentLaunchForce;         // 子弹当前发射值
        private bool m_Fired;                       // 是否发射的标志位，初始为否
        float i = 1f;                               //发射间隔
        private void OnEnable()
        {
     
        }


        private void Start ()
        {
            playertrans = GameObject.FindWithTag("Player1").transform;
        }


        private void Update()
        {
            if (i >= 1)
            {
                i = 0;
                 // 如果距离小于20，开火
                 if (Vector3.Distance(this.transform.position, playertrans.position) < 20 && playertrans!=null)
                {
                    this.transform.LookAt(playertrans);//朝向主角
                    m_CurrentLaunchForce = m_MinLaunchForce;
                    Fire ();

                }
            }

            i += Time.deltaTime;

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
            m_CurrentLaunchForce = m_MinLaunchForce;
        }
    }
}