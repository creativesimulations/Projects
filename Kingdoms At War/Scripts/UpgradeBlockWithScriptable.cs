using UnityEngine;
using Core;
using Scriptables;

public class UpgradeBlockWithScriptable : MonoBehaviour
{
    [Header("Add ProjectileScriptableObject here.")]
    public BlockUpgradesScriptableObject blockUpgradeType;
    public int typeOfUpgrade;
    public float amountToChange;
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private AudioClip explosionSound;

    void Awake()
    {
        typeOfUpgrade = (int)blockUpgradeType.upgradeType;
        amountToChange = blockUpgradeType.increaseAmountBy;
    }

    void Start()
    {
        if(!blockUpgradeType)
        {
            Debug.Log("Did you forget to add the ProjectileScriptableObject to this upgarde block?");
        }
    }

    public void OnHit(int team)
    {
        EventManager.onUpgrade(typeOfUpgrade, amountToChange, team);
        ParticleSystem explosion = Instantiate(explosionParticle, transform.position, transform.rotation);
        Vector3 blockSize = transform.GetComponent<Renderer>().bounds.size;
        explosion.transform.localScale = new Vector3(blockSize.x, blockSize.y, blockSize.z);
        explosion.gameObject.transform.SetParent(null);
        Destroy(gameObject);
    }

}
