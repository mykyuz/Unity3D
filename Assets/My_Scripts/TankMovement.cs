using UnityEngine;
using System.Collections;

public class TankMovement : MonoBehaviour {
    public float speed = 5;  //移动的速度
    public float rotateSpeed = 5;   //旋转的速度
    public int tankNumber = 1;  //坦克的编号
    public AudioClip idleAudio; //待机时播放的声音
    public AudioClip runingAudio;   //移动时播放的声音

    AudioSource audio;  //音频组件
    Rigidbody tankRigidbody;  //刚体组件

    void Start () {
        //获取组件
        tankRigidbody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
	}
	
	void FixedUpdate () {
        //得到输入WS(垂直轴)为移动  AD(水平轴)为旋转
        float v = Input.GetAxis("Vertical"+tankNumber);
        float h = Input.GetAxis("Horizontal"+tankNumber);
        MoveAndRotation(h, v); //控制移动和旋转
        PlayAudio(h, v);  //控制播放的声音
	}
    void MoveAndRotation(float h, float v)
    {
        tankRigidbody.velocity = transform.forward * v * speed;
        //前后移动
        tankRigidbody.angularVelocity = transform.up * h * rotateSpeed;
        //左右旋转
    }

    void PlayAudio(float h, float v)
    {
        if (h != 0 || v != 0)
        {   //如果有值输入，则播放移动的声音
            audio.clip = runingAudio;
            if(audio.isPlaying==false)
                audio.Play();
        }
        else
        {   // 否则播放待机的声音
            audio.clip = idleAudio;
            if (audio.isPlaying == false)
                audio.Play();
        }
    }
}
