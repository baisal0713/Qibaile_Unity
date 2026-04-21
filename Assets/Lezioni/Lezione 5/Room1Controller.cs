using DG.Tweening;
using UnityEngine;
using Ludocore;

/// <summary>
/// Room 1 interaction controller.
/// Detects player presence using Trigger Sensor and plays layered feedback animations.
/// Each feedback has its own DoTween animation with exposed curves.
/// </summary>
public class Room1Controller : MonoBehaviour
{
    //==================== CONFIG =====================
    [Header("Trigger Sensor")]
    [Tooltip("Trigger Sensor to detect player")]
    [SerializeField] private TriggerSensor triggerSensor;

    [Header("Audio Feedback")]
    [Tooltip("Audio Source for pitch animation")]
    [SerializeField] private AudioSource audioSource;
    [Min(0.1f)]
    [SerializeField] private float pitchEntryDuration = 1f;
    [SerializeField] private AnimationCurve pitchEntryCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [Min(0f)]
    [SerializeField] private float targetPitch = 1f;

    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;

    //==================== STATE =====================
    private Tween _pitchTween;
    private bool _isPlaying;
    private bool _wasDetected;

    //==================== LIFECYCLE =====================
    private void Update()
    {
        if (!triggerSensor) return;

        bool isDetected = triggerSensor.HasDetections;

        // Transition from no detection to detection
        if (isDetected && !_wasDetected)
        {
            Play();
        }

        _wasDetected = isDetected;
    }

    //==================== PUBLIC =====================
    /// <summary>Play the interaction feedback.</summary>
    public void Play()
    {
        if (_isPlaying) return;

        _isPlaying = true;
        PlayPitchAnimation();
    }

    /// <summary>Stop the interaction feedback.</summary>
    public void Stop()
    {
        _isPlaying = false;
        KillPitchTween();
    }

    //==================== PRIVATE =====================
    private void PlayPitchAnimation()
    {
        if (!audioSource)
        {
            Debug.LogWarning("[Room1Controller] No Audio Source assigned", gameObject);
            return;
        }

        KillPitchTween();

        // Animate pitch from current value to target
        _pitchTween = DOTween.To(
            () => audioSource.pitch,
            x => audioSource.pitch = x,
            targetPitch,
            pitchEntryDuration
        )
        .SetEase(pitchEntryCurve)
        .OnComplete(() => _isPlaying = false);
    }

    private void KillPitchTween()
    {
        if (_pitchTween is { active: true })
        {
            _pitchTween.Kill();
        }
        _pitchTween = null;
    }

    private void OnDestroy()
    {
        KillPitchTween();
    }

    //==================== DEBUG =====================
    private void OnGUI()
    {
        if (!showDebugInfo) return;

        GUI.Label(new Rect(10, 70, 300, 20), $"[Room1Controller] Playing: {_isPlaying}");
        
        if (triggerSensor)
        {
            GUI.Label(new Rect(10, 90, 300, 20), $"Detected: {triggerSensor.HasDetections}");
        }

        if (audioSource)
        {
            GUI.Label(new Rect(10, 110, 300, 20), $"Audio Pitch: {audioSource.pitch:F2}");
        }
    }
}