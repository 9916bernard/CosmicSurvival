//using System.Collections.Generic;
//using UnityEngine;

//public static class BaseStationExtensions
//{
//    public static Vector3[] AddBaseStationPosition(this Vector3[] positions, int numberOfNewPositions, float spacing)
//    {
//        List<Vector3> positionList = new List<Vector3>(positions);
//        int currentCount = positionList.Count;

//        // Start the grid expansion based on the number of existing positions
//        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(currentCount));

//        for (int i = 0; i < numberOfNewPositions; i++)
//        {
//            int nextIndex = currentCount + i;

//            int x = nextIndex % (gridSize + 1);
//            int y = nextIndex / (gridSize + 1);

//            Vector3 newPosition = new Vector3(x * spacing, y * spacing, positions[0].z);

//            // Adjust based on the initial center (the average of the current grid's center)
//            Vector3 centerOffset = new Vector3(15f, 15f, 2f); // Adjust the center based on where the grid should be centered

//            newPosition += centerOffset;

//            positionList.Add(newPosition);

//            // If we've filled out the current grid size, increase it
//            if (x == gridSize && y == gridSize)
//            {
//                gridSize++;
//            }
//        }

//        return positionList.ToArray();
//    }
//}
