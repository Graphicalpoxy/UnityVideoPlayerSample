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
        // この設定をしないとFrameReadyのイベントは発火されないので注意
        videoPlayer.sendFrameReadyEvents = true;
    }

    private void Start()
    {
        SetupVideoPlayer();
    }

    /// <summary>
    /// 動画が最終フレームに到達したとき
    /// </summary>
    /// <param name="vp"></param>
    private void OnLoopPointReached(VideoPlayer vp)
    {
        // 例として、自動で一時停止させる
        videoPlayer.Pause();
    }

    /// <summary>
    /// 再生準備が完了したとき
    /// </summary>
    /// <param name="vp"></param>
    private void OnPrepareCompleted(VideoPlayer vp)
    {
        SetThumNail();
    }

    /// <summary>
    /// 表示するフレームの準備が完了したとき
    /// </summary>
    /// <param name="vp"></param>
    /// <param name="frameIndex"></param>
    private void OnFrameReady(VideoPlayer vp,long frameIndex)
    {
        // 再生・シークなど更新が入るたびに叩かれる関数
    }

    private void SetupVideoPlayer()
    {
        Debug.Log((int)videoPlayer.width);
        // 動画表示用のレンダーテクスチャーを作成
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
        // サムネイルにしたいフレームも設定可能
        Seek(100);
    }

    /// <summary>
    /// 動画再生
    /// </summary>
    public void Play()
    {
        videoPlayer.Play();
    }

    /// <summary>
    /// 一時停止
    /// </summary>
    public void Pause()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
    }

    /// <summary>
    /// シーク
    /// </summary>
    /// <param name="frameIndex">飛ばしたいフレーム</param>
    public void Seek(int frameIndex)
    {
        if (frameIndex > 0 && frameIndex <= (int)videoPlayer.frameCount)
        {
            videoPlayer.frame = frameIndex;
        }
    }

    /// <summary>
    /// コマ送り
    /// </summary>
    public void ForwardFrame()
    {
        // 念のため一時停止
        Pause();

        var currentFrame = (int)videoPlayer.frame;

        // フレームの整合性はシークの関数で行う
        Seek(currentFrame + 1);
    }

    /// <summary>
    /// コマ戻し
    /// </summary>
    public void BackFrame()
    {
        // 念のため一時停止
        Pause();

        var currentFrame = (int)videoPlayer.frame;

        // フレームの整合性はシークの関数で行う
        Seek(currentFrame - 1);
    }

    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnLoopPointReached;
        videoPlayer.frameReady -= OnFrameReady;
    }
}
