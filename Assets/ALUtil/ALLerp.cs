using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AL.ALUtil
{
    public static class ALLerp
    {
        public static float Lerp(float began, float end, float time)
        {
            return (1 - time) * began + (time * end);
        }

        public static Vector2 Lerp(Vector2 began, Vector2 end, float time)
        {
            return new Vector2(Lerp(began.x, end.x, time), Lerp(began.y, end.y, time));
        }

        public static Vector3 Lerp(Vector3 began, Vector3 end, float time)
        {
            return new Vector3(Lerp(began.x, end.x, time), Lerp(began.y, end.y, time), Lerp(began.z, end.z, time));
        }

        public static Color Lerp(Color began, Color end, float time)
        {
            return new Color(Lerp(began.r, end.r, time), Lerp(began.g, end.g, time), Lerp(began.b, end.b, time), Lerp(began.a, end.a, time));
        }
    }
}