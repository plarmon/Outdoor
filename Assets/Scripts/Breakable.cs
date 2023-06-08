using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private LevelManager lm;
    [SerializeField]
    private ObjectType objectType;
    public ObjectSize objectSize;
    [SerializeField]
    private GameObject brokenPrefab;
    [SerializeField]
    private LineRenderer cord;
    [SerializeField]
    private GameObject anchor;
    [SerializeField]
    private Transform cordObjectAttachPoint;

    [SerializeField]
    private GameObject sparkleEffectPrefab;
    private GameObject sparkleInstance;

    private enum ObjectType {
        NONE,
        VASE,
        CHANDELIER
    }

    public enum ObjectSize {
        SMALL,
        MEDIUM,
        BIG
    }

    private void Start() {
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        sparkleInstance = Instantiate(sparkleEffectPrefab, transform.position, transform.rotation);
    }

    private void Update() {
        if(cord != null) {
            cord.SetPosition(0, anchor.transform.position);
            cord.SetPosition(1, cordObjectAttachPoint.position);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Floor")) {
            lm.DecreaseScore(objectSize);
            lm.AlertTheGallery();
            switch(objectType) {
                case(ObjectType.VASE):
                    AudioManager.Instance.PlaySFX("Vase");
                    break;
                case(ObjectType.CHANDELIER):
                    AudioManager.Instance.PlaySFX("Chandelier");
                    break;
                default:
                    Debug.Log("Object Type not defined");
                    break;
            }

            if(brokenPrefab != null) {
                GameObject brokenObject = Instantiate(brokenPrefab, transform.position, transform.rotation);
                /* if(objectType == ObjectType.VASE) {
                    brokenObject.transform.Translate(new Vector3(0, -0.5f, 0), Space.World);
                } */
                brokenObject.transform.Rotate(new Vector3(-90,0,0), Space.Self);
            } else {
                Debug.Log("Breakable: No Broken Prefab defined");
            }

            Destroy(sparkleInstance);
            Destroy(gameObject);
        }
    }
}
