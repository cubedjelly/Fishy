using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_Bar_Minigame : MonoBehaviour
{
    [SerializeField] Transform TopPivot;
    [SerializeField] Transform BottomPivot;

    [SerializeField] Transform fish;

    float fishPosition;
    float fishDestination;

    float fishTimer;
    [SerializeField] float timerMultplicator = 3f;

    float fishSpeed;
    [SerializeField] float smoothMotion = 1f;

    [SerializeField] Transform hook;
    float hookPosition;
    [SerializeField] float hookSize = 0.1f;
    [SerializeField] float hookPower = 0.5f;
    float hookProgress;
    float hookPullVelocity;
    [SerializeField] float hookPullPower = 0.01f;
    [SerializeField] float hookGravityPower = 0.005f;
    [SerializeField] float hookProgressDegradationPower = 0.1f;

    //Abe's MOD
    private Vector3 _1s;
    public UIScript uiScript;

    [SerializeField] SpriteRenderer hookSpriteRenderer;

    [SerializeField] Transform progressBarContainer;

    bool pause = false;

    [SerializeField] float failTimer = 10f;



    private void Start()
    {
        Resize();
    }

   private void Update()
    {
        if(pause) { return; }
        Fish();
        Hook();
        ProgressCheck();
    }


    private void Resize()
    {
        Bounds b = hookSpriteRenderer.bounds;
        float ySize = b.size.y;
        Vector3 _1s = hook.localScale;
        float distance = Vector3.Distance(TopPivot.position, BottomPivot.position);
        _1s.y = (distance / ySize * hookSize);
        hook.localScale = _1s;
    }


    private void ProgressCheck()
    {
        Vector3 _1s = progressBarContainer.localScale;
        _1s.y = hookProgress;
        progressBarContainer.localScale = _1s;

        float min = hookPosition - hookSize / 2;
        float max = hookPosition + hookSize / 2;

        if(min < fishPosition && fishPosition < max)
        {
            hookProgress += hookPower * Time.deltaTime;
        }
        else{
            hookProgress -= hookProgressDegradationPower * Time.deltaTime;
            failTimer -= Time.deltaTime;
            if(failTimer < 0f)
            {
                Lose();
            }
        }
        if(hookProgress >= 1f)
        {
            Win();
        }

        hookProgress = Mathf.Clamp(hookProgress, 0f, 1f);
    }


    private void Lose()
    {
        //pause = true;
        Debug.Log("Fish Lost!");
    }

    private void Win()
    {
        //pause = true;
        Debug.Log("Fish Caught!");
        uiScript.AddPoints(0,2);
        //Load another level
    }

    void Hook()
    {
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
        {
            hookPullVelocity += hookPullPower * Time.deltaTime;
        }
        hookPullVelocity -= hookGravityPower * Time.deltaTime;

        hookPosition += hookPullVelocity;

        if(hookPosition <= 0f && hookPullVelocity < 0f)
        {
            hookPullVelocity = 0f;
        }
        if(hookPosition >= 1f && hookPullVelocity > 0f)
        {
            hookPullVelocity = 0f;
        }

        hookPosition = Mathf.Clamp(hookPosition, hookSize/ 2, 1 - hookSize/2);
        hook.position = Vector3.Lerp(BottomPivot.position, TopPivot.position, hookPosition);
    }

    void Fish()


    {
        fishTimer -= Time.deltaTime;
        if (fishTimer < 0f)
        {
            fishTimer = UnityEngine.Random.value * timerMultplicator;

            fishDestination = UnityEngine.Random.value;
        }
        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        fish.position = Vector3.Lerp(BottomPivot.position, TopPivot.position, fishPosition);
    }
}

