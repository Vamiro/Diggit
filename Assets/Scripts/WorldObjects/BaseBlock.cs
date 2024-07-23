using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WorldObjects
{
    [RequireComponent(typeof(ParticleSystem), typeof(AudioSource))]
    public class BaseBlock : MonoBehaviour, IBlock
    {
        public GameObject GetInstance => gameObject;

        private ParticleSystem _particleSystem;
        private AudioSource _audioSource;

        private void Start()
        {
            // var mat = GetComponent<MeshRenderer>();
            // mat.material.mainTextureOffset = new Vector2(Random.Range(0f, 10f), Random.Range(0f, 10f));
            _particleSystem = GetComponent<ParticleSystem>();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.pitch = Random.Range(0.8f, 1.2f);
        }

        public void Destroy()
        {
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            _audioSource.Play();
            _particleSystem.Play();
            
        }

        private void OnParticleSystemStopped()
        {
            Destroy(gameObject);
        }
    }
}