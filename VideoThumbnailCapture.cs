using UnityEngine.Video;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class VideoThumbnailCapture
{
    bool videoplayerReady;

    string outputDirectory = Application.persistentDataPath + "/VideoThumbnails/";

    // Makes an API Video Player.
    private VideoPlayer videoPlayer
    {
        get
        {
            if (_videoPlayer == null)
            {
                GameObject ThumbnailProcessor = new GameObject("Thumbnail Processor");
                UnityEngine.Object.DontDestroyOnLoad(ThumbnailProcessor);
                _videoPlayer = ThumbnailProcessor.AddComponent<VideoPlayer>();
                _videoPlayer.renderMode = VideoRenderMode.APIOnly;
                _videoPlayer.playOnAwake = false;
                _videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
            }
            return _videoPlayer;
        }
    }
    private VideoPlayer _videoPlayer;

    public async UniTask<string> GenerateThumbnail(string videoPath)
    {
        if (string.IsNullOrEmpty(videoPath))
        {
            //Debug.LogError("No video selected.");
            return null;
        }
        Debug.Log("Generate video thumbnail for: " + videoPath);

        videoplayerReady = false;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoPath;


        PrepareVideoForProcessing();            // When video is ready, it will call VideoplayerReadyForCapture.

        // Await while processing
        await UniTask.WaitUntil(() => videoplayerReady);

        return SaveImage(_videoPlayer);
    }

    void PrepareVideoForProcessing(int frameToCapture = 0)
    {
        videoPlayer.sendFrameReadyEvents = true;
        videoPlayer.frameReady += VideoplayerReadyForCapture;                   // When video is ready, it will call VideoplayerReadyForCapture.
        videoPlayer.frame = frameToCapture;
        //videoPlayer.Play();                                                   // If video thumbnail is not being captured try uncommenting this.
        videoPlayer.Pause();
    }

    private void VideoplayerReadyForCapture(VideoPlayer _videoPlayer, long frameIdx)        // Called when videoPlayer.frameReady.
    {
        videoplayerReady = true;
        videoPlayer.sendFrameReadyEvents = false;
        videoPlayer.frameReady -= VideoplayerReadyForCapture;
    }

    string SaveImage(VideoPlayer _videoPlayer)
    {
        Texture2D tex = new Texture2D(2, 2);
        RenderTexture renderTexture = _videoPlayer.texture as RenderTexture;
        if (tex.width != renderTexture.width || tex.height != renderTexture.height)
        {
            tex.Reinitialize(renderTexture.width, renderTexture.height);
        }

        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
        renderTexture.Release();
        RenderTexture.active = currentActiveRT;

        // Set thumbnail name from video name
        string videoName = System.IO.Path.GetFileNameWithoutExtension(videoPlayer.url);
        string exportImagePath = outputDirectory + videoName + ".jpg";
        if (!System.IO.Directory.Exists(outputDirectory))
            System.IO.Directory.CreateDirectory(outputDirectory);

        byte[] imageBytes = tex.EncodeToJPG();
        System.IO.File.WriteAllBytes(exportImagePath, imageBytes);

        Debug.Log("Thumbnail saved to: " + exportImagePath);
        return exportImagePath;
    }
}
