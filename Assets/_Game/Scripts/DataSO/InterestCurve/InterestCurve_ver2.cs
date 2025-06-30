// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// [CreateAssetMenu(fileName = "ScriptableObjects/InterestCurve/Ver_2", menuName = "ScriptableObjects/InterestCurve/Ver_2")]
// public class InterestCurve_ver2 : InterestCurve, IHeatPoint
// {
//     public List<Vector2> StuckPoints = new List<Vector2>();
//     public override float GetPriorityCount(int currentProcess, int totalProcess)
//     {
//         for(int i = 0;i < StuckPoints.Count; i++)
//         {
//             if (Mathf.Approximately(currentProcess + 1, StuckPoints[i].x))
//                 return StuckPoints[i].y;
//         }
//         return 0;
//     }
//
//     public List<Vector2> GetListHeatPoint()
//     {
//         return StuckPoints;
//     }
//
//     public void SetListHeatPoint(List<Vector2> heatPoints)
//     {
//         StuckPoints = heatPoints;
//     }
//     
// }