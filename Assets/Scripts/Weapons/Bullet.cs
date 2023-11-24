using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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
            Debug.Log("Generated Offset : " + offset);
            transform.rotation = Quaternion.Euler(angles.x + (offset.x), angles.y + (offset.y), angles.z);
        }
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
        if(Physics.Raycast(p1, direction, out hit, distance))
        {
            // we have a hit and we check for penetration test
            if(canPenetrate)
            {
                // we perform a reverse raycast and check for penetration
                // we set p1 on next frame to the other side of the wall.
                // we alter the angle a little on impact
                Vector3 targetReverseRaycastPosition = hit.point + (direction * maxPenetrationDistance);
                if(Physics.Raycast(targetReverseRaycastPosition, -direction, out penetrationTest, maxPenetrationDistance))
                {
                    // the bullet has penetrated
                    speed *= 0.9f;
                    var exitHole = WeaponsSingleton.Instance.DecalPool.Get();
                    exitHole.transform.position = penetrationTest.point;
                    exitHole.GetComponent<DecalHandler>().Setup(15.0f);
                    exitHole.transform.forward = -direction;
                    // spawn an exit hole at penertationTest.point;
                    // we will have to set p1 to penetrationtest.point?
                }
            }
            //else
            //{
            //    GetComponent<ReturnToPool>().SendToPool();
            //}
            var impactHole = WeaponsSingleton.Instance.DecalPool.Get();
            impactHole.GetComponent<DecalHandler>().Setup(15.0f);
            impactHole.transform.position = hit.point;
            impactHole.transform.forward = direction;
            lineRenderer.SetPosition(0, hit.point);
            transform.position = hit.point;
            lineRenderer.SetPosition(1, p1);
            if(!canPenetrate)
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
