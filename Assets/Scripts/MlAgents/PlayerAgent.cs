using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PlayerAgent : Agent
{
    [Header("Environment")]
    public Transform patrolAgent;
    public Transform hideZone;
    public LayerMask viewMask;

    [Header("Movement")]
    public float moveSpeed = 3f;

    private Vector3 startPosition;
    private bool isHiding = false;

    public override void Initialize()
    {
        startPosition = transform.position;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        isHiding = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 dirToPatrol = (patrolAgent.position - transform.position).normalized;
        float distToPatrol = Vector3.Distance(transform.position, patrolAgent.position);
        float distToHideZone = Vector3.Distance(transform.position, hideZone.position);

        // Observations
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(patrolAgent.localPosition);
        sensor.AddObservation(hideZone.localPosition);
        sensor.AddObservation(dirToPatrol);
        sensor.AddObservation(distToPatrol / 20f);  // normalize
        sensor.AddObservation(distToHideZone / 20f);
        sensor.AddObservation(isHiding ? 1f : 0f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        Vector3 move = new Vector3(moveX, 0f, moveZ).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;

        if (CanBeSeenByPatrol())
        {
            SetReward(-1f); // caught
            EndEpisode();
        }
        else if (isHiding)
        {
            SetReward(1f); // successful hide
            EndEpisode();
        }
        else
        {
            AddReward(-0.001f); // step penalty
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> ca = actionsOut.ContinuousActions;
        ca[0] = Input.GetAxis("Horizontal");
        ca[1] = Input.GetAxis("Vertical");
    }

    private bool CanBeSeenByPatrol()
    {
        Vector3 dirToPlayer = (transform.position - patrolAgent.position).normalized;
        float angle = Vector3.Angle(patrolAgent.forward, dirToPlayer);
        float distance = Vector3.Distance(patrolAgent.position, transform.position);

        if (angle < 45f && distance < 10f)
        {
            if (!Physics.Linecast(patrolAgent.position, transform.position, viewMask))
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HideZone"))
        {
            isHiding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HideZone"))
        {
            isHiding = false;
        }
    }
}
