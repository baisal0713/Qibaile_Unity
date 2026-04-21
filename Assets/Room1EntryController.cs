using UnityEngine;
using DG.Tweening;

namespace Ludocore
{
    /// <summary>Plays layered feedback when the player enters Room 1.</summary>
    public class Room1EntryController : MonoBehaviour
    {
        //==================== CONFIG =====================
        [Header("Source")]
        [Tooltip("Sensor that detects the player")]
        [SerializeField] private Sensor sensor;

        [Header("Audio")]
        [Tooltip("Audio source to animate")]
        [SerializeField] private AudioSource audioSource;

        [Tooltip("Duration of the pitch ramp from 0 to 1")]
        [Min(0f)]
        [SerializeField] private float pitchEntryDuration = 1f;

        [Tooltip("Curve for the pitch animation")]
        [SerializeField] private AnimationCurve pitchEntryCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        //==================== STATE =====================
        private Tween _pitchTween;

        //==================== LIFECYCLE =====================
        private void OnEnable()
        {
            if (sensor) sensor.OnSignalAdded += HandleDetected;
        }

        private void OnDisable()
        {
            if (sensor) sensor.OnSignalAdded -= HandleDetected;
        }

        private void OnDestroy() => _pitchTween?.Kill();

        //==================== INPUTS =====================
        /// <summary>Play the entry interaction.</summary>
        [ContextMenu("On Play")]
        public void OnPlay()
        {
            _pitchTween?.Kill();

            audioSource.pitch = 0f;
            _pitchTween = DOTween.To(
                () => audioSource.pitch,
                x => audioSource.pitch = x,
                1f,
                pitchEntryDuration
            ).SetEase(pitchEntryCurve);
        }

        //==================== PRIVATE =====================
        private void HandleDetected(Signal signal)
        {
            OnPlay();
        }
    }
}
