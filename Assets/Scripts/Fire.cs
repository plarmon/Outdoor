using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float fireTime;
    [SerializeField]
    private bool loops = true;
    [SerializeField]
    private bool startingOff;
    [SerializeField]
    private ParticleSystem fireParticles;
    [SerializeField]
    private ParticleSystem smokeParticles;
    [SerializeField]
    private AudioSource fireSoundSource;

    private BoxCollider boxCollider;
    private bool isInFire;
    private Door doorRef;

    private void Start() {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        if(loops) {
            StartCoroutine(FireLoop());
        }
    }

    private void Update() {
        if(isInFire) {
            if(doorRef != null) {
                if(doorRef.TakeDamage(damage * Time.deltaTime)) {
                    isInFire = false;
                    fireSoundSource.Stop();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Door")) {
            doorRef = other.gameObject.GetComponent<Door>();
            isInFire = true;
            fireSoundSource.Play();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Door")) {
            isInFire = false;
            fireSoundSource.Stop();
        }
    }

    private IEnumerator FireLoop() {
        if(startingOff) {
            fireParticles.Stop();
            smokeParticles.Stop();
            boxCollider.enabled = false;
            yield return new WaitForSeconds(fireTime);
            fireParticles.Play();
            smokeParticles.Play();
            boxCollider.enabled = true;
        }

        while(true) {
            yield return new WaitForSeconds(fireTime);
            fireParticles.Stop();
            smokeParticles.Stop();
            boxCollider.enabled = false;
            yield return new WaitForSeconds(fireTime);
            fireParticles.Play();
            smokeParticles.Play();
            boxCollider.enabled = true;
        }
    }
}
