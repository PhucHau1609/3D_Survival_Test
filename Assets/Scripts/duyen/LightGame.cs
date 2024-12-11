using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGame : MonoBehaviour
{
    public float timeDayNight = 1f;
    public Light light;
    public Gradient gradient;

    public float rotationSpeed;

    void Start()
    {
        rotationSpeed = 360f / (timeDayNight * 60f);
    }

    void Update()
    {
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        if(light != null && gradient != null)
        {
            float time = Mathf.PingPong(Time.time /(timeDayNight * 30f), 1f);
            light.color = gradient.Evaluate(time);
        }
    }
}
