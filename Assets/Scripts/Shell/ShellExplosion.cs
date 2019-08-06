using UnityEngine;

namespace Complete
{
    public class ShellExplosion : MonoBehaviour
    {
        public LayerMask m_TankMask;                        // ���Է�����ըЧ����ͼ��
        public ParticleSystem m_ExplosionParticles;         // ��ըЧ��������ϵͳ
        public AudioSource m_ExplosionAudio;                // ��ը����Ч
        public float m_MaxDamage = 100f;                    // ��ը�����˺�ֵ
        public float m_ExplosionForce = 1000f;              // ��ը������������ֵ
        public float m_MaxLifeTime = 2f;                    // �ڵ����ڵ�ʱ��
        public float m_ExplosionRadius = 5f;                // ��ըӰ��ķ�Χ�뾶
        private void Start ()
        {
            // ���û��ײ��������������2s����������
            Destroy (gameObject, m_MaxLifeTime);
        }


        private void OnTriggerEnter (Collider other)
        {
			// ����ײ���ĵط�Ϊ���ģ����涨��İ뾶������������tank����������collider����
            Collider[] colliders = Physics.OverlapSphere (transform.position, m_ExplosionRadius, m_TankMask);

            // ������Щ��ײ�����
            for (int i = 0; i < colliders.Length; i++)
            {
                // ��ȡ�����ϵĸ������
                Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody> ();
                // ���û�и�������Ļ���������������
                if (!targetRigidbody)
                    continue;

                // �����������Ϊ(���Ĵ�С����������λ�ã����İ뾶)
                targetRigidbody.AddExplosionForce (m_ExplosionForce, transform.position, m_ExplosionRadius);

                // ��ȡHealth���
                TankHealth2 targetHealth2 = targetRigidbody.GetComponent<TankHealth2>();
                // ���û�еĻ������������һ��
                if (!targetHealth2)
                    continue;

                // ���ݾ��뱬ըλ�õ�Զ�������˺�ֵ
                float damage = CalculateDamage (targetRigidbody.position);

                // ����̹�����ϵĿ�������ֵ�Ľű����ı�����
                targetHealth2.TakeDamage2 (damage);
            }

            // ���븸�Ӳ㼶��ϵ
            m_ExplosionParticles.transform.parent = null;

            // �������Ӷ���
            m_ExplosionParticles.Play();

            // ���ű�ը����
            m_ExplosionAudio.Play();

            // ���Ӷ�����������Ժ�����������ը����
            Destroy (m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);

            // �����ڵ�
            Destroy (gameObject);
        }


        private float CalculateDamage (Vector3 targetPosition)
        {
            // Ŀ��λ�õ���ը����λ�õ�����
            Vector3 explosionToTarget = targetPosition - transform.position;

            // �������������ĳ���
            float explosionDistance = explosionToTarget.magnitude;

            // ����ʵ�ʾ���ռ��ըӰ��İ뾶�İٷֱȣ�Խ��ԽС
            float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

            // �����˺�ֵ(����Խ���˺�ֵԽ��)
            float damage = relativeDistance * m_MaxDamage;

            // ����뾶�����˱�ը�����뾶��������õ����Ǹ�ֵ�������˺�ֵΪ0
            // Ҳ������������˱�ը�뾶�����ܵ�0�˺�
            damage = Mathf.Max (0f, damage);
            return damage; 
        }
    }
}