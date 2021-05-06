using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector3 velocity;

    public LineRenderer cohensionVector;

    public LineRenderer seperationVector;

    public LineRenderer alignmentVector;

    public LineRenderer velocityVector;

    public float maxSpeed = 1.0f;

    public float smoothTurning = 1.0f;

    public float cohensionRadius = 1.0f;

    public float seperationRadius = 1.0f;

    public float alignmentRadius = 1.0f;

    public float bouncingFactor = 10.0f;

    public float containerMinX = 0.0f;
    public float containerMaxX = 0.0f;

    public float containerMinY = 0.0f;
    public float containerMaxY = 0.0f;

    public int ID;

    //[Range(0.0f, 1.0f)]
    //public float cohensionFactor = 1.0f;

    //[Range(0.0f, 1.0f)]
    //public float seperateFactor = 1.0f;

    //[Range(0.0f, 1.0f)]
    //public float alignmentFactor = 1.0f;

    public bool drawCohensionRadius = true;

    public bool drawSeperationRadius = true;

    public bool drawAlignmentRadius = true;

    public bool drawVelocity = true;

    public bool drawVelocityComponents = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cohenV = Cohension();
        Vector3 seperateV = Seperation();
        Vector3 alignV = Alignment();
        Vector3 containmentV = Containment();

        cohenV *= Constants.cohensionFactor;
        seperateV *= Constants.seperationFactor;
        alignV *= Constants.alignmentFactor;

        cohenV = limit_speed(cohenV);
        seperateV = limit_speed(seperateV);
        alignV = limit_speed(alignV);

        velocity += transform.up + cohenV + seperateV + alignV + containmentV;

        velocity = limit_speed(velocity);

        Move(velocity, maxSpeed);

        //Only in debug
        if (drawVelocityComponents)
        {
            Debug.DrawLine(transform.position, transform.position + (cohenV * Constants.cohensionFactor).normalized, Color.blue);
            Debug.DrawLine(transform.position, transform.position + (seperateV * Constants.seperationFactor).normalized, Color.red);
            Debug.DrawLine(transform.position, transform.position + (alignV * Constants.alignmentFactor).normalized, Color.yellow);
            Debug.DrawLine(transform.position, transform.position + containmentV.normalized, Color.black);
        }
        /*-----------------------*/

        if(Constants.enableSingleBoidViewing == false)
        {
            if (Constants.viewVelocityComponentsVector)
            {
                cohensionVector.positionCount = 2;
                seperationVector.positionCount = 2;
                alignmentVector.positionCount = 2;

                cohensionVector.SetPosition(1, cohenV / maxSpeed);
                cohensionVector.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1);

                seperationVector.SetPosition(0, seperateV / maxSpeed);
                seperationVector.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1);

                alignmentVector.SetPosition(0, alignV / maxSpeed);
                alignmentVector.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1);
            }
            else
            {
                cohensionVector.positionCount = 0;
                seperationVector.positionCount = 0;
                alignmentVector.positionCount = 0;
            }
        }
        else
        {
            if(Constants.singleBoidID == ID)
            {
                if (Constants.viewVelocityComponentsVector)
                {
                    cohensionVector.positionCount = 2;
                    seperationVector.positionCount = 2;
                    alignmentVector.positionCount = 2;

                    cohensionVector.SetPosition(1, cohenV / maxSpeed);
                    cohensionVector.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1);

                    seperationVector.SetPosition(0, seperateV / maxSpeed);
                    seperationVector.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1);

                    alignmentVector.SetPosition(0, alignV / maxSpeed);
                    alignmentVector.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1);
                }
                else
                {
                    cohensionVector.positionCount = 0;
                    seperationVector.positionCount = 0;
                    alignmentVector.positionCount = 0;
                }
            }
            else
            {
                cohensionVector.positionCount = 0;
                seperationVector.positionCount = 0;
                alignmentVector.positionCount = 0;
            }
        }
        
        if(Constants.enableSingleBoidViewing == false)
        {
            if (Constants.viewVelocityVector)
            {
                velocityVector.positionCount = 2;

                velocityVector.SetPosition(0, velocity / maxSpeed);
                velocityVector.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1);
            }
            else
            {
                velocityVector.positionCount = 0;
            }
        }
        else
        {
            if(Constants.singleBoidID == ID)
            {
                if (Constants.viewVelocityVector)
                {
                    velocityVector.positionCount = 2;

                    velocityVector.SetPosition(0, velocity / maxSpeed);
                    velocityVector.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1);
                }
                else
                {
                    velocityVector.positionCount = 0;
                }
            }
            else
            {
                velocityVector.positionCount = 0;
            }
        } 
    }

    void Move(Vector3 _v, float speed)
    {
        _v = limit_speed(_v);

        _v = Vector3.Lerp(velocity, _v, Vector3.Distance(velocity, _v) / Time.deltaTime);

        transform.position += _v * speed * Time.deltaTime;

        float angle = Mathf.Atan2(_v.y, _v.x) * Mathf.Rad2Deg - 90;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), smoothTurning * Time.deltaTime);
        
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (drawVelocity)
        {
            Debug.DrawLine(transform.position, transform.position + _v, Color.green);
        }
    }

    Vector3 steerTowardTarget(Vector3 target)
    {
        Vector3 desired = target - transform.position;

        desired = desired.normalized;
        desired *= maxSpeed;

        Vector3 steer = desired - velocity;

        steer = limit_speed(desired);

        return steer;
    }

    Vector3 limit_speed(Vector3 v)
    {
        if(v.magnitude > maxSpeed)
        {
            v = v.normalized * maxSpeed;
        }

        return v;
    }

    Vector3 Cohension()
    {
        Vector3 average = Vector3.zero;
        int count = 0;

        Boid[] boids = FindObjectsOfType<Boid>();

        for(int i = 0; i < boids.Length; ++i)
        {
            Vector3 diff = boids[i].transform.position - transform.position;

            if (diff.magnitude > 0 && diff.magnitude < cohensionRadius)
            {
                average += boids[i].transform.position;
                ++count;
            }
        }

        if(count > 0)
        {
            average /= count;

            Vector3 steer = steerTowardTarget(average);

            return steer;
        }

        return Vector3.zero;
    }

    Vector3 Seperation()
    {
        Vector3 average = Vector3.zero;
        int count = 0;

        Boid[] boids = FindObjectsOfType<Boid>();

        for (int i = 0; i < boids.Length; ++i)
        {
            float dst = Vector3.Distance(boids[i].transform.position, transform.position);

            if (dst > 0 && dst < seperationRadius)
            {
                Vector3 diff = boids[i].transform.position - transform.position;

                diff = diff.normalized;
                diff /= dst;

                average -= diff;
                ++count;
            }
        }

        if (count > 0)
        {
            average /= count;
        }

        return average;
    }


    Vector3 Alignment()
    {
        Vector3 average = Vector3.zero;
        int count = 0;

        Boid[] boids = FindObjectsOfType<Boid>();

        for (int i = 0; i < boids.Length; ++i)
        {
            float dst = Vector3.Distance(boids[i].transform.position, transform.position);

            if (dst > 0 && dst < alignmentRadius)
            {
                average += boids[i].velocity;
                ++count;
            }
        }

        if (count > 0)
        {
            average /= count;

            average = limit_speed(average);

            Vector3 steer = average - velocity;

            steer = limit_speed(steer);

            return steer;
        }

        return average;
    }

    Vector3 Containment()
    {
        Vector3 v = Vector3.zero;

        if(transform.position.x < containerMinX)
        {
            v.x = bouncingFactor;
        }else if(transform.position.x > containerMaxX)
        {
            v.x = -bouncingFactor;
        }

        if (transform.position.y < containerMinY)
        {
            v.y = bouncingFactor;
        }
        else if (transform.position.y > containerMaxY)
        {
            v.y = -bouncingFactor;
        }

        return v;
    }

    private void OnDrawGizmos()
    {
        if (drawCohensionRadius)
        {
            Gizmos.DrawWireSphere(transform.position, cohensionRadius);
        }

        if (drawSeperationRadius)
        {
            Gizmos.DrawWireSphere(transform.position, seperationRadius);
        }

        if (drawAlignmentRadius)
        {
            Gizmos.DrawWireSphere(transform.position, alignmentRadius);
        }
    }
}
