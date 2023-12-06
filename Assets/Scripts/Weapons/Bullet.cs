using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] LayerMask m_CollidableSurfaces;
    float speed;
    float timeToLive;
    float currentTime;
    Vector3 initialPosition;
    bool canPenetrate;
    float maxPenetrationDistance;
    RaycastHit hit;
    RaycastHit penetrationTest;
    Vector3 p1;
    Vector3 p2;
    LineRenderer lineRenderer;
    float spreadFactor;
    bool penetrated;
    float damage;
    bool explode;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void OnEnable()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        simulateBullet();
    }
    /// <summary>
    /// This method sets up the bullet.
    /// </summary>
    /// <param name="speed">Muzzle velocity of bullet</param>
    /// <param name="initialPosition">World position on spawn</param>
    /// <param name="timeToLive">time the bullet should be alive</param>
    /// <param name="canPenetrate">Can this bullet penetrate</param>
    public void SetupBullet(float speed, Vector3 initialPosition, float timeToLive, bool canPenetrate = false, float maxPenetrationDistance = 0, float spreadFactor = 0.0f)
    {
        currentTime = 0;
        this.speed = speed;
        this.initialPosition = initialPosition;
        this.timeToLive = timeToLive;
        this.canPenetrate = canPenetrate;
        this.maxPenetrationDistance = maxPenetrationDistance;
        if (spreadFactor > 0.0f)
        {
            Vector3 angles = transform.rotation.eulerAngles;
            Vector2 offset = new Vector2(Random.Range(-spreadFactor, spreadFactor), Random.Range(-spreadFactor, spreadFactor));
            //Debug.Log("Generated Offset : " + offset);
            transform.rotation = Quaternion.Euler(angles.x + (offset.x), angles.y + (offset.y), angles.z);
        }
    }
    public void SetDamage(float damage, bool explode = false)
    {
        this.damage = damage;
        this.explode = explode;
    }
    public void SetupBullet(float speed, Vector3 initialPosition, float timeToLive, Vector3 spread, bool canPenetrate = false, float maxPenetrationDistance = 0)
    {
        currentTime = 0;
        this.speed = speed;
        this.initialPosition = initialPosition;
        this.timeToLive = timeToLive;
        this.canPenetrate = canPenetrate;
        this.maxPenetrationDistance = maxPenetrationDistance;
        transform.forward += transform.TransformDirection(spread);
    }
    void simulateBullet()
    {
        if(currentTime > timeToLive)
        {
            GetComponent<ReturnToPool>().SendToPool();
        }
        p1 = getBulletPosition();
        currentTime += Time.deltaTime;
        p2 = getBulletPosition();
        rayCastSegment(p1, p2);
    }

    Vector3 getBulletPosition()
    {
        Vector3 position = (speed * currentTime * transform.forward + initialPosition) + (0.5f * currentTime * currentTime * 9.8f * Vector3.down);
        return position;
    }

    void rayCastSegment(Vector3 p1,  Vector3 p2)
    {
        Vector3 direction = (p2-p1).normalized;
        float distance = Vector3.Distance(p1, p2);
        if(Physics.Raycast(p1, direction, out hit, distance,m_CollidableSurfaces))
        {
            WeaponsSingleton.Instance.InvokeTakeDamageEvent(hit.collider.gameObject.GetInstanceID(), (int)damage, explode);
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Meat"))
            {
                // add particle fx here ignore bullet hole
                var blood = WeaponsSingleton.Instance.BloodPool.Get();
                blood.transform.position = hit.point;
                blood.transform.forward = direction;
                blood.GetComponent<ParticleSystem>().Play();
                blood.GetComponent<DecalHandler>().Setup(30.0f);
                //Debug.Log("Collided with : " + hit.collider.gameObject.name);
                //var meatCollider = hit.collider.GetComponent<TestSubjectGore>();
                //if(meatCollider)
                //{
                //    meatCollider.TakeDamage((int)damage, explode);
                //}
                
            }
            else
            {
                var impactHole = WeaponsSingleton.Instance.DecalPool.Get();
                impactHole.GetComponent<DecalHandler>().Setup(15.0f);
                impactHole.transform.position = hit.point;
                impactHole.transform.forward = direction;
                lineRenderer.SetPosition(0, hit.point);
            }
            transform.position = hit.point;
            lineRenderer.SetPosition(1, p1);
            // we have a hit and we check for penetration test
            if (canPenetrate)
            {
                // we perform a reverse raycast and check for penetration
                // we set p1 on next frame to the other side of the wall.
                // we alter the angle a little on impact
                Vector3 targetReverseRaycastPosition = hit.point + (direction * maxPenetrationDistance);
                if(Physics.Raycast(targetReverseRaycastPosition, -direction, out penetrationTest, maxPenetrationDistance, m_CollidableSurfaces))
                {
                    // again check for meat
                    // the bullet has penetrated
                    if (penetrationTest.collider.gameObject.layer == LayerMask.NameToLayer("Meat"))
                    {
                        // add particle fx here ignore bullet hole
                        var blood = WeaponsSingleton.Instance.BloodPool.Get();
                        blood.transform.position = hit.point;
                        blood.transform.forward = direction;
                        blood.GetComponent<ParticleSystem>().Play();
                        blood.GetComponent<DecalHandler>().Setup(30.0f);
                    }
                    else
                    {
                        var exitHole = WeaponsSingleton.Instance.DecalPool.Get();
                        exitHole.transform.position = penetrationTest.point;
                        exitHole.GetComponent<DecalHandler>().Setup(30.0f);
                        exitHole.transform.forward = -direction;
                    }
                    penetrated = true;
                    speed *= 0.9f;
                }
                else
                {
                    GetComponent<ReturnToPool>().SendToPool();
                    return;
                }
            }
            else
            {
                GetComponent<ReturnToPool>().SendToPool();
            }
            
        }
        else
        {
            lineRenderer.SetPosition(0, p2);
            lineRenderer.SetPosition(1, p1);
            transform.position = p2;
        }
    }
}
