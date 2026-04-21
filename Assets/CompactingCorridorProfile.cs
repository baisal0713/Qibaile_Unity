using UnityEngine;

namespace Ludocore
{
    /// <summary>Tuning profile for CompactingCorridorController — wall compaction driven by proximity.</summary>
    [CreateAssetMenu(menuName = "Ludocore/Compacting Corridor Profile")]
    public class CompactingCorridorProfile : ScriptableObject
    {
        //==================== INPUT =====================
        [Header("Input — Proximity")]
        [Tooltip("Maximum detection distance — proximity is normalized against this value")]
        [Min(0.01f)]
        public float maxDistance = 5f;

        //==================== WALL COMPACTION =====================
        [Header("Wall Compaction — Bound Interaction")]
        [Tooltip("Proximity→compaction remapping curve (X = 0 far‥1 close, Y = 0 rest‥1 full)")]
        public AnimationCurve compactionCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [Tooltip("Maximum inward offset from the wall rest position (units)")]
        [Min(0f)]
        public float maxCompaction = 1f;

        [Tooltip("How fast walls close when proximity increases (units/sec, 0 = instant)")]
        [Min(0f)]
        public float attackSpeed;

        [Tooltip("How fast walls return to rest when proximity decreases (units/sec, 0 = instant)")]
        [Min(0f)]
        public float releaseSpeed;
    }
}
