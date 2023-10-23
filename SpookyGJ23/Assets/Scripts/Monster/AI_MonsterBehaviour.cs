using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.WSA;

public class AI_MonsterBehaviour : MonoBehaviour
{
    [Header("Roaming Movement")]

    [SerializeField] float roamSpeed = 6;
    [SerializeField] int minRandTimeRoam = 1;
    [SerializeField] int maxRandTimeRoam = 5;
    [SerializeField] int minRandRotRoam = 25;
    [SerializeField] int maxRandRotRoam = 110;
    [SerializeField] float rotTurnSpeedRoam = 5;
    float curRandRotTarget;
    float curRotatedAmount = 0;
    float rotDirMult = 1;
    float rotPerFrame = 0;
    [SerializeField] Transform rotationTrans;
    [SerializeField] string walkAnim = "Walk";

    [Space(5)]
    [Header("Tracks")]

    [SerializeField] float trackLeaveTime = 1;
    [SerializeField] Transform trackPrefab;

    [Space(5)]
    [Header("Attacking")]
    [SerializeField] float[] healthThresholds = {.75f, .5f, .25f };
    int curThreshold = 0;

    [Space(5)]
    [Header("Pounce Behaviour")]

    [SerializeField] float circlingSpeed = 5;
    [SerializeField] int circleAtks = 4;
    [SerializeField] float timeToCircleMin = 2;
    [SerializeField] float timeToCircleMax = 5;
    [SerializeField] float circlePounceSpeed = 15;
    [SerializeField] float circleTimeToAttack = .5f;
    [SerializeField] float circleTimeToStopPounce = 1f;
    [SerializeField] string pounceAnim = "Pounce";

    [Space(5)]
    [Header("Shoot Proj Behaviour")]

    [SerializeField] Transform projPrefab;
    [SerializeField] Transform projSpawn;
    [SerializeField] float minShootCircleTime = 1;
    [SerializeField] float maxShootCircleTime = 2;
    [SerializeField] int minShootTimes = 2;
    [SerializeField] int maxShootTimes = 5;
    [SerializeField] float shootTime = 1.5f;
    [SerializeField] string shootAnim = "Shoot";

    [Space(5)]
    [Header("Chase & Atk Behaviour")]

    [SerializeField] int chaseTimeMin = 8;
    [SerializeField] int chaseTimeMax = 14;
    [SerializeField] float chaseDistToAtk = 4;
    [SerializeField] float chaseSpeed = 25;
    [SerializeField] string atkAnim = "Attack";
    [SerializeField] string runAnim = "Run";

    [Space(5)]
    [Header("Flee Behaviour")]
    [SerializeField] int fleeTimeMin = 4;
    [SerializeField] int fleeTimeMax = 8;
    [SerializeField] float fleeSpeed = 6;
    [SerializeField] int minRandTimeFlee = 1;
    [SerializeField] int maxRandTimeFlee = 5;
    [SerializeField] int minRandRotFlee = 25;
    [SerializeField] int maxRandRotFlee = 110;
    [SerializeField] float rotTurnSpeedFlee = 5;


    Rigidbody2D rigidbody;
    AI_LosChecker losChecker;
    SpringJoint2D springJoint;
    Animator animator;
    Health health;

    Coroutine RoamInputCor;
    Coroutine LeaveTracksCor;

    Transform target;
    bool targetlocked;
   

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        losChecker = GetComponentInChildren<AI_LosChecker>();
        springJoint = GetComponent<SpringJoint2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();

