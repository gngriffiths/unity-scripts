using UnityEngine;

// Performance settings for Quest. Settings can also be set via the Editor.

public class QuestGraphicsSettings : MonoBehaviour
{
    // Supported refresh rates on Quest 2.
    public enum RefreshRates
    {
        r72 = 72,
        r90 = 90,
        r120 = 120
    }

    [SerializeField] private bool _enableSpacewarpOnStart = true, _useDynamicFFR = false;
    [SerializeField] private OVRManager.FoveatedRenderingLevel _ffrLevelOnStart = OVRManager.FoveatedRenderingLevel.Off;
    [SerializeField] private RefreshRates _standardFramerate = RefreshRates.r90;

    void Start()
    {
        //Set spacwarp
        if (_enableSpacewarpOnStart) SetSpaceWarp(true);

        //Set FFR. Recommended to enable Subsampled Layout in Oculus Android settings for better visuals
        if (OVRManager.fixedFoveatedRenderingSupported)
        {
            SetFFR(_ffrLevelOnStart, _useDynamicFFR);
        }

        //Set framerate
        SetRefreshRate(_standardFramerate);

        //Extra performance tools
        OVRManager.suggestedCpuPerfLevel = OVRManager.ProcessorPerformanceLevel.SustainedHigh;
        OVRManager.suggestedGpuPerfLevel = OVRManager.ProcessorPerformanceLevel.SustainedHigh;
    }

    static public void SetSpaceWarp(bool enable)
    {
        OVRManager.SetSpaceWarp(enable);
    }

    static public void SetFFR(OVRManager.FoveatedRenderingLevel ffrLevel, bool useDynamic = false)
    {
        OVRManager.foveatedRenderingLevel = ffrLevel;
        OVRManager.useDynamicFoveatedRendering = useDynamic;
    }

    static public void SetRefreshRate(RefreshRates refreshRate)
    {
        OVRPlugin.systemDisplayFrequency = (int)refreshRate;

        Application.targetFrameRate = -1;           // -1 for as fast as possible (better than setting it to the same as systemDisplayFrequency.
    }
}
