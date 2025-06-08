using UnityEngine;
using UnityEngine.AI;

public class MazeTester : MonoBehaviour
{
    public Maze level;
    public NavMeshAgent agent;
    public Vector3 startingPos;
    public Vector3 goalPos;
    public bool testingInProgress = false;
    public Vector3 destination;

    public void Start()
    {
        //gameObject.SetActive(false);
        //agent.enabled = false;
    }

    public void Update()
    {
        if (testingInProgress) return;
        if (level == null)
        {
            level = FindAnyObjectByType<Maze>();
        }
        if (level != null && level.startingCell != null && level.endingCell != null)
        {
            gameObject.SetActive(true);
            startingPos = level.startingCell.transform.position;
            goalPos = level.endingCell.transform.position;
            transform.position = startingPos;
            //agent.enabled = true;
            //agent.SetDestination(goalPos);
            //destination = agent.destination;
            testingInProgress = true;

        }
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(goalPos);
        }
    }




}
