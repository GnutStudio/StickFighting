using UnityEngine;

public class BigCoinCS : MonoBehaviour
{
    [SerializeField] private int minCoin = 150,maxCoin = 225;
    [SerializeField] private ParticleSystem par;
    [SerializeField] private Transform point;

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.transform.parent.GetComponent<Player>();
        if(player != null) {
            if(DataManager.Instance != null) {
                int random = Random.Range(minCoin,maxCoin);
                
                SoundManager.Instance.PlaySoundGoldCollect();
                var Parti =  par.Spawn();
                Parti.Play();
                Parti.transform.position = point.transform.position;

                DataManager.Instance.PlayerData.AddItem(new ItemStack(ItemID.COIN, random));
                SpawnerTextDame.Instance.Spawner(player.transform.position, $"+{random}");
                gameObject.SetActive(false);
            }
        }
    }
}
