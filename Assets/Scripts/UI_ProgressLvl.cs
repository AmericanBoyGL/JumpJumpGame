using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ProgressLvl : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform EndLvl;
    [SerializeField] Slider slider;

    float maxDistance;

    void Start()
    {
        maxDistance = getDistance();
    }

    void Update()
    {
        float dist = 1 - (getDistance() / maxDistance);
        SetProgress(dist);
    }

    float getDistance()
    {
        return Vector2.Distance(player.localPosition, EndLvl.position);
    }

    void SetProgress(float prog)
    {
        slider.value = prog;
    }
}
