using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2TV : EventHandler
{
    public GameObject TV;
    public int minSanityLoss = 15;
    public int maxSanityLoss = 25;
    public float duration = 0.5f;

    public override IEnumerator Event()
    {
        TV.SetActive(true);
        GameManager.Instance.sanityManager.CheckFearLevel(minSanityLoss, maxSanityLoss, duration);
        Destroy(TV, 2f);
        return base.Event();
    }

}
