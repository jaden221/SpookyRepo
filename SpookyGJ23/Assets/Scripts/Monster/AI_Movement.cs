using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.WSA;

public class AI_Movement : MonoBehaviour
{
    [Header("Roaming Movement")]

    [SerializeField] float roamSpeed = 6;
    [SerializeField] int minRandTime = 1;
    [SerializeField] int maxRandTime = 5;
    float curRandTime;
    [SerializeField] int minRandRot = 25;
    [SerializeField] int maxRandRot = 110;
    [SerializeField] float rotTurnSpeed = 5;
    float curRandRotTarget;
    float curRotatedAmount = 0;
    float rotDirMult = 1;
    float rotPerFrame = 0;
    [SerializeField] Transform rotationTrans;

    [Space(5)]
    [Header("Tracks")]

    [SerializeField] float trackLeaveTime = 1;
    [SerializeField] Transform trackPrefab;

    [Space(5)]
    [Header("Circling Behaviour")]

    [SerializeField] float circlingSpeed = 5;
    [SerializeField] int circleAtks = 4;
    [SerializeField] float timeToCircleMin = 2;
    [SerializeField] float timeToCircleMax = 5;
    [SerializeField] float circlePounceSpeed = 15;
    [SerializeField] float circleTimeToAttack = .5f;
    [SerializeField] float circleTimeToStopPounce = 1f;

    Rigidbody2D rigidbody;
    AI_LosChecker losChecker;
    DistanceJoint2D distanceJoint;

    Coroutine RoamInputCor;
    Coroutine LeaveTracksCor;

    Transform target;
    bool targetlocked = false;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        losChecker = GetComponentInChildren<AI_LosChecker>();
        distanceJoint = GetComponent<DistanceJoint2D>();

        distanceJoint.enabled = false;
    }

    void OnEnable() 
    {
        StartCoroutine(RoamState());

        losChecker.OnHasTarget += HandleHasTarget;
        losChecker.OnNoTarget += HandleNoTarget;
    }

    void OnDisable()
    {
        losChecker.OnHasTarget -= HandleHasTarget;
        losChecker.OnNoTarget -= HandleNoTarget;
    }

    void HandleHasTarget(Transform transform)
    {
        if (targetlocked) return;
        target = transform;
    }

    void HandleNoTarget()
    {
        if (targetlocked) return;
        target = null;
    }

    IEnumerator RoamState() 
    {
        RoamInputCor = StartCoroutine(ChangeRoamInput());
        LeaveTracksCor = StartCoroutine(LeaveTracks());

        while (target == null) 
        {
            //if has yet to reach its rotate target
            if (Mathf.Abs(curRotatedAmount) < Mathf.Abs(curRandRotTarget)) 
            {
                rotPerFrame = rotTurnSpeed * rotDirMult;

                rotationTrans.eulerAngles = new Vector3(0f, 0f, rotationTrans.eulerAngles.z + rotPerFrame);

                curRotatedAmount += rotPerFrame;
            }

            rigidbody.velocity = rotationTrans.right * roamSpeed;

            yield return new WaitForFixedUpdate();
        }

        //Cleanup
        StopCoroutine(RoamInputCor);
        StopCoroutine(LeaveTracksCor);

        rigidbody.velocity = Vector2.zero;

        //Only State to go to from here
        AttackTarget();
    }

    IEnumerator ChangeRoamInput() 
    { 
        while (true) 
        {
            curRotatedAmount = 0;
            curRandTime = Random.Range(minRandTime, maxRandTime + 1);
            curRandRotTarget = Random.Range(minRandRot, maxRandRot + 1);

            rotDirMult = Random.Range(-1, 2);
            while (rotDirMult == 0) rotDirMult = Random.Range(-1, 2);

            yield return new WaitForSeconds(curRandTime);
        }
    }

    IEnumerator LeaveTracks() 
    { 
        while (true) 
        {
            yield return new WaitForSeconds(trackLeaveTime);

            Transform trackInst = Instantiate(trackPrefab, transform.position, Quaternion.identity);
            trackInst.rotation = rotationTrans.rotation;
        }
    }

    void AttackTarget() 
    { 
        //if passed health or time threshold then stop attacking target
        
        //when called check if can still attack otherwise atk

        //pick a random attack behaviour and execute it
        targetlocked = true;
        int randomAttack = Random.Range(1, 4);
        switch (randomAttack) 
        {
            case 1:
            StartCoroutine(CirclingBehaviour()); 
            break;

            case 2:
            StartCoroutine(CirclingBehaviour());
            break;

            case 3:
            StartCoroutine(CirclingBehaviour());
            break;
        }

        //When done attacking flee and then start roaming again
    }

    IEnumerator CirclingBehaviour() 
    {
        for (int i = 0; i < circleAtks; i++)
        {
            int randDir = Random.Range(-1, 2);
            while (randDir == 0) randDir = Random.Range(-1, 2);

            yield return StartCoroutine(CircleTarget(Random.Range(timeToCircleMin, timeToCircleMax), randDir));

            rigidbody.velocity = Vector3.zero;
            Vector2 dirToPlayer = (target.position - transform.position).normalized;
            rigidbody.AddForce(dirToPlayer * circlePounceSpeed, ForceMode2D.Impulse);
            yield return new WaitForSeconds(circleTimeToAttack);
            Debug.Log("Attack");
            yield return new WaitForSeconds(circleTimeToStopPounce);
            Debug.Log("Stop Pounce");
        }

        AttackTarget();
    }

    IEnumerator CircleTarget(float timeToCircle, float dir) 
    {
        float timeCircled = 0;

        distanceJoint.enabled = true;

        while (timeCircled < timeToCircle)
        {
            distanceJoint.connectedAnchor = target.position;

            Vector3 difference = target.position - transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            rotationTrans.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

            rigidbody.velocity = rotationTrans.up * circlingSpeed * dir;

            timeCircled += Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }

        distanceJoint.enabled = false;
    }
}