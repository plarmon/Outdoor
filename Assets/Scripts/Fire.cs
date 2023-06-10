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
    private bool startingOff;
    [SerializeField]
    private ParticleSystem fireParticles;
    [SerializeField]
    private ParticleSystem smokeParticles;
    private BoxCollider boxCollider;
    private bool isInFire;
    private Door doorRef;

    private void Start() {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        StartCoroutine(FireLoop());
    }

    private void Update() {
        if(isInFire) {
            if(doorRef != null) {
                if(doorRef.TakeDamage(damage * Time.deltaTime)) {
                    isInFire = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Door")) {
            doorRef = other.gameObject.GetComponent<Door>();
            isInFire = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Door")) {
            isInFire = false;
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
