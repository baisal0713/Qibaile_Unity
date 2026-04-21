using UnityEngine;
using UnityEngine.Rendering;

namespace Ludocore
{
    /// <summary>Compacts corridor walls based on player proximity to the corridor exit.
    /// Applies wall compaction, texture scrolling, and post-processing effects.
    /// Reads all tuning values from a swappable CompactingCorridorProfile asset.</summary>
    public class CompactingCorridorController : MonoBehaviour
    {
        //==================== SCENE REFERENCES =====================
        [Header("Scene References")]
        [Tooltip("Proximity sensor placed at or near the corridor exit")]
        [SerializeField] private ProximitySensor sensor;

        [SerializeField] private Transform wallA;
        [SerializeField] private Transform wallB;

        //==================== CONFIG =====================
        [Header("Config")]
        [Tooltip("Local-space direction each wall moves inward (wallA +axis, wallB −axis)")]
        [SerializeField] private Vector3 compactionAxis = Vector3.forward;

        //==================== PROFILE =====================
        [Header("Profile")]
        [Tooltip("Scriptable object with compaction and smoothing settings")]
        [SerializeField] private CompactingCorridorProfile profile;

        //==================== STATE =====================
        private Vector3 _restA;
        private Vector3 _restB;
        private float _currentOffset;

        //==================== LIFECYCLE =====================
        private void Start()
        {
            if (wallA) _restA = wallA.localPosition;
            if (wallB) _restB = wallB.localPosition;
        }

        private void Update()
        {
            if (!sensor || !wallA || !wallB || !profile) return;

            // --- Evaluate raw target from proximity ---
            float offsetTarget = 0f;

            if (sensor.TryGetNearest(out Signal nearest))
            {
                float proximity = 1f - Mathf.Clamp01(nearest.Distance / profile.maxDistance);
                float curved = profile.compactionCurve.Evaluate(proximity);
                offsetTarget = Mathf.Lerp(0f, profile.maxCompaction, curved);
            }

            // --- Smooth ---
            _currentOffset = Smooth(_currentOffset, offsetTarget,
                                    profile.attackSpeed, profile.releaseSpeed);

            // --- Apply ---
            Vector3 axis = compactionAxis.normalized;
            wallA.localPosition = _restA + axis * _currentOffset;
            wallB.localPosition = _restB - axis * _currentOffset;
        }

        //==================== PRIVATE =====================
        private static float Smooth(float current, float target, float attackSpeed, float releaseSpeed)
        {
            float speed = (target >= current) ? attackSpeed : releaseSpeed;
            if (speed <= 0f) return target;
            return Mathf.MoveTowards(current, target, speed * Time.deltaTime);
        }
    }
}
