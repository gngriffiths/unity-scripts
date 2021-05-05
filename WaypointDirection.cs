using UnityEngine;

namespace app.item.junction
{
    public class WaypointDirection : MonoBehaviour
    {
        public enum Direction
        {
            none,
            Forward,
            Left,
            Right,
            Backward
        }

        public Direction Location(GameObject playerGameObject)
        {
            Direction waypointDirection = Direction.none;

            Vector3 targetDir = transform.position - playerGameObject.transform.position;
            targetDir = targetDir.normalized;

            float dot = Vector3.Dot(targetDir, playerGameObject.transform.forward);
            dot = Mathf.Clamp(dot, -1f, 1f);        // Clamping dot because number can be slightly outside -1 to 1 and breaks the angle
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;      // The angle is from 0 to 180. Vector3.Cross is used below to determine positive and negative (left/right)

            Vector3 normalizedDirection = transform.position - playerGameObject.transform.position;
            float direction = Vector3.Cross(playerGameObject.transform.forward, normalizedDirection).y;
 
            //Debug.Log("Angle: " + angle + " Dir: " + direction);

            if (angle <= 45)
            {
                //Debug.Log("Front");
                waypointDirection = Direction.Forward;
            }

            if (angle >= 135)
            {
                //Debug.Log("Behind");
                waypointDirection = Direction.Backward;
            }

            if (angle > 45 && angle < 135)
            {
                if (direction >= 0)
                {
                    //Debug.Log("Right");
                    waypointDirection = Direction.Right;
                }
                else
                {
                    //Debug.Log("Left");
                    waypointDirection = Direction.Left;
                }
            }

            return waypointDirection;
        }

       
    }
}