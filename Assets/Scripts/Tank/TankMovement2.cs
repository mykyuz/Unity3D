using UnityEngine;
using System.Collections;
namespace Complete
{
    public class TankMovement2 : MonoBehaviour
    {
        private Rigidbody m_Rigidbody;              // 刚体组件
        private float m_MovementInputValue;         // 当前输入的用来控制移动的数值
        private float m_TurnInputValue;             // 输入的旋转值
        private Transform playertrans;
        public NavMeshAgent nav;
        private void OnEnable()
        {
            // 移动，旋转值归0
            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;
        }


        private void OnDisable()
        {
            // 坦克禁用时，不参与物理碰撞
            m_Rigidbody.isKinematic = true;
        }


        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();  //获取组件
            nav = this.GetComponent<NavMeshAgent>();
            playertrans = GameObject.FindWithTag("Player1").transform;
            nav.Resume();
        }
        private void Update()
        {
            if (nav != null && playertrans!=null)
            {
                //寻路算法
                nav.SetDestination(playertrans.position);
                if (Vector3.Distance(this.transform.position, playertrans.position) < 20)
                {
                    nav.Stop();
                }
                else
                {
                    nav.Resume();
                }
               
            }
        }




    }


       
}