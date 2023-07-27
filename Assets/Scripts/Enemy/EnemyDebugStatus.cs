using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TMPro.TextMeshPro;

public class EnemyDebugStatus : MonoBehaviour
{
   private EnemyBehaviour _enemyBehaviour;
   private TextMeshPro _textMesh;
   
   private void Start()
   {
      _enemyBehaviour = GetComponentInParent<EnemyBehaviour>();
      _textMesh = GetComponentInChildren<TextMeshPro>();
   }
   
   private void Update()
   {
      _textMesh.text = _enemyBehaviour.enemyStatus;
   }
}