        springJoint.enabled = false;
    }

    void OnEnable()
    {
        StartCoroutine(RoamBehaviour());

        losChecker.OnHasTarget += HandleHasTarget;
        losChecker.OnNoTarget += HandleNoTarget;

        health.OnDeath += HandleDeath;
    }
    

    void OnDisable()
    {
        losChecker.OnHasTarget -= HandleHasTarget;
        losChecker.OnNoTarget -= HandleNoTarget;

        health.OnDeath -= HandleDeath;
    }

    void HandleDeath()
    {
        StopAllCoroutines();
        rigidbody.velocity = Vector2.zero;
        animator.SetBool("Dead", true);
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

    void AttackTarget() 
    { 
       if (health.GetHealthPercent < curThreshold) 
       {
            targetlocked = false;
            target = null;
            curThreshold++;

            StartCoroutine(FleeBehaviour());
            return;
       }

        targetlocked = true;
        int randomAttack = Random.Range(1, 4);
        switch (randomAttack) 
        {
            case 1:
                Debug.Log("Beginning Pounce Behaviour");
                StartCoroutine(PounceBehaviour()); 
                break;

            case 2:
                Debug.Log("Beginning Shoot Behaviour");
                StartCoroutine(ShootBehaviour());
                break;

            case 3:
                Debug.Log("Beginning Chase Behaviour");
                StartCoroutine(ChaseBehaviour());
                break;
        }

        //When done attacking flee and then start roaming again
    }

    IEnumerator ChangeMoveInput(float minTime, float maxTime, float minRot, float maxRot)
    {
        float randTime;

        while (true)
        {
            curRotatedAmount = 0;
            randTime = Random.Range(minTime, maxTime + 1);
            curRandRotTarget = Random.Range(minRot, maxRot + 1);

            rotDirMult = Random.Range(-1, 2);
            while (rotDirMult == 0) rotDirMult = Random.Range(-1, 2);

            yield return new WaitForSeconds(randTime);
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

    IEnumerator CircleTarget(float timeToCircle)
    {
        float timeCircled = 0;

        springJoint.enabled = true;

        int randDir = Random.Range(-1, 2);
        while (randDir == 0) randDir = Random.Range(-1, 2);

        while (timeCircled < timeToCircle)
        {
            springJoint.connectedAnchor = target.position;

            Vector3 difference = target.position - transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            rotationTrans.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

            rigidbody.velocity = rotationTrans.up * circlingSpeed * randDir;

            timeCircled += .02f;

            yield return new WaitForFixedUpdate();
        }

        rigidbody.velocity = Vector2.zero;

        springJoint.enabled = false;
    }

    #region Behaviours

    /// <summary>
    /// Randomly picks a target dir then slowly rotates its movement towards that direction. Leaves tracks
    /// </summary>
    /// <returns></returns>
    IEnumerator RoamBehaviour()
    {
        RoamInputCor = StartCoroutine(
            ChangeMoveInput(minRandTimeRoam, maxRandTimeRoam, minRandRotRoam, maxRandRotRoam));
        LeaveTracksCor = StartCoroutine(LeaveTracks());

        animator.SetBool(walkAnim, true);

        while (target == null)
        {
            //if has yet to reach its rotation target keep rotating
            if (Mathf.Abs(curRotatedAmount) < Mathf.Abs(curRandRotTarget))
            {
                rotPerFrame = rotTurnSpeedRoam * rotDirMult;

                rotationTrans.eulerAngles = new Vector3(0f, 0f, rotationTrans.eulerAngles.z + rotPerFrame);

                curRotatedAmount += rotPerFrame;
            }

            rigidbody.velocity = rotationTrans.right * roamSpeed;

            yield return new WaitForFixedUpdate();
        }

        //Cleanup
        StopCoroutine(RoamInputCor);
        StopCoroutine(LeaveTracksCor);

        animator.SetBool(walkAnim, false);
        rigidbody.velocity = Vector2.zero;
        curRotatedAmount = 0f;

        //Only State to go to from here
        AttackTarget();
    }

    /// <summary>
    /// Circle Target rand times then pounce
    /// </summary>
    /// <returns></returns>
    IEnumerator PounceBehaviour()
    {
        for (int i = 0; i < circleAtks; i++)
        {
            animator.SetBool(walkAnim, true);
            yield return StartCoroutine(CircleTarget(Random.Range(timeToCircleMin, timeToCircleMax)));

            //When done circling target run at player
            animator.SetBool(walkAnim, false);
            animator.SetBool(runAnim, true);
            rigidbody.velocity = Vector3.zero;
            Vector2 dirToPlayer = (target.position - transform.position).normalized;
            rigidbody.AddForce(dirToPlayer * circlePounceSpeed, ForceMode2D.Impulse);

            yield return new WaitForSeconds(circleTimeToAttack); 
            //After x seconds of running at player attack

            animator.SetBool(runAnim, false);
            animator.SetBool(pounceAnim, true);
            
            yield return new WaitForSeconds(circleTimeToStopPounce);
            //After y seconds of attacking stop
            animator.SetBool(pounceAnim, false);
        }

        animator.SetBool(pounceAnim, false);
        rigidbody.velocity = Vector2.zero;

        AttackTarget();
    }

    /// <summary>
    /// Circle target random times, then shoot
    /// </summary>
    /// <returns></returns>
    IEnumerator ShootBehaviour()
    {
        int shootTimes = Random.Range(minShootTimes, maxShootTimes + 1);

        for (int i = 0; i < shootTimes; i++)
        {
            yield return StartCoroutine(CircleTarget(Random.Range(minShootCircleTime, maxShootCircleTime)));

            animator.SetBool(shootAnim, true);
        }

        animator.SetBool(shootAnim, false);

        AttackTarget();
    }

    public void AE_Shoot() 
    {
        Transform projInst = Instantiate(projPrefab, projSpawn.position, Quaternion.identity);

        projInst.right = projSpawn.right;
    }

    IEnumerator ChaseBehaviour() 
    {
        float behaviourTime = Random.Range(chaseTimeMin, chaseTimeMax + 1);
        float curBehaviourTime = 0;

        while (curBehaviourTime < behaviourTime) 
        {
            float distToTarget = Vector2.Distance(transform.position, target.position);

            if (animator.GetBool(atkAnim) == false) 
            {
                if (distToTarget < chaseDistToAtk) //Atk
                {
                    animator.SetBool(runAnim, false);
                    animator.SetBool(atkAnim, true);

                    rigidbody.velocity = Vector2.zero;
                }
                else //Chase Target
                {
                    animator.SetBool(atkAnim, false);
                    animator.SetBool(runAnim, true);

                    rigidbody.velocity = (target.position - transform.position).normalized * chaseSpeed;
                }
            }

            curBehaviourTime += .02f; // .02 is time between fixed updates

            yield return new WaitForFixedUpdate();
        }

        rigidbody.velocity = Vector2.zero;
        animator.SetBool(runAnim, false);
        animator.SetBool(atkAnim, false);

        AttackTarget();
    }

    /// <summary>
    /// Same as RoamBehviour but with minor differences
    /// </summary>
    /// <returns></returns>
    IEnumerator FleeBehaviour() 
    {
        RoamInputCor = StartCoroutine(
            ChangeMoveInput(minRandTimeRoam, maxRandTimeRoam, minRandRotRoam, maxRandRotRoam));
        LeaveTracksCor = StartCoroutine(LeaveTracks());

        animator.SetBool(runAnim, true);

        float curTimeFleeing = 0;
        float fleeTime = Random.Range(fleeTimeMin, fleeTimeMax);

        while (curTimeFleeing < fleeTime)
        {
            //if has yet to reach its rotation target keep rotating
            if (Mathf.Abs(curRotatedAmount) < Mathf.Abs(curRandRotTarget))
            {
                rotPerFrame = rotTurnSpeedFlee * rotDirMult;

                rotationTrans.eulerAngles = new Vector3(0f, 0f, rotationTrans.eulerAngles.z + rotPerFrame);

                curRotatedAmount += rotPerFrame;
            }

            rigidbody.velocity = rotationTrans.right * fleeSpeed;

            curTimeFleeing += .02f;

            yield return new WaitForFixedUpdate();
        }

        //Cleanup
        StopCoroutine(RoamInputCor);
        StopCoroutine(LeaveTracksCor);

        animator.SetBool(runAnim, false);

        rigidbody.velocity = Vector2.zero;
        curRotatedAmount = 0f;

        //Only State to go to from here
        StartCoroutine(RoamBehaviour());
    }

    #endregion
}