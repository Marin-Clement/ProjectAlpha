using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class Projectile_Data : ScriptableObject
{
    // Projectile Variables
    
    [Header("Projectile Variables")]
    public float speed;
    public float damage;
    public float lifeTime;
    public int pierceCount;
    
    // Status Variables
    [HideInInspector] public bool isSticky;
    [HideInInspector] public bool isBouncy;
    [HideInInspector] public bool isPoisonous;
    [HideInInspector] public bool isFreezing;
    [HideInInspector] public bool isBurning;
    [HideInInspector] public bool isElectrifying;
    [HideInInspector] public bool isStunning;

    // Homing Variables`
    [HideInInspector]
    public float turnSpeed;
    
    // Bouncy Variables
    [HideInInspector]
    public int bounces;

    // Explosion Variables
    [Header("Projectile Behaviour")]
    public bool isHoming;
    public bool isExplosive;
    public bool statusEffect;
    [HideInInspector]
    public float timeToExplode;
    [HideInInspector]
    public float explosionRadius;
    [HideInInspector]
    public float explosionForce;
    [HideInInspector]
    public float explosionDamage;
    [HideInInspector]
    public int numberOfProjectiles;
    [HideInInspector]
    public Projectile_Data childProjectile;
}

[CustomEditor(typeof(Projectile_Data))]
public class Projectile_Data_Editor : Editor
{
    private Projectile_Data _projectileData;
    private void OnEnable()
    {
        _projectileData = (Projectile_Data) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (_projectileData.isHoming)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Homing Variables");
            _projectileData.turnSpeed = EditorGUILayout.FloatField("Turn Speed", _projectileData.turnSpeed);
        }
        if (_projectileData.isExplosive)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Explosion Variables");
            _projectileData.timeToExplode = EditorGUILayout.FloatField("Time To Explode", _projectileData.timeToExplode);
            _projectileData.explosionRadius = EditorGUILayout.FloatField("Explosion Radius", _projectileData.explosionRadius);
            _projectileData.explosionForce = EditorGUILayout.FloatField("Explosion Force", _projectileData.explosionForce);
            _projectileData.explosionDamage = EditorGUILayout.FloatField("Explosion Damage", _projectileData.explosionDamage);
            _projectileData.numberOfProjectiles = EditorGUILayout.IntField("Number Of Projectiles", _projectileData.numberOfProjectiles);
            _projectileData.childProjectile = (Projectile_Data) EditorGUILayout.ObjectField("Child Projectile", _projectileData.childProjectile, typeof(Projectile_Data), false);
        }
        if (_projectileData.statusEffect)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Status Variables");
            _projectileData.isSticky = EditorGUILayout.Toggle("Sticky", _projectileData.isSticky);
            _projectileData.isBouncy = EditorGUILayout.Toggle("Bouncy", _projectileData.isBouncy);
            _projectileData.isPoisonous = EditorGUILayout.Toggle("Poisonous", _projectileData.isPoisonous);
            _projectileData.isFreezing = EditorGUILayout.Toggle("Freezing", _projectileData.isFreezing);
            _projectileData.isBurning = EditorGUILayout.Toggle("Burning", _projectileData.isBurning);
            _projectileData.isElectrifying = EditorGUILayout.Toggle("Electrifying", _projectileData.isElectrifying);
            _projectileData.isStunning = EditorGUILayout.Toggle("Stunning", _projectileData.isStunning);
        }
        if (_projectileData.isBouncy)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Bouncy Variables");
            _projectileData.bounces = EditorGUILayout.IntField("Bounces", _projectileData.bounces);
        }
    }
}
