using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ArsonistMod.Content.Controllers
{
    internal class TextVibration : MonoBehaviour
    {
        public float speed = 0.5f;
        public float intensity = 0.1f;
        public bool vibrating = false;
        public float stopwatch;
        public Vector3 originalPosition;

        public void Start() 
        {
            stopwatch = 0f;
            originalPosition = transform.localPosition;
        }

        public void Update() 
        {
            if (vibrating)
            {
                stopwatch += Time.deltaTime;
                transform.localPosition = originalPosition + (intensity * new Vector3(
                    Mathf.PerlinNoise(speed * Time.time, 1),
                    Mathf.PerlinNoise(speed * Time.time, 2),
                    Mathf.PerlinNoise(speed * Time.time, 3)));
            }
            else 
            {
                stopwatch = 0f;
                transform.localPosition = originalPosition;
            }
        }
    }
}
