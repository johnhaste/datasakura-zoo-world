using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class BoxColliderGizmoDrawer : MonoBehaviour
{
    public GizmoColor gizmoColor = GizmoColor.Green; // Default color

    void OnDrawGizmos()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            // Select the color based on the enum selection
            switch (gizmoColor)
            {
                case GizmoColor.Red:
                    Gizmos.color = Color.red;
                    break;
                case GizmoColor.Green:
                    Gizmos.color = Color.green;
                    break;
                case GizmoColor.Blue:
                    Gizmos.color = Color.blue;
                    break;
                case GizmoColor.Yellow:
                    Gizmos.color = Color.yellow;
                    break;
                case GizmoColor.Cyan:
                    Gizmos.color = Color.cyan;
                    break;
                case GizmoColor.Magenta:
                    Gizmos.color = Color.magenta;
                    break;
            }

            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}

public enum GizmoColor
{
    Red,
    Green,
    Blue,
    Yellow,
    Cyan,
    Magenta
}

