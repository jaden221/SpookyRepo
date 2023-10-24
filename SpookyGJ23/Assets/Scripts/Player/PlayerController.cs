/*
    sso was here bahahah
*/

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("GameObjects")]
    [SerializeField] private GameObject firePrefab;
    #region Player Flame
    [SerializeField] private GameObject plrFlame;
    [SerializeField] private Vector2 plrFlameLeft = new Vector2(0.6f, 0.87f);
    [SerializeField] private Vector2 plrFlameRight = new Vector2(-0.6f, 0.87f);
    [SerializeField] private Vector2 plrFlameUp = new Vector2(0f, 2.69f);
    [SerializeField] private Vector2 largeFlameScale = new Vector2(0.3f, 0.3f);
    [SerializeField] private Vector2 mediumFlameScale = new Vector2(0.2f, 0.2f);
    [SerializeField] private Vector2 smallFlameScale = new Vector2(0.13f, 0.13f);
    [SerializeField] private Vector2 deadFlameScale = new Vector2(0, 0);
    // *Unused* [SerializeField] private float flameSizeSmoothing = 5f;
    #endregion

    [Space(5)]

    [Header("States")]
    [SerializeField] public bool canMove = false;
    [SerializeField] public bool canTakeDamage = false;

    [Space(5)]

    [Header("Mechanics")]
    [SerializeField] private float speed = 5;
    [SerializeField] public int lives = 3;

    [Space(5)]
    
    [Header("Obstruction")]
    [SerializeField] private Transform rayStart;
    [SerializeField] private float rayCheckDist = 5;
    [SerializeField] private LayerMask obstructionMask;

    [Space(5)]
    
    [Header("Input")]
    [SerializeField] private InputActionReference moveRef;
    [SerializeField] private InputActionReference shootRef;
    private InputAction move;
    private InputAction shoot;

    [Space(5)]

    [Header("Physics")]
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Vector2 inputDir;

    [Space(5)]

    [Header("Animation")]
    [SerializeField] private Animator plrAnim;
    [SerializeField] private SpriteRenderer plrSpriteRenderer;

    [Space(5)]

    [Header("Sound")]
    [SerializeField] private AudioSource plrAudioSource;
    [SerializeField] private AudioClip[] plrSounds;

    #endregion

    #region Processes

    void Awake()
    {
        canMove = true;
        move = moveRef;
        shoot = shootRef;
        if (rayStart == null) rayStart = transform;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        plrAnim = transform.GetChild(0).GetComponent<Animator>();
        plrFlame = transform.GetChild(1).gameObject;
        plrFlame.transform.localScale = largeFlameScale;
        plrSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        plrAudioSource = GetComponent<AudioSource>();
        StartCoroutine(Shooting());
    }

    void OnEnable()
    {
        move.Enable();
        shoot.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        shoot.Disable();
    }

    private void Update()
    {
        AnimationAndSoundsLogic();
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster") && canTakeDamage)
        {
            PlayerLoseLife(1);
            ScalePlayerFlame();
            StartCoroutine(SafeFrames());
        }
    }

    #endregion

    #region Function Which Reads And Makes Use Of Input From The Player

    private void Movement()
    {
        if (canMove) inputDir = move.ReadValue<Vector2>().normalized;

        if (inputDir.magnitude != 0)
        {
            if (canMove) rigidbody.velocity = inputDir * speed;
            else rigidbody.velocity = Vector3.zero;
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
        }
    }

    #endregion

    #region Function Which Controls Whether Animations Play Or Not And Whether The Player Sprite Should Be Flipped Depending On Input Directions Along With Sound Effects

    private void AnimationAndSoundsLogic()
    {
        #region Enable Movement Animation
        if (inputDir != Vector2.zero) // checks for the player movement
        { 
            plrAnim.SetBool("moving", true); 
            if (!plrAudioSource.isPlaying && canMove) { plrAudioSource.PlayOneShot(plrSounds[Random.Range(0, 1)]); } 
        }
        else // checks for no player movement 
            plrAnim.SetBool("moving", false);
        #endregion

        #region Direct Sprite To Input Direction
        if (inputDir.x < -0.1f && canMove) plrSpriteRenderer.flipX = true;
        else if (inputDir.x > 0.1f && canMove) plrSpriteRenderer.flipX = false;
        else if (plrAnim.GetBool("heal") && !canMove) plrFlame.transform.localPosition = plrFlameUp;

        if (plrSpriteRenderer.flipX && !plrAnim.GetBool("heal") && canMove) plrFlame.transform.localPosition = plrFlameRight;
        else if (!plrSpriteRenderer.flipX && !plrAnim.GetBool("heal") && canMove) plrFlame.transform.localPosition = plrFlameLeft;
        // by using such a large float, inputDirection changes too fast to flip back (on PC), leaving the flipped sprite in it's last flipped state
        #endregion
    }

    #endregion

    #region Global Function Which Can Be Called Upon To Take Away Player Health

    public void PlayerLoseLife(int lifeLoss)
    {
        #region Logic Which Negates Life
        lives -= lifeLoss;
        #endregion

        #region Logic Which Switches Scale Of Flame Depending On Life
        switch (lives)
        {
            case 3:
                plrFlame.transform.localScale = largeFlameScale;
                canMove = true;
                break;
            case 2:
                plrFlame.transform.localScale = mediumFlameScale;
                canMove = true;
                break;
            case 1:
                plrFlame.transform.localScale = smallFlameScale;
                canMove = true;
                break;
            case 0:
                plrFlame.transform.localScale = deadFlameScale;
                rigidbody.velocity = Vector2.zero;
                inputDir = Vector2.zero;
                canMove = false;
                GameManager.Instance.EndGame(false);
                break;
        }
        #endregion

        #region Logic For Clamping The Amount Of Lives The Player Can Have
        lives = Mathf.Clamp(lives, 0, 3);
        #endregion
    }

    #region Function Which Serves As An Extra Method For Scaling The Players Flame

    public void ScalePlayerFlame()
    {
        switch (lives)
        {
            case 3:
                plrFlame.transform.localScale = largeFlameScale;
                break;
            case 2:
                plrFlame.transform.localScale = mediumFlameScale;
                break;
            case 1:
                plrFlame.transform.localScale = smallFlameScale;
                break;
            case 0:
                plrFlame.transform.localScale = deadFlameScale;
                rigidbody.velocity = Vector2.zero;
                inputDir = Vector2.zero;
                canMove = false;
                GameManager.Instance.EndGame(false);
                break;
        }
    }

    #endregion

    #endregion

    #region Function Which Enables The Player To Shoot Fireballs

    private IEnumerator Shooting() 
    {
        canMove = true;
        plrAnim.SetBool("attack", false);

        yield return new WaitUntil(() => shoot.WasPressedThisFrame());

        Instantiate(firePrefab, transform.position, Quaternion.identity);
        canMove = false;
        plrAnim.SetBool("attack", true);
        lives -= 1;
        ScalePlayerFlame();

        yield return new WaitForSeconds(1f);

        plrAnim.SetBool("attack", false);
        canMove = true;

        StartCoroutine(Shooting());
    }

    #endregion

    #region Function Which Allows The Player To Take Damage With SafeFrames
    public IEnumerator SafeFrames()
    {
        canTakeDamage = false;

        yield return new WaitForSeconds(1f);

        canTakeDamage = true;
    }
    #endregion
}
