using UnityEngine;

namespace Complete
{
    public class ShellExplosion2 : MonoBehaviour
    {
        public LayerMask m_TankMask;                        // 可以发生爆炸效果的图层
        public ParticleSystem m_ExplosionParticles;         // 爆炸效果的粒子系统
        public AudioSource m_ExplosionAudio;                // 爆炸的音效
        public float m_MaxDamage = 100f;                    // 爆炸最大的伤害值
        public float m_ExplosionForce = 1000f;              // 爆炸向外辐射的力的值
        public float m_MaxLifeTime = 2f;                    // 炮弹存在的时间
        public float m_ExplosionRadius = 5f;                // 爆炸影响的范围半径
        private GameObject targetRigidbody2;                // 玩家坦克
        private void Start()
        {
            // 如果没碰撞到其他东西，则2s后销毁自身
            Destroy(gameObject, m_MaxLifeTime);
            targetRigidbody2 = GameObject.FindWithTag("Player1");
        }


        private void OnTriggerEnter(Collider other)
        {
            // 以碰撞到的地方为中心，上面定义的半径球体内所有在tank层里的物体的collider集合
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);
            // 遍历这些碰撞器组件
            if (other.tag == "Player1")
            {
                // 获取Health组件
                TankHealth targetHealth = targetRigidbody2.GetComponent<TankHealth>();
                float damage = CalculateDamage(targetRigidbody2.transform.position);
                // 调用坦克身上的控制生命值的脚本来改变生命
                targetHealth.TakeDamage(damage);
            }

            // 脱离父子层级关系
            m_ExplosionParticles.transform.parent = null;

            // 播放粒子动画
            m_ExplosionParticles.Play();

            // 播放爆炸声音
            m_ExplosionAudio.Play();

            // 粒子动画播放完毕以后就销毁这个爆炸物体
            Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);

            // 销毁炮弹
            Destroy(gameObject);
        }


        private float CalculateDamage(Vector3 targetPosition)
        {
            // 目标位置到爆炸中心位置的向量
            Vector3 explosionToTarget = targetPosition - transform.position;

            // 计算出这个向量的长度
            float explosionDistance = explosionToTarget.magnitude;

            // 计算实际距离占爆炸影响的半径的百分比，越近越小
            float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

            // 计算伤害值(距离越近伤害值越大)
            float damage = relativeDistance * m_MaxDamage;

            // 如果半径超过了爆炸的最大半径，则上面得到的是负值，所以伤害值为0
            // 也就是如果超过了爆炸半径，则受到0伤害
            damage = Mathf.Max(0f, damage);
            return damage;
        }
    }
}