using System;
using UnityEngine;

namespace Complete
{
    [Serializable]  // 序列化,使其可在检视面板中显示
    public class TankManager
    {
        
        public Color m_PlayerColor;                             // 坦克的颜色
        public Transform m_SpawnPoint;                          // 坦克的出生地
        [HideInInspector] public string m_ColoredPlayerText;    // 一个字符串表示和坦克编号的颜色一样
        [HideInInspector] public GameObject m_Instance;         // 一个坦克实例
        private TankMovement m_Movement;                        // 控制坦克移动的脚本
        private TankShooting m_Shooting;                        // 控制坦克开火的脚本
        private GameObject m_CanvasGameObject;                  // 显示蓄力的箭头UI


        public void Setup ()
        {
            // 通过物体获取组件
            m_Movement = m_Instance.GetComponent<TankMovement> ();
            m_Shooting = m_Instance.GetComponent<TankShooting> ();
            m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas> ().gameObject;

            // 设置坦克的编号，用来区分不同的移动和开火键

            // 一个带有颜色的字符串用来显示文字，颜色和坦克的颜色一样
            m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " +  "</color>";

            // 获取坦克的子物体身上的渲染组件
            MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer> ();

            // 遍历所有取得的子物体的渲染组件
            for (int i = 0; i < renderers.Length; i++)
            {
                // 给它的子物体的材质赋指定颜色
                renderers[i].material.color = m_PlayerColor;
            }
        }


        // 禁用坦克的控制(移动、开火、不显示UI)
        public void DisableControl ()
        {
            m_Movement.enabled = false;
            m_Shooting.enabled = false;

            m_CanvasGameObject.SetActive (false);
        }


        // 打开坦克的控制
        public void EnableControl ()
        {
            m_Movement.enabled = true;
            m_Shooting.enabled = true;

            m_CanvasGameObject.SetActive (true);            
        }


        // 每回合的开始重置坦克的状态(位置、旋转等)
        public void Reset ()
        {
            m_Instance.transform.position = m_SpawnPoint.position;
            m_Instance.transform.rotation = m_SpawnPoint.rotation;

            m_Instance.SetActive (false);
            m_Instance.SetActive (true);
        }
    }
}