using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PatrolAgent : Agent
{
    [Header("Environment")]
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    
    [Header("References")]
    public Transform player;
    public LayerMask viewMask;
    
    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Vision")]
    public float viewDistance = 10f;
    public float viewAngle = 90f;

    [Header("Detection")]
    public float catchDistance = 1.5f;
    public float detectionChanceInHide = 0.2f;

    [HideInInspector] public bool playerIsHiding;

    private Vector3 startPos;

    public override void Initialize()
    {
        startPos = transform.position;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = startPos;
        transform.rotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Normalize and add observations
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(player.localPosition);
        sensor.AddObservation(dirToPlayer);
        sensor.AddObservation(distanceToPlayer / viewDistance);
        sensor.AddObservation(playerIsHiding ? 1f : 0f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Movement from neural net
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        Vector3 move = new Vector3(moveX, 0f, moveZ).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;

        // Optional: rotate to face move direction
        if (move != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(move);

        // Add small step penalty
        AddReward(-0.001f);

        // If can see and catch player
        if (CanSeePlayer())
        {
            if (!playerIsHiding || Random.value < detectionChanceInHide)
            {
                if (Vector3.Distance(transform.position, player.position) < catchDistance)
                {
                    SetReward(1.0f);  // Successful catch
                    EndEpisode();
                }
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // For manual testing with WASD
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private bool CanSeePlayer()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle < viewAngle / 2f && Vector3.Distance(transform.position, player.position) <= viewDistance)
        {
            if (!Physics.Linecast(transform.position, player.position, viewMask))
            {
                return true;
            }
        }
        return false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(1f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }
    }
}
