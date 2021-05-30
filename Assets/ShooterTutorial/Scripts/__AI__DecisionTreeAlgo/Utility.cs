using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static Vector3 DirectionFromAngle(float angleInDegrees)
    {// gdy x 1 to z 0 i na odwrot, do wyliczania znormalizowanego wektora kierunku, gdzie 0 stopni to chyba Forward
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
