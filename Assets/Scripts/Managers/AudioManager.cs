using UnityEngine;

namespace LemonadeStand.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public bool isMusicEnabled = true;
        public bool isSFXEnabled = true;
        
        [Range(0, 1)]
        public float musicVolume = 1.0f;
        [Range(0, 1)]
        public float sFXVolume = 1.0f;

        public AudioClip backgroundMusic;
        public AudioClip clickSound;
        public AudioClip coinsSound;
        public AudioClip customerChimeSound;
        public AudioClip greetingsSound;
        public AudioClip menuOpenSound;
        public AudioClip upgradeChimeSound;

        public AudioSource audioSource;
        
        [Header("Music Icons")] 
        public GameObject musicOffIcon;
        public GameObject musicOnIcon;
        public GameObject sfxOffIcon;
        public GameObject sfxOnIcon;
        
        private void Start()
        {
            PlayBackgroundMusic(backgroundMusic);
            ToggleMusicIcons();
        }

        private void PlayBackgroundMusic(AudioClip audioClip)
        {
            if (!isMusicEnabled || !audioClip || !audioSource)
            {
                return;
            }
            
            audioSource.Stop();
            audioSource.clip = audioClip;
            audioSource.volume = musicVolume;
            audioSource.loop = true;
            audioSource.Play();
        }

        private void UpdateMusic()
        {
            if (audioSource.isPlaying != isMusicEnabled)
            {
                if (isMusicEnabled)
                {
                    
                    PlayBackgroundMusic(backgroundMusic);
                }
                else
                {
                    audioSource.Stop();
                }
            }
        }

        public void ToggleMusic()
        {
            isMusicEnabled = !isMusicEnabled;
            UpdateMusic();
            ToggleMusicIcons();
        }
        
        public void ToggleSFX()
        {
            isSFXEnabled = !isSFXEnabled;
            ToggleMusicIcons();
        }

        private void ToggleMusicIcons()
        {
            if (isMusicEnabled)
            {
                musicOnIcon.gameObject.SetActive(true);
                musicOffIcon.gameObject.SetActive(false);
            }
            else
            {
                musicOnIcon.gameObject.SetActive(false);
                musicOffIcon.gameObject.SetActive(true);
            }

            if (isSFXEnabled)
            {
                sfxOnIcon.gameObject.SetActive(true);
                sfxOffIcon.gameObject.SetActive(false);
            }
            else
            {
                sfxOnIcon.gameObject.SetActive(false);
                sfxOffIcon.gameObject.SetActive(true);
            }
        }
    }
}