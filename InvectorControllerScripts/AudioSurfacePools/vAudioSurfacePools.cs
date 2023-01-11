using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace Invector
{
    public class vAudioSurfacePools : ScriptableObject
    {
        public GameObject audioSourceObject;
        public AudioMixerGroup audioMixerGroup;                 // The AudioSource that will play the clips.   
        public AudioClip[] audioClips;

        // Added for Object Pooling
        private ObjectPool<GameObject> _audioSourcePool;
 
        /// <summary>
        /// Create an instance of AudioSource for the AudioSource Pool
        /// </summary>
        /// <returns></returns>        
        private GameObject AudioSourceCreatePoolItem()
        {
            GameObject audioSourceGo = Instantiate<GameObject>(audioSourceObject);
            audioSourceGo.name = string.Format("AudioSourcePool({0})", Time.fixedTime);
            return audioSourceGo;
        }

        /// <summary>
        /// Set the instance to active on taking from the pool
        /// </summary>
        /// <param name="audioSource"></param>
        private void AudioSourceOnTakeFromPool(GameObject audioSourceGo)
        {
            audioSourceGo.SetActive(true);
        }

        /// <summary>
        /// Reset position and set inactive on removal from pool
        /// </summary>
        /// <param name="audioSource"></param>
        private void AudioSourceOnReturnToPool(GameObject audioSourceGo)
        {
            if (audioSourceGo.transform)
            {
                audioSourceGo.transform.position = Vector3.zero;
                audioSourceGo.transform.rotation = Quaternion.identity;
            }
            if (audioSourceGo)
            {
                audioSourceGo.SetActive(false);
            }
        }

        /// <summary>
        /// Destory AudioSource game object instance on destory pool
        /// </summary>
        /// <param name="audioSource"></param>
        private void AudioSourceOnDestroyPoolObject(GameObject audioSourceGo)
        {
            Destroy(audioSourceGo);
        }

        /// <summary>
        /// Async method to wait then return the object to the pool
        /// </summary>
        /// <param name="audioSource"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator AudioSourceReturnToPoolAsync(GameObject audioSourceGo, float delay)
        {
            yield return new WaitForSeconds(delay);
            _audioSourcePool.Release(audioSourceGo);
            yield break;
        }

        /// <summary>
        /// Initialise the Pools
        /// </summary>
        public void OnEnable()
        {
            _audioSourcePool = new ObjectPool<GameObject>(new Func<GameObject>(this.AudioSourceCreatePoolItem),
                new Action<GameObject>(AudioSourceOnTakeFromPool),
                new Action<GameObject>(this.AudioSourceOnReturnToPool),
                new Action<GameObject>(this.AudioSourceOnDestroyPoolObject), false, 10, 20);
        }

        /// <summary>
        /// Spawn Sound effect
        /// </summary>
        /// <param name="footStepObject">Step object surface info</param>      
        protected virtual void PlaySound(FootStepObject footStepObject)
        {
            // INVECTOR CODE...
            // ...

            // Get an object instance from the pool
            GameObject audioSourceGo = _audioSourcePool.Get();
            audioSourceGo.transform.position = footStepObject.sender.position;
            audioSourceGo.transform.rotation = Quaternion.identity;
            audioSourceGo.transform.SetParent(vObjectContainer.root, true);

            // Set the MixerGroup if it has been set
            if (audioMixerGroup != null)
            {
                audioSourceGo.GetComponent<AudioSource>().outputAudioMixerGroup = audioMixerGroup;
            }

            // INVECTOR CODE...
            // ...

            int index = 0;
            audioSourceGo.GetComponent<AudioSource>().PlayOneShot(audioClips[index], footStepObject.volume);
            CoroutineController.Start(AudioSourceReturnToPoolAsync(audioSourceGo, 3f));
        }

        /// <summary>
        /// Private class to allow use of Coroutines in ScriptableObjects
        /// </summary>
        private class CoroutineController : MonoBehaviour
        {

            static CoroutineController _singleton;
            static Dictionary<string, IEnumerator> _routines = new Dictionary<string, IEnumerator>(100);

            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
            static void InitializeType()
            {
                _singleton = new GameObject($"#{nameof(CoroutineController)}").AddComponent<CoroutineController>();
                DontDestroyOnLoad(_singleton);
            }

            public static Coroutine Start(IEnumerator routine) => _singleton.StartCoroutine(routine);
            public static Coroutine Start(IEnumerator routine, string id)
            {
                var coroutine = _singleton.StartCoroutine(routine);
                if (!_routines.ContainsKey(id)) _routines.Add(id, routine);
                else
                {
                    _singleton.StopCoroutine(_routines[id]);
                    _routines[id] = routine;
                }
                return coroutine;
            }
            public static void Stop(IEnumerator routine) => _singleton.StopCoroutine(routine);
            public static void Stop(string id)
            {
                if (_routines.TryGetValue(id, out var routine))
                {
                    _singleton.StopCoroutine(routine);
                    _routines.Remove(id);
                }
                else Debug.LogWarning($"coroutine '{id}' not found");
            }
            public static void StopAll() => _singleton.StopAllCoroutines();
        }
    }
}