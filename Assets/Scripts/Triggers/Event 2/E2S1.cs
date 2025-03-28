using System.Collections;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class E2S1 : EventHandler
{
    public GameObject lightBulb;
    public int minSanityLoss = 50;
    public int maxSanityLoss = 80;
    //private string E2S1BulbBreakSound = "event:/BulbSound";

    public override IEnumerator Event()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Screamer");
        GameManager.Instance.TriggerJumpScare();
        //Decrease Sanity here

        this.transform.parent.gameObject.SetActive(false);
        int sanityLoss = Random.Range(minSanityLoss, maxSanityLoss);
        GameManager.Instance.sanityManager.TriggerScreamer();
        if (lightBulb != null)
        {
            lightBulb.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No link to the light bulb!");
        }

        //EventInstance bulbBreak = RuntimeManager.CreateInstance(E2S1BulbBreakSound);
        //bulbBreak.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        //bulbBreak.start();
        //bulbBreak.release();
    }
}