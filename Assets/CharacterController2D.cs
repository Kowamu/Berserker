using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField]
    private CapsuleCollider2D _collider;
    
    [SerializeField]
    private float _smoothing = 30f;
    
    [SerializeField]
    private int _maxIteration = 3;

    [SerializeField] 
    private LayerMask _layerMask;
    
    [SerializeField]
    private LayerMask _characterLayerMask;

    private Transform _transform;
    private Vector2 _position;
    private Vector2 _velocity;
    private Vector2 _origin;
    private ContactFilter2D _contactFilter;
    private readonly RaycastHit2D[] _raycastHits = new RaycastHit2D[2];
    private readonly Collider2D[] _collisions = new Collider2D[4];
    private int _collisionsCount;
    
    public Vector2 Position
    {
        get => _position;
        set
        {
            _position = value;
            _origin = value + _collider.offset;
        }
    }
    
    public Vector2 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }

    private void Awake()
    {
        _transform = transform;
        Position = _transform.position;
        _contactFilter = new ContactFilter2D();
    }

    public void Move(float deltaTime)
    {
        const float distanceThreshold = 0.001f;
        
        var speed = Velocity.magnitude;
        if (speed <= 0f) return;

        var direction = Velocity / speed;
        var distance = speed * deltaTime;
        
        for (var i = 0; i < _maxIteration; i++)
        {
            var hit = CapsuleCast(direction, distance, _layerMask);
            if (hit.collider == null)
            {
                Position += direction * distance;
                break;
            }
            
            Position += direction * hit.distance + hit.normal * distanceThreshold;
            distance -= hit.distance;
            direction -= Vector2.Dot(direction, hit.normal) * hit.normal;
            
            if (Vector2.Dot(direction, _velocity) < Mathf.Epsilon * 8f)
            {
                break;
            }
        }
        
        _transform.position = Vector2.Lerp(_transform.position, Position, _smoothing * deltaTime);

        //var overlappingCharacters = CheckOverlappingColliders(_characterLayerMask);
    }

    public RaycastHit2D CapsuleCast(Vector2 direction, float distance, LayerMask layerMask = default)
    {
        _raycastHits[0] = default;
        _raycastHits[1] = default;
        _contactFilter.SetLayerMask(layerMask);
        
        Physics2D.CapsuleCast(
            _origin, _collider.size, _collider.direction, 0f, direction, _contactFilter, _raycastHits, distance);
        
        return _raycastHits.FirstOrDefault(hit => hit.collider != _collider);
    }

    public IEnumerable<Collider2D> CheckOverlappingColliders(LayerMask layerMask)
    {
        _contactFilter.SetLayerMask(layerMask);
        _collisionsCount = _collider.OverlapCollider(_contactFilter, _collisions);

        return _collisions
            .Take(_collisionsCount)
            .Where(overlappingCollider => overlappingCollider != _collider);
    }
}
