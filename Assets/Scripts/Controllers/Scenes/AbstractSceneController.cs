using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using SO;
using Sounds;
using Types;
using AudioType = Types.AudioType;

namespace Controllers.Scenes
{
    public abstract class AbstractSceneController : MonoBehaviour
    {
        [SerializeField] 
        private SoundsController _soundsController;
        [SerializeField]
        private SceneSounds _sceneSounds;

        private MusicController _musicController;

        private void OnEnable()
        {
            _musicController = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicController>();
            
            _sceneSounds.SetAudioClip();
            
            Initialize();
            Subscribe();
            OnSceneEnable();
        }

        private void Start()
        {
            PlayMusic();
            OnSceneStart();
        }

        private void OnDisable()
        {
            Unsubscribe();
            OnSceneDisable();
        }

        protected abstract void OnSceneEnable();
        protected abstract void OnSceneStart();
        protected abstract void OnSceneDisable();
        protected abstract void Initialize();
        protected abstract void Subscribe();
        protected abstract void Unsubscribe();

        protected void LoadScene(SceneType sceneName)
        {
            SetClickClip();
            
            StartCoroutine(DelayLoadScene(sceneName.ToString()));
        }

        protected void SetClickClip()
        {
            PlaySound(AudioType.ClickClip);
        }

        protected AudioClip GetAudioClip(string clipName)
        {
            return _sceneSounds.GetAudioClipByName(clipName);
        }

        protected void PlaySound(AudioType audioType)
        {
            AudioClip clip = GetAudioClip(audioType.ToString());
            
           _soundsController.TryPlaySound(clip);
        }

        protected void PlayMusic()
        {
            AudioClip clip = GetAudioClip(AudioType.MenuClip.ToString());
            
            _musicController.TryPlayMusic(clip);
        }

        private IEnumerator DelayLoadScene(string sceneName)
        {
            yield return new WaitForSecondsRealtime(0.3f);

            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }

            SceneManager.LoadScene(sceneName);
        }
    }
}