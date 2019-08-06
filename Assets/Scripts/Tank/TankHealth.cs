using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
namespace Complete
{
    public class TankHealth : MonoBehaviour
    {
        public float m_StartingHealth = 100f;               // 初始生命值
        public Slider m_Slider;                             // 滑块组件
        public Image m_FillImage;                           // 滑块填充组件
        public Color m_FullHealthColor = Color.green;       // 满血状态时的血条颜色
        public Color m_ZeroHealthColor = Color.red;         // 空血的血条状态颜色
        public GameObject m_ExplosionPrefab;                // 爆炸效果物体
        private AudioSource m_ExplosionAudio;               // 爆炸音效
        private ParticleSystem m_ExplosionParticles;        // 爆炸的粒子效果
        private float m_CurrentHealth;                      // 当前生命值
        private bool m_Dead;                                // 是否死了
        public GameObject gameover;
        private void Awake()
        {
            // 初始化一个爆炸效果的游戏物体，并获取它身上的ParticleSystem（粒子系统）组件
            m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();

            // 顺便得到它的声音组件
            m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

            // 暂时不激活这个游戏物体
            m_ExplosionParticles.gameObject.SetActive(false);
        }


        private void OnEnable()
        {
            // 一开始，设置当前生命值为初始生命值
            m_CurrentHealth = m_StartingHealth;
            m_Dead = false;  // 是否死亡设为否

            // 更新UI显示的血条
            SetHealthUI();
        }


        public void TakeDamage(float amount)   //被击中时调用
        {
            // 当前生命值-=受到的伤害值
            m_CurrentHealth -= amount;
            // 更新UI显示的血条
            SetHealthUI();

            // 如果当前生命值<=0 了，并且是否死亡还没有被设为真
            if (m_CurrentHealth <= 0f && !m_Dead)
            {
                OnDeath(); //正在死亡方法
            }
        }


        private void SetHealthUI()  //设置UI更新血条的显示
        {
            // 设置滑块的值等于当前生命值
            m_Slider.value = m_CurrentHealth;

            // 差值运算得到显示的过度颜色，从0~100之间取值
            m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
        }


        private void OnDeath()  //正在死亡(处理后事)
        {
            // 死亡标志位设为真
            m_Dead = true;

            // 设置爆炸的位置为当前游戏体的位置，并且激活这个物体
            m_ExplosionParticles.transform.position = transform.position;
            m_ExplosionParticles.gameObject.SetActive(true);

            // 播放爆炸粒子动画
            m_ExplosionParticles.Play();

            // 播放爆炸声音
            m_ExplosionAudio.Play();
            // 关闭这个坦克
            Destroy(this.gameObject);
            gameover.SetActive(true);
        }
    }
}