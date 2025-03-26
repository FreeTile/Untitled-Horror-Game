using System.Collections;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class E2S1 : EventHandler
{
    public GameObject lightBulb;

    //private string E2S1BulbBreakSound = "event:/BulbSound";

    public override IEnumerator Event()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Screamer");
        GameManager.Instance.TriggerJumpScare();

        this.transform.parent.gameObject.SetActive(false);

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
