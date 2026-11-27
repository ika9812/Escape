using System;
using UnityEngine;
using UnityEngine.Audio;
namespace Assets.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [SerializeField] AudioSource BGMAudioSource;//BGM再生用

        [SerializeField] AudioSource SEAudioSource;//SE再生用

        [SerializeField] AudioMixer AudioMixer;

        [SerializeField, Header("現在の音量データが入っているScriptableObject")] private VolumeDeta VolumeDeta;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            SetMasterVolume(VolumeDeta.MasterVolume);
            SetBGMVolume(VolumeDeta.BGMVolume);
            SetSEVolume(VolumeDeta.SEVolume);
        }

        //-------------------------------音楽再生---------------------------------//

        /// <summary>
        /// SEの再生
        /// </summary>
        /// <param name="clip">再生する曲</param>
        public void PlaySE(AudioClip clip)
        {
            if (clip == null) return;

            SEAudioSource.PlayOneShot(clip);
        }

        /// <summary>
        /// BGMの再生
        /// </summary>
        /// <param name="clip">再生する曲</param>
        /// <param name="awake">ゲーム開始時にならすか</param>
        /// <param name="loop">ループ再生するか</param>
        public void PlayBGM(AudioClip clip, bool awake, bool loop)
        {
            //再生する曲が設定されていない場合抜ける
            if (clip == null) return;
            //同じ曲が再生中なら抜ける
            if (BGMAudioSource.clip == clip && BGMAudioSource.isPlaying) return;

            BGMAudioSource.clip = clip;
            BGMAudioSource.playOnAwake = awake;//trueなら開始時にならす
            BGMAudioSource.loop = loop;//trueならループ再生する
            BGMAudioSource.Play();
        }
        /// <summary>
        /// BGMの停止
        /// </summary>
        public void StopBGM()
        {
            BGMAudioSource.Stop();
        }


        //--------------------------------音量設定---------------------------------//

        /// <summary>
        /// マスター音量の設定
        /// </summary>
        /// <param name="volume">音量</param>
        public void SetMasterVolume(float volume)
        {
            volume = Mathf.Clamp(volume, 0.001f, 1f);

            VolumeDeta.MasterVolume = volume;

            AudioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        }
        /// <summary>
        /// BGM音量の設定
        /// </summary>
        /// <param name="volume">音量</param>
        public void SetBGMVolume(float volume)
        {
            volume = Mathf.Clamp(volume, 0.001f, 1f);

            VolumeDeta.BGMVolume = volume;

            AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        }
        /// <summary>
        /// SE音量の設定
        /// </summary>
        /// <param name="volume">音量</param>
        public void SetSEVolume(float volume)
        {
            volume = Mathf.Clamp(volume, 0.001f, 1f);

            VolumeDeta.SEVolume = volume;

            AudioMixer.SetFloat("SE", Mathf.Log10(volume) * 20);
        }
        /// <summary>
        /// 現在のマスター音量を取得
        /// </summary>
        /// <returns></returns>
        public float GetMasterVolume()
        {
            return VolumeDeta.MasterVolume;
        }
        /// <summary>
        /// 現在のBGM音量を取得
        /// </summary>
        /// <returns></returns>
        public float GetBGMVolume()
        {
            return VolumeDeta.BGMVolume;
        }
        /// <summary>
        /// 現在のSE音量を取得
        /// </summary>
        /// <returns></returns>
        public float GetSEVolume()
        {
            return VolumeDeta.SEVolume;
        }
    }
}
