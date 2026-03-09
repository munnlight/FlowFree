// using UnityEngine;
// using System.Collections.Generic;

// public class PathDrawer : MonoBehaviour
// {
//     public LineRenderer line;
//     private List<Vector3> points = new List<Vector3>();

//     void Update()
//     {
//         if (Input.GetMouseButton(0))
//         {
//             Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//             pos.z = 0;

//             if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], pos) > 0.5f)
//             {
//                 points.Add(pos);
//                 line.positionCount = points.Count;
//                 line.SetPositions(points.ToArray());
//             }
//         }
//     }
// }