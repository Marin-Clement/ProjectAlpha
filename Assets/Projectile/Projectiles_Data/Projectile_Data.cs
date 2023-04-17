using System.Collections;
using System.Collections.Generic;
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

    // Homing Variables`
    [HideInInspector]
    public float turnSpeed;

    // Explosion Variables
    [Header("Projectile Behaviour")]
    public bool isHoming;
    public bool isExplosive;
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
    }
}
