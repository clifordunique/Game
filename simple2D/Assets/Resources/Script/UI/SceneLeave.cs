using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLeave : MonoBehaviour
{
    [SerializeField]
    bool Top, Bottom, Left, Right;
    [SerializeField]
    float RayLenth = 0.5f;
    [SerializeField]
    LayerMask collisionMask;

    private string prev_name;
    Bounds bounds;
    float HorLen, VerLen;
    bool loaded;

    // Start is called before the first frame update
    void Start()
    {
        prev_name = SceneManager.GetActiveScene().name;
        DontDestroyOnLoad(this);
        SceneManager.activeSceneChanged += LoadScene;
        SceneManager.sceneLoaded += LoadDone;
        bounds = GetComponent<Collider2D>().bounds;
        if (Top || Bottom) VerLen = (bounds.max.y - bounds.min.y);
        if (Left || Right) HorLen = (bounds.max.x - bounds.min.x);
        collisionMask |= (1 << LayerMask.NameToLayer("Player_w"));
        collisionMask |= (1 << LayerMask.NameToLayer("Player_b"));
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Top) VerticalRaycast(1);
        if (Bottom) VerticalRaycast(-1);
        if (Left) HorizontalRaycast(-1);
        if (Right) HorizontalRaycast(1);
    }

    void LoadDone(Scene scene, LoadSceneMode loadSceneMode)
    {
        loaded = true;
    }

    protected IEnumerator MovePlayerOnActive()
    {
        yield return new WaitUntil(() => { return loaded; });
        Transform player_trans = GameObject.Find("player").transform;
        Transform spawner = GameObject.Find("From_" + prev_name).transform;
        Player player = player_trans.GetComponent<Player>();
        player_trans.position = spawner.position;
        //yield return new WaitUntil(() => { return player.state.isGround; });
        /*Time.timeScale = 0.3f;
        Debug.Log("is ground");
        if (spawner.childCount != 0)
        {
            Vector2 toward = spawner.GetChild(0).localPosition;
            
            player.state.canControll = false;
            int tmp_layer = player.gameObject.layer;
            player.gameObject.layer = 8;

            for(int i = 0; i < 150; i++)
            {
                player.velocity = toward;
                yield return null;
            }
            player.state.canControll = true;
            player.gameObject.layer = tmp_layer;
        }
        Time.timeScale = 1f;*/
        Destroy(gameObject);
    }
    public void LoadScene(Scene prev, Scene next)
    {
        if (this == null) return;
        //GameObject.Find("player").transform.position = GameObject.Find("From_" + prev_name).transform.position;
        StartCoroutine(MovePlayerOnActive());
        
    }
    void VerticalRaycast(int Dir)
    {
        //from the middle bottom or top send raycast
        //Vector2 startFrom = (Vector2)bounds.center + Dir * new Vector2(0, VerLen / 2);
        Vector2 startFrom = Dir == -1 ? (Vector2)bounds.min : new Vector2(bounds.min.x, bounds.max.y);
        for (int i = 0; startFrom.x < bounds.max.x; i++)
        {
            RaycastHit2D hit;
            Debug.DrawRay(startFrom, Vector2.up * Dir * RayLenth, Color.green);
            
            if (hit = Physics2D.Raycast(startFrom, Vector2.up * Dir, RayLenth, collisionMask))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    SceneManager.LoadScene(this.name.Substring(3));
                }
                break;
            }
            startFrom += new Vector2(0.25f, 0);
        }
    }
    void HorizontalRaycast(int Dir)
    {
        RaycastHit2D hit;
        //Vector2 startFrom = new Vector2(bounds.center.x, bounds.min.y) + Dir * new Vector2(HorLen / 2, 0);
        Vector2 startFrom = Dir == -1 ? (Vector2)bounds.min : new Vector2(bounds.max.x, bounds.min.y);
        for (int i = 0; startFrom.y < bounds.max.y; i++)
        {   
            Debug.DrawRay(startFrom, Vector2.right * Dir * RayLenth, Color.red);
            if (hit = Physics2D.Raycast(startFrom, Vector2.right * Dir, RayLenth, collisionMask))
            {
                if(hit.collider.gameObject.tag=="Player" && hit.collider.gameObject.GetComponent<Player>().forward * Dir == -1)
                {
                    SceneManager.LoadScene(this.name.Substring(3));
                }
                break;
            }
            startFrom += new Vector2(0, 1.5f);
        }
    }
    private void OnDrawGizmos()
    {
        GameObject[] tar = GameObject.FindGameObjectsWithTag("SceneChange");
        foreach(GameObject a in tar)
        {
            
            if (a.name[0] == 'F')
            {
                Bounds b = a.GetComponent<SpriteRenderer>().bounds;
                Gizmos.color = new Color(1, 0, 0, 0.5f);
                Gizmos.DrawCube(b.center, b.size);
                if (a.transform.childCount != 0)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(b.center, a.transform.GetChild(0).position);
                }
            }
            if (a.name[0] == 'T')
            {
                
                Bounds b = a.GetComponent<Collider2D>().bounds;
                Gizmos.color = new Color(0, 0, 1, 0.5f);
                Gizmos.DrawCube(b.center, b.size);
            }
            
        }
    }
}
