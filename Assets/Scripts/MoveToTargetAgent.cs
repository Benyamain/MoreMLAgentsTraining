using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToTargetAgent : Agent
{
    [SerializeField] private Transform target;

    public override void OnEpisodeBegin() {
        transform.position = new Vector3(Random.Range(-3.5f, -1.5f), Random.Range(-3.5f, 3.5f));
        target.position = new Vector3(Random.Range(-1.5f, -3.5f), Random.Range(-3.5f, 3.5f));
    }
    
    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation((Vector2)transform.position);
        sensor.AddObservation((Vector2)target.position);
    }
    
    public override void OnActionReceived(ActionBuffers actions) {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float movementSpeed = 5f;

        // Restraint to the single environment and not others (we will have multiple environments)
        transform.localPosition += new Vector3(moveX, moveY) * Time.deltaTime * movementSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent(out Target target))
        {
            AddReward(10f);
            EndEpisode();
        } else if (other.TryGetComponent(out Wall wall)) {
            AddReward(-2f);
            EndEpisode();
        }
    }
}
