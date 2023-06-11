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

    [SerializeField]
    private GameObject firePrefab;

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
        sparkleInstance.transform.parent = transform;
        sparkleInstance.transform.localScale = new Vector3(1, 1, 1);
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
                    if(firePrefab != null) {
                        GameObject fireInstance = Instantiate(firePrefab, transform.position, Quaternion.Euler(Vector3.up));
                        fireInstance.transform.position = new Vector3(fireInstance.transform.position.x, collision.gameObject.transform.position.y, fireInstance.transform.position.z);
                    } else {
                        Debug.Log("Breakable: Fire prefab not defined");
                    }
                    break;
                default:
                    Debug.Log("Object Type not defined");
                    break;
            }

            if(brokenPrefab != null) {
                GameObject brokenObject = Instantiate(brokenPrefab, transform.position, transform.rotation);
                brokenObject.transform.Rotate(new Vector3(-90,0,0), Space.Self);
                brokenObject.transform.localScale = transform.localScale / 100;
            } else {
                Debug.Log("Breakable: No Broken Prefab defined");
            }

            Destroy(sparkleInstance);
            Destroy(gameObject);
        }
    }
}
