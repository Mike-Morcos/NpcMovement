// ----------------------------------------------------------------------------
// The NpcMovement script controls the movement of a non-player character
// (NPC) in a game. It allows the NPC to move within specified boundaries,
// with randomized durations for both movement and idle states.
// The script uses serialized fields to define parameters such as movement
// speed and distance boundaries. It clamps the NPC's position within the
// defined boundaries and provides a visual representation of the boundaries
// in the Unity editor. Overall, the script provides randomized and bounded
// movement behavior for an NPC in a game environment.
// ----------------------------------------------------------------------------

using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f; // The movement speed of the NPC.

    [SerializeField] float _minIdleTime = 1f; // The minimum duration of idle state.
    [SerializeField] float _maxIdleTime = 4f; // The maximum duration of idle state.

    [SerializeField] float _minMoveTime = 1f; // The minimum duration of movement state.
    [SerializeField] float _maxMoveTime = 3f; // The maximum duration of movement state.

    [SerializeField] float _maxDistanceX = 3f; // The maximum distance the NPC can move on the X-axis.
    [SerializeField] float _minDistanceX = -3f; // The minimum distance the NPC can move on the X-axis.

    [SerializeField] float _maxDistanceY = 3f; // The maximum distance the NPC can move on the Y-axis.
    [SerializeField] float _minDistanceY = -3f; // The minimum distance the NPC can move on the Y-axis.

    float _randomIdleTime; // The randomly generated duration for idle state.
    float _randomMovementTime; // The randomly generated duration for movement state.

    Vector3 _randomMoveDirection; // The randomly generated direction for movement.
    bool _isMoving; // Flag indicating if the NPC is currently moving.


    private void Start()
    {
        // Initialize the random idle and movement durations.
        _randomIdleTime = Random.Range(_minIdleTime * 0.5f, _minIdleTime * 2f);
        _randomMovementTime = Random.Range(_minMoveTime * 0.5f, _minMoveTime * 2f);     
    }

    private void Update()
    {
        if (_isMoving)
        {
            MoveRandomDuration();
        }
        else
        {
            IdleRandomDuration();
        }
    }

    private void MoveRandomDuration ()
    {
        MoveWithinBounds();

        
        _randomMovementTime -= Time.deltaTime;
        // Check if the movement duration has expired.
        if (_randomMovementTime < 0)
        {
            _isMoving = false;
            _randomIdleTime = Random.Range(_minIdleTime * 0.5f, _minIdleTime * 2f);
        }
    }
    private void IdleRandomDuration()
    {
        
        _randomIdleTime -= Time.deltaTime;
        // Check if the idle duration has expired.
        if (_randomIdleTime < 0f)
        {
            _isMoving = true;
            _randomMovementTime = Random.Range(_minMoveTime * 0.5f, _minMoveTime * 2f);
            
            // Generate a random movement direction.
            _randomMoveDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
        }
    }

    private void MoveWithinBounds()
    {
        // Calculate the new position based on the current position, move direction, move speed, and delta time.
        Vector3 newPosition = transform.position + _randomMoveDirection * _moveSpeed * Time.deltaTime;

        // Clamp the X position within the game boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, _minDistanceX, _maxDistanceX);

        // Clamp the Y position within the game boundaries
        newPosition.y = Mathf.Clamp(newPosition.y, _minDistanceY, _maxDistanceY);

        // Update the NPC's position
        transform.position = newPosition;
    }


    private void OnDrawGizmosSelected()
    {

        // Calculate the center position of the perimeter box
        Vector3 center = new Vector3((_minDistanceX + _maxDistanceX) * 0.5f, (_minDistanceY + _maxDistanceY) * 0.5f, transform.position.z);

        // Calculate the size of the perimeter box
        Vector3 size = new Vector3(_maxDistanceX - _minDistanceX, _maxDistanceY - _minDistanceY, 0f);

        // Set Gizmos color
        Gizmos.color = Color.blue;

        // Draw the perimeter box wireframe
        Gizmos.DrawWireCube(center, size);
    }

}

