using System.Collections;
using Photon.Pun;
using UnityEngine;

public class SnowBall : MonoBehaviourPun
{
    public static SnowBall Spawn(Vector3 position)
    {
        var obj = PhotonNetwork.Instantiate("SnowBall", position, Quaternion.identity);

        SnowBall snowBall = obj.GetComponent<SnowBall>();

        return snowBall;
    }
    
    /******************************************************************************************************************/
    /******************************************************************************************************************/

    public void Fire(Vector3 direction, float speed, float duration = 3f)
    {
        if (_fireCoroutine != null)
            StopCoroutine(_fireCoroutine);

        _fireCoroutine = StartCoroutine(FireCoroutine(direction, speed, duration));
    }

    private Coroutine _fireCoroutine;
    private IEnumerator FireCoroutine(Vector3 direction, float speed, float duration)
    {
        float elapsed = 0f;

        while (elapsed <= duration)
        {
            elapsed += Time.deltaTime;

            transform.position =
                Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

            yield return null;
        }

        Kill();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine == false) return;

        if (other.CompareTag("Wall"))
        {
            Kill();
            return;
        }

        if (other.CompareTag("Player") == false)
            return;

        if (other.TryGetComponent(out PhotonView view) == false)
            return;

        if (view.IsMine)
            return;
        
        view.RPC("GetHit", RpcTarget.All);
    }

    private void Kill()
    {
        if (_fireCoroutine != null)
            StopCoroutine(_fireCoroutine);
        
        PhotonNetwork.Destroy(gameObject);
    }
}