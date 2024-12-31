using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Running, Jumped, Death, Slide, HitByObs, Hit, Finished, RollJump, MoveSkate,SpwanSkate, Skate_L_to_R, Skate_R_to_L, Skate_Jump, Skate_Slide, Jet_Move
}


public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private Vector3 targetPosition;
    private float swipeThreshold = 50f; 
    private bool touchMoved;
    private int distancePerUnit = 0;
    private Rigidbody rb;
    private Animator animator;
    private int rollJumpAfter = 3;
    private int didJump = 0;
    public float moveSpeed = 10f;
    public float distanceBetweenLane = 4.69f;
    public float currentLane = 1f;
    public float shiftSpeed = 10f;
    public PlayerState playerState;
    private Skateboard skateBoard;
    public float maxHeightCanFly = 10f;
    public float moveSpeedToUp = 1.5f;



    void Start()
    {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
        InvokeRepeating("distanceCalculate", 0, 1 / moveSpeed);
        skateBoard = transform.GetChild(5).GetComponent<Skateboard>();

        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()

    {
        SwipeControl();


        Vector3 newPosition = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, shiftSpeed * Time.deltaTime);
        //rb.velocity = newPosition * moveSpeed;
        SwitchAnimations();

        if (transform.GetChild(6).gameObject.activeSelf)
        {
            if (transform.position.y <= maxHeightCanFly)
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z), shiftSpeed * Time.deltaTime);
            //var yValue = (transform.position.y + moveSpeedToUp) * Time.deltaTime;
            //float clamp = Mathf.Clamp(yValue, 0, maxHeightCanFly);

            //if (transform.position.y <= maxHeightCanFly)
            //{
            //    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z), shiftSpeed * Time.deltaTime) ;
            //}
        }
        


    }

 
    void SwipeControl()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    touchMoved = false;
                    break;

                case TouchPhase.Moved:
                    if (!touchMoved)
                    {
                        currentTouchPosition = touch.position;
                        Vector2 distance = currentTouchPosition - startTouchPosition;

                        if (Mathf.Abs(distance.x) > swipeThreshold)
                        {
                            if (distance.x > 0)
                                OnSwipeRight();
                            else
                                OnSwipeLeft();

                            touchMoved = true;
                        }
                        // Check vertical swipe (up or down)
                        else if (Mathf.Abs(distance.y) > swipeThreshold && Mathf.Abs(distance.y) > Mathf.Abs(distance.x))
                        {
                            if (distance.y < 0)
                                OnSwipeDown(); // Swipe down (bottom swipe)
                           
                            else
                                OnSwipeUp(); // Swipe up

                            touchMoved = true;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    touchMoved = false;
                    break;
            }
        }
    }
    void OnSwipeDown() {

        if (transform.GetChild(5).gameObject.activeSelf)
        {
            playerState = PlayerState.Skate_Slide;
            skateBoard.skateboardState = SkateboardState.Slide;

        }
        else
        {
            playerState = PlayerState.Slide;
        }
        

    }

        void OnSwipeUp()
    {
        if (transform.GetChild(5).gameObject.activeSelf)
        {
            playerState = PlayerState.Skate_Jump;
            skateBoard.skateboardState = SkateboardState.Jump;
        }
        else
        {
            if (didJump == rollJumpAfter)
            {
                didJump = 0;
                playerState = PlayerState.RollJump;

            }
            else
            {
                didJump += 1;
                playerState = PlayerState.Jumped;
            }
        }
       
       
    }


    void OnSwipeRight()
    {
        // Move the player to the right
        currentLane++;
        if (currentLane == 2)
        {
            currentLane = 1;
        }
        targetPosition = new Vector3(currentLane * distanceBetweenLane, transform.position.y, transform.position.z);

        if (transform.GetChild(5).gameObject.activeSelf)
        {
            playerState = PlayerState.Skate_L_to_R;
            skateBoard.skateboardState = SkateboardState.Right;

        }

    }

    void OnSwipeLeft()
    {
        // Move the player to the left

        currentLane--;
        if (currentLane == -2)
        {
            currentLane = -1;
        }
        targetPosition = new Vector3(currentLane * distanceBetweenLane, transform.position.y, transform.position.z);
        if (transform.GetChild(5).gameObject.activeSelf)
        {
            playerState = PlayerState.Skate_R_to_L;
            skateBoard.skateboardState = SkateboardState.Left;
        }
     

    }

    void SwitchAnimations() {

        switch (playerState) {
            case PlayerState.Running: 
                animator.SetBool("run", true);
                animator.SetBool("slide", false);
                animator.SetBool("jump", false);
                animator.SetBool("roll_jump", false);
                animator.SetBool("move_skate", false);
                animator.SetBool("jet_move", false);
                break;

            case PlayerState.Slide:

                animator.SetBool("run", false);
                animator.SetBool("slide", true);
                startRunAgain("Slide");
                break;
            case PlayerState.Jumped:
                animator.SetBool("jump", true);
                animator.SetBool("run", false);
                startRunAgain("Jump");
                break;
            case PlayerState.RollJump:
                animator.SetBool("roll_jump", true);
                animator.SetBool("run", false);
                startRunAgain("Roll_Jump");
                break;
            case PlayerState.MoveSkate:
                animator.SetBool("move_skate", true);
                animator.SetBool("skate_spwan", false);
                animator.SetBool("skate_L_to_R", false);
                animator.SetBool("skate_R_to_L", false);
                animator.SetBool("skate_jump", false);
                animator.SetBool("skate_slide", false);
                break;
            case PlayerState.SpwanSkate:
                animator.SetBool("skate_spwan", true);
                animator.SetBool("run", false);
                break;
            case PlayerState.Skate_L_to_R:
                animator.SetBool("skate_L_to_R", true);
                animator.SetBool("move_skate", false);
                StartCoroutine(startSkateMoveAgain());
                break;
            case PlayerState.Skate_R_to_L:
                animator.SetBool("skate_R_to_L", true);
                animator.SetBool("move_skate", false);
                StartCoroutine(startSkateMoveAgain());
                break;
            case PlayerState.Skate_Jump:
                animator.SetBool("skate_jump", true);
                animator.SetBool("move_skate", false);
                StartCoroutine(startSkateMoveAgain());
                break;
            case PlayerState.Skate_Slide:
                animator.SetBool("skate_slide", true);
                animator.SetBool("move_skate", false);
                StartCoroutine(startSkateMoveAgain());
                break;
            case PlayerState.Jet_Move:
                animator.SetBool("jet_move", true);
                animator.SetBool("run", false);
                //StartCoroutine(startRunAgain());
                break;
            default:
                break; 
        }

    }

   

    void distanceCalculate()
    {
        distancePerUnit = distancePerUnit + 1;
        GameManager.instance.distance = distancePerUnit;

    }
    private void OnTriggerEnter(Collider other)
    {

     
        if (other.gameObject.CompareTag(Constants.Skateboard))
        {

            Destroy(other.gameObject);
            transform.GetChild(5).gameObject.SetActive(true);
            transform.GetChild(7).gameObject.SetActive(true);
            StartCoroutine(disableSmoke());

        }
        if (other.gameObject.CompareTag(Constants.Jet))
        {

            Destroy(other.gameObject);
            transform.GetChild(6).gameObject.SetActive(true);
            transform.GetChild(7).gameObject.SetActive(true);
            StartCoroutine(disableSmoke());
            playerState = PlayerState.Jet_Move;
        }

    }

    IEnumerator disableSmoke()
    {
        yield return new WaitForSeconds(0.5f);
        transform.GetChild(7).gameObject.SetActive(false);
    }

    void startRunAgain(string name)
    {

        //yield return new WaitForSeconds(1f);
        // Get the current state information from the Animator
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
     
        // Check if the current animation is the first animation and has finished playing
        if (stateInfo.IsName(name) && stateInfo.normalizedTime >= 0.8f)
        {
            // Play the next animation
            playerState = PlayerState.Running;
        }




        
    }

    IEnumerator startSkateMoveAgain()
    {

        yield return new WaitForSeconds(0.8f);
        //AnimationStateInfo state = animator.GetCurrentAnimatorStateInfo(0)
        playerState = PlayerState.MoveSkate;
    }




    private void OnDisable()
    {
        CancelInvoke();

    }




}
