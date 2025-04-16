using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Image LoadingProgressBar;

    private static SceneTransition instance;
    private static bool shouldPlayOpeningAnimation = false;

    private Animator componentAnimator;
    private AsyncOperation loadingSceneOperation;

    [SerializeField] private static string musicBusPath = "bus:/Music";
    [SerializeField] private static string sfxBusPath = "bus:/SFX";

    public static void SwitchToScene(string sceneName)
    {
        Time.timeScale = 1f;

        Bus musicBus = RuntimeManager.GetBus(musicBusPath);
        musicBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        Bus sfxBus = RuntimeManager.GetBus(sfxBusPath);
        sfxBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);

        instance.componentAnimator.SetTrigger("sceneClosing");

        instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);

        instance.loadingSceneOperation.allowSceneActivation = false;

        instance.LoadingProgressBar.fillAmount = 0;

        Debug.Log("Switched to " + sceneName);
    }

    private void Start()
    {
        instance = this;

        componentAnimator = GetComponent<Animator>();

        if (shouldPlayOpeningAnimation)
        {
            componentAnimator.SetTrigger("sceneOpening");
            instance.LoadingProgressBar.fillAmount = 1;

            shouldPlayOpeningAnimation = false;
        }
    }

    private void Update()
    {
        if (loadingSceneOperation != null)
        {
            LoadingProgressBar.fillAmount = Mathf.Lerp(LoadingProgressBar.fillAmount, loadingSceneOperation.progress,
                Time.deltaTime * 5);
        }
    }

    public void OnAnimationOver()
    {
        shouldPlayOpeningAnimation = true;

        loadingSceneOperation.allowSceneActivation = true;

        Debug.Log("Animation Over");
    }
    
}