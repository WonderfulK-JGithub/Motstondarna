//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(LaserEnemy))]
//public class LaserEnemyEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        LaserEnemy enemy = (LaserEnemy)target;

//        if (GUILayout.Button("Toggle Lasers"))
//        {
//            if (enemy.lasersOn)
//            {
//                enemy.TurnOffLasers();
//            }
//            else
//            {
//                enemy.TurnOnLasers();
//            }
//        }
//    }
//}
