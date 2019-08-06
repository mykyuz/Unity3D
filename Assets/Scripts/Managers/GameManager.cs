using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Complete
{
    public class GameManager : MonoBehaviour
    {
        public int m_NumRoundsToWin = 5;            // 胜利所要赢的回合数
        public float m_StartDelay = 3f;             // 每局开始的延时
        public float m_EndDelay = 3f;               // 结束的延时
        public CameraControl m_CameraControl;       // 控制相机的脚本的实例
        public Text m_MessageText;                  // 显示文字
        public GameObject m_TankPrefab;             // 坦克的实例
        public TankManager[] m_Tanks;               // 管理坦克的脚本(有多个)

        
        private int m_RoundNumber;                  // 当前游戏的回合数
        private WaitForSeconds m_StartWait;         // 开始等待的时间
        private WaitForSeconds m_EndWait;           // 结束等待的时间
        private TankManager m_RoundWinner;          // 每回合获胜的坦克
        private TankManager m_GameWinner;           // 最终赢的坦克


        private void Start()
        {
            // 初始化延时
            m_StartWait = new WaitForSeconds (m_StartDelay);
            m_EndWait = new WaitForSeconds (m_EndDelay);

            SpawnAllTanks();  // 创建坦克
            SetCameraTargets();  // 摄像机跟踪

            // 一切条件准备就绪以后，开始游戏循环
            StartCoroutine (GameLoop ());
        }


        private void SpawnAllTanks()
        {
            // 初始化所有坦克
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // 生成坦克
                m_Tanks[i].m_Instance =
                    Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position,
                                m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
                m_Tanks[i].Setup();
            }
        }


        private void SetCameraTargets()
        {
            // 创建一个数组用来存放所有要聚焦的坦克的位置
            Transform[] targets = new Transform[m_Tanks.Length];

            // 遍历所有的坦克
            for (int i = 0; i < targets.Length; i++)
            {
                // 赋值
                targets[i] = m_Tanks[i].m_Instance.transform;
            }

            // 设置相机的目标(在摄像机控制脚本中)
            m_CameraControl.m_Targets = targets;
        }


        // 游戏循环
        private IEnumerator GameLoop ()
        {
            // 回合开始（初始化坦克，显示回合等操作）
            yield return StartCoroutine (RoundStarting ());

            // 游戏开始
            yield return StartCoroutine (RoundPlaying());

            // 回合结束，开始结算(显示)
            yield return StartCoroutine (RoundEnding());

            // 如果产生了最终赢家，则重载游戏场景
            if (m_GameWinner != null)
            {
                // 重新开始
                SceneManager.LoadScene (0);
            }
            else
            {
                // 如果没有产生最终赢家，则继续gameloop
                // 本局的gameloop结束，继续执行下一个gameloop
                StartCoroutine (GameLoop ());
            }
        }


        private IEnumerator RoundStarting ()
        {
            // 每回合开始需要处理的事
            ResetAllTanks ();
            DisableTankControl ();

            // 设置相机初始的位置和焦距
            m_CameraControl.SetStartPositionAndSize ();

            // 回合数递增，并显示这是第几回合
            m_RoundNumber++; 
            m_MessageText.text = "ROUND " + m_RoundNumber;

            // 延时几秒后回到gameloop
            yield return m_StartWait;
        }


        private IEnumerator RoundPlaying ()
        {
            // 坦克可以开始被控制了
            EnableTankControl ();

            // 清除屏幕上显示的文字
            m_MessageText.text = string.Empty;

            // 如果场景中不止一个坦克存在则一直循环
            while (!OneTankLeft())
            {
                // 暂停协同程序，下一帧再继续往下执行               
                yield return null;
            }
        }


        private IEnumerator RoundEnding ()
        {
            // 禁用坦克移动
            DisableTankControl ();

            // 重置本局的胜者
            m_RoundWinner = null;

            // 找出本局的存活者为胜者
            m_RoundWinner = GetRoundWinner ();

            // 求出赢得比赛的玩家(当前赢得场数等于5)
            m_GameWinner = GetGameWinner ();
            // 计算需要显示的字符串，并显示
            string message = EndMessage ();
            m_MessageText.text = message;

            // 完成上面以后返回gameloop
            yield return m_EndWait;
        }


        // 检查是否只剩下一个坦克
        private bool OneTankLeft()
        {
            // 初始剩余的坦克为0
            int numTanksLeft = 0;

            // 遍历所有的坦克
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // 如果坦克是激活状态的，则数量递增
                if (m_Tanks[i].m_Instance.activeSelf)
                    numTanksLeft++;
            }

            // 如果场景中激活的坦克只剩下一个 则返回真，否则返回假
            return numTanksLeft <= 1;
        }
        
        
       // 得出本局的胜者
        private TankManager GetRoundWinner()
        {
            // 遍历所有的坦克
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // 找出那个还是激活状态的坦克并返回
                if (m_Tanks[i].m_Instance.activeSelf)
                    return m_Tanks[i]; 
            }

            // 如果没有坦克活着则返回空
            return null;
        }


        // 求出最终赢的玩家
        private TankManager GetGameWinner()
        {
            // 遍历所有坦克
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // 如果当前坦克累计的胜场等于前面定义的胜场数则返回
                    return m_Tanks[i];
            }

            // 如果没有，则返回空
            return null;
        }


        // 计算每局的胜者并返回字符串
        private string EndMessage()
        {
            // 一个字符串用来表示是谁赢了本局（默认是DRAW）
            string message = "DRAW!";

            // 如果有人赢了本局则显示(PLAYER1(玩家的颜色)WINS THE ROUND)
            if (m_RoundWinner != null)
                message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

            // 添加几个回车
            message += "\n\n\n\n";

            // 字符串后面继续递加
            // 下面显示：
            // PLAYER1：1 WINS
            // PLAYER2：2 WINS
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                message += m_Tanks[i].m_ColoredPlayerText + ": "  + " WINS\n";
            }

            // 如果游戏最终赢家不为空，则显示最终赢家
            if (m_GameWinner != null)
                message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

            return message;
        }


        // 重置坦克信息(位置，旋转等)
        private void ResetAllTanks()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].Reset();
            }
        }


        private void EnableTankControl() // 启用
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].EnableControl();
            }
        }


        private void DisableTankControl()  // 禁用
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].DisableControl();
            }
        }
    }
}