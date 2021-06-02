using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private RawImage displayRawImage;

    private void Awake()
    {
        videoPlayer.loopPointReached += OnLoopPointReached;
        videoPlayer.prepareCompleted += OnPrepareCompleted;

        videoPlayer.frameReady += OnFrameReady;
        // ���̐ݒ�����Ȃ���FrameReady�̃C�x���g�͔��΂���Ȃ��̂Œ���
        videoPlayer.sendFrameReadyEvents = true;
    }

    private void Start()
    {
        SetupVideoPlayer();
    }

    /// <summary>
    /// ���悪�ŏI�t���[���ɓ��B�����Ƃ�
    /// </summary>
    /// <param name="vp"></param>
    private void OnLoopPointReached(VideoPlayer vp)
    {
        // ��Ƃ��āA�����ňꎞ��~������
        videoPlayer.Pause();
    }

    /// <summary>
    /// �Đ����������������Ƃ�
    /// </summary>
    /// <param name="vp"></param>
    private void OnPrepareCompleted(VideoPlayer vp)
    {
        SetThumNail();
    }

    /// <summary>
    /// �\������t���[���̏��������������Ƃ�
    /// </summary>
    /// <param name="vp"></param>
    /// <param name="frameIndex"></param>
    private void OnFrameReady(VideoPlayer vp,long frameIndex)
    {
        // �Đ��E�V�[�N�ȂǍX�V�����邽�тɒ@�����֐�
    }

    private void SetupVideoPlayer()
    {
        Debug.Log((int)videoPlayer.width);
        // ����\���p�̃����_�[�e�N�X�`���[���쐬
        var renderTexture = new RenderTexture(1920, 1080,0);
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        displayRawImage.texture = renderTexture;

        videoPlayer.Prepare();
    }

    public void SetThumNail()
    {
        Play();
        Pause();
        // �T���l�C���ɂ������t���[�����ݒ�\
        Seek(100);
    }

    /// <summary>
    /// ����Đ�
    /// </summary>
    public void Play()
    {
        videoPlayer.Play();
    }

    /// <summary>
    /// �ꎞ��~
    /// </summary>
    public void Pause()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
    }

    /// <summary>
    /// �V�[�N
    /// </summary>
    /// <param name="frameIndex">��΂������t���[��</param>
    public void Seek(int frameIndex)
    {
        if (frameIndex > 0 && frameIndex <= (int)videoPlayer.frameCount)
        {
            videoPlayer.frame = frameIndex;
        }
    }

    /// <summary>
    /// �R�}����
    /// </summary>
    public void ForwardFrame()
    {
        // �O�̂��߈ꎞ��~
        Pause();

        var currentFrame = (int)videoPlayer.frame;

        // �t���[���̐������̓V�[�N�̊֐��ōs��
        Seek(currentFrame + 1);
    }

    /// <summary>
    /// �R�}�߂�
    /// </summary>
    public void BackFrame()
    {
        // �O�̂��߈ꎞ��~
        Pause();

        var currentFrame = (int)videoPlayer.frame;

        // �t���[���̐������̓V�[�N�̊֐��ōs��
        Seek(currentFrame - 1);
    }

    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnLoopPointReached;
        videoPlayer.frameReady -= OnFrameReady;
    }
}
