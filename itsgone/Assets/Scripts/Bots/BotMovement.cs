using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BotMovement : MonoBehaviour
{
    [Flags]
    public enum DamageType
    {
        Near = 1,
        Distant = 2
    }
    [Header("Base characteristics", order = 1)]
    public float radiusTrigger = 10;
    public int live = 15;
    public GameObject[] wayPoints;
    [Space]
    [Header("Damage settings", order = 1)]
    public DamageType damageType;
    [Header("Near damage settings", order = 2)]
    public BotBullet bulletNear;
    [Header("Distance damage settings", order = 2)]
    public BotBullet bulletDistance;
    public float distanceRadiusDamage = 2f;
    public GameObject shootStartPosition;

    private NavMeshAgent agent;
    private PlayerScript player;
    private Vector3[] points;
    private bool isActivate = false, isBlocked = false;
    private int currentIndex = 0;
    private NavMeshPath path;

    void Start()
    {
        path = new NavMeshPath();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        agent = GetComponent<NavMeshAgent>();
        points = new Vector3[wayPoints.Length + 1];
        points[0] = transform.position;
        for (int i = 0; i < wayPoints.Length; i++)
        {
            points[i + 1] = wayPoints[i].transform.position;
        }
        if (wayPoints.Length > 0)
        {
            agent.destination = points[1];
        }
    }

    private void Update()
    {
        if (isActivate)
        {
            if(Vector3.Distance(player.transform.position, transform.position) > 1.5f)
            {
                agent.destination = player.transform.position;
            } else
            {
                agent.destination = transform.position;
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            }
            if(agent.path.status != NavMeshPathStatus.PathComplete)
            {
                if (!isBlocked) StartCoroutine(WaitPlayer());
                isBlocked = true;
            }
            else
            {
                
            }
        }
        else if (Vector3.Distance(player.transform.position, transform.position) <= radiusTrigger)
        {
            agent.CalculatePath(player.transform.position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                isActivate = true;
            }
        }
        else if (agent.remainingDistance < 1)
        {
            currentIndex = (currentIndex + 1) % points.Length;
            agent.destination = points[currentIndex];
        }
    }

    private void OnDrawGizmos()
    {
        NavMeshPath path = new NavMeshPath();
        Gizmos.color = new Color(1, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, radiusTrigger);
        Gizmos.color = Color.blue;
        if (wayPoints.Length > 0)
        {
            var prevPos = transform.position;
            foreach (var point in wayPoints)
            {
                if (point == null) continue;
                NavMesh.CalculatePath(prevPos, point.transform.position, NavMesh.AllAreas, path);
                foreach (var p in path.corners)
                {
                    Gizmos.DrawLine(prevPos, p);
                    prevPos = p;
                }
            }
        }
    }

    private IEnumerator WaitPlayer()
    {
        yield return new WaitForSeconds(3);
        agent.CalculatePath(player.transform.position, path);
        if(path.status != NavMeshPathStatus.PathComplete)
        {
            isActivate = false;
            agent.destination = points[currentIndex % points.Length];
        }
        isBlocked = false;
      
    }
}
