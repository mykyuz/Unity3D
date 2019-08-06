using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Complete
{
    public class GameManager : MonoBehaviour
    {
        public int m_NumRoundsToWin = 5;            // ʤ����ҪӮ�Ļغ���
        public float m_StartDelay = 3f;             // ÿ�ֿ�ʼ����ʱ
        public float m_EndDelay = 3f;               // ��������ʱ
        public CameraControl m_CameraControl;       // ��������Ľű���ʵ��
        public Text m_MessageText;                  // ��ʾ����
        public GameObject m_TankPrefab;             // ̹�˵�ʵ��
        public TankManager[] m_Tanks;               // ����̹�˵Ľű�(�ж��)

        
        private int m_RoundNumber;                  // ��ǰ��Ϸ�Ļغ���
        private WaitForSeconds m_StartWait;         // ��ʼ�ȴ���ʱ��
        private WaitForSeconds m_EndWait;           // �����ȴ���ʱ��
        private TankManager m_RoundWinner;          // ÿ�غϻ�ʤ��̹��
        private TankManager m_GameWinner;           // ����Ӯ��̹��


        private void Start()
        {
            // ��ʼ����ʱ
            m_StartWait = new WaitForSeconds (m_StartDelay);
            m_EndWait = new WaitForSeconds (m_EndDelay);

            SpawnAllTanks();  // ����̹��
            SetCameraTargets();  // ���������

            // һ������׼�������Ժ󣬿�ʼ��Ϸѭ��
            StartCoroutine (GameLoop ());
        }


        private void SpawnAllTanks()
        {
            // ��ʼ������̹��
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // ����̹��
                m_Tanks[i].m_Instance =
                    Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position,
                                m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
                m_Tanks[i].Setup();
            }
        }


        private void SetCameraTargets()
        {
            // ����һ�����������������Ҫ�۽���̹�˵�λ��
            Transform[] targets = new Transform[m_Tanks.Length];

            // �������е�̹��
            for (int i = 0; i < targets.Length; i++)
            {
                // ��ֵ
                targets[i] = m_Tanks[i].m_Instance.transform;
            }

            // ���������Ŀ��(����������ƽű���)
            m_CameraControl.m_Targets = targets;
        }


        // ��Ϸѭ��
        private IEnumerator GameLoop ()
        {
            // �غϿ�ʼ����ʼ��̹�ˣ���ʾ�غϵȲ�����
            yield return StartCoroutine (RoundStarting ());

            // ��Ϸ��ʼ
            yield return StartCoroutine (RoundPlaying());

            // �غϽ�������ʼ����(��ʾ)
            yield return StartCoroutine (RoundEnding());

            // �������������Ӯ�ң���������Ϸ����
            if (m_GameWinner != null)
            {
                // ���¿�ʼ
                SceneManager.LoadScene (0);
            }
            else
            {
                // ���û�в�������Ӯ�ң������gameloop
                // ���ֵ�gameloop����������ִ����һ��gameloop
                StartCoroutine (GameLoop ());
            }
        }


        private IEnumerator RoundStarting ()
        {
            // ÿ�غϿ�ʼ��Ҫ�������
            ResetAllTanks ();
            DisableTankControl ();

            // ���������ʼ��λ�úͽ���
            m_CameraControl.SetStartPositionAndSize ();

            // �غ�������������ʾ���ǵڼ��غ�
            m_RoundNumber++; 
            m_MessageText.text = "ROUND " + m_RoundNumber;

            // ��ʱ�����ص�gameloop
            yield return m_StartWait;
        }


        private IEnumerator RoundPlaying ()
        {
            // ̹�˿��Կ�ʼ��������
            EnableTankControl ();

            // �����Ļ����ʾ������
            m_MessageText.text = string.Empty;

            // ��������в�ֹһ��̹�˴�����һֱѭ��
            while (!OneTankLeft())
            {
                // ��ͣЭͬ������һ֡�ټ�������ִ��               
                yield return null;
            }
        }


        private IEnumerator RoundEnding ()
        {
            // ����̹���ƶ�
            DisableTankControl ();

            // ���ñ��ֵ�ʤ��
            m_RoundWinner = null;

            // �ҳ����ֵĴ����Ϊʤ��
            m_RoundWinner = GetRoundWinner ();

            // ���Ӯ�ñ��������(��ǰӮ�ó�������5)
            m_GameWinner = GetGameWinner ();
            // ������Ҫ��ʾ���ַ���������ʾ
            string message = EndMessage ();
            m_MessageText.text = message;

            // ��������Ժ󷵻�gameloop
            yield return m_EndWait;
        }


        // ����Ƿ�ֻʣ��һ��̹��
        private bool OneTankLeft()
        {
            // ��ʼʣ���̹��Ϊ0
            int numTanksLeft = 0;

            // �������е�̹��
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // ���̹���Ǽ���״̬�ģ�����������
                if (m_Tanks[i].m_Instance.activeSelf)
                    numTanksLeft++;
            }

            // ��������м����̹��ֻʣ��һ�� �򷵻��棬���򷵻ؼ�
            return numTanksLeft <= 1;
        }
        
        
       // �ó����ֵ�ʤ��
        private TankManager GetRoundWinner()
        {
            // �������е�̹��
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // �ҳ��Ǹ����Ǽ���״̬��̹�˲�����
                if (m_Tanks[i].m_Instance.activeSelf)
                    return m_Tanks[i]; 
            }

            // ���û��̹�˻����򷵻ؿ�
            return null;
        }


        // �������Ӯ�����
        private TankManager GetGameWinner()
        {
            // ��������̹��
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // �����ǰ̹���ۼƵ�ʤ������ǰ�涨���ʤ�����򷵻�
                    return m_Tanks[i];
            }

            // ���û�У��򷵻ؿ�
            return null;
        }


        // ����ÿ�ֵ�ʤ�߲������ַ���
        private string EndMessage()
        {
            // һ���ַ���������ʾ��˭Ӯ�˱��֣�Ĭ����DRAW��
            string message = "DRAW!";

            // �������Ӯ�˱�������ʾ(PLAYER1(��ҵ���ɫ)WINS THE ROUND)
            if (m_RoundWinner != null)
                message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

            // ��Ӽ����س�
            message += "\n\n\n\n";

            // �ַ�����������ݼ�
            // ������ʾ��
            // PLAYER1��1 WINS
            // PLAYER2��2 WINS
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                message += m_Tanks[i].m_ColoredPlayerText + ": "  + " WINS\n";
            }

            // �����Ϸ����Ӯ�Ҳ�Ϊ�գ�����ʾ����Ӯ��
            if (m_GameWinner != null)
                message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

            return message;
        }


        // ����̹����Ϣ(λ�ã���ת��)
        private void ResetAllTanks()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].Reset();
            }
        }


        private void EnableTankControl() // ����
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].EnableControl();
            }
        }


        private void DisableTankControl()  // ����
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].DisableControl();
            }
        }
    }
}