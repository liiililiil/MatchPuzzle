using System.Collections;
using UnityEngine;

public class GameSpeedManager : Managers<GameSpeedManager>
{

    // 전체 속도에 대한 배율
    public const float BASE_GAME_SPEED = 1f;

    // 속도증가 비율
    public const float GAME_SPEED_INCREASE_RATE = 2f;


    //코루틴
    private Coroutine increaseCoroutine;

    private void Update() {
        
        GAME_SPEED = BASE_GAME_SPEED + (GAME_SPEED_VALUE * GAME_SPEED_INCREASE_RATE);
        SetValues(GAME_SPEED);

        // Debug.Log($"Game Speed = {GAME_SPEED} | GameSpeedValue = {GAME_SPEED_VALUE}" );
    }
    

    public void StartSpeedIncrease()
    {
        if (increaseCoroutine != null)
        {
            return;
        }
        
        increaseCoroutine = StartCoroutine(IncreaseGameSpeed());
    }

    public void StopSpeedIncrease()
    {
        if(increaseCoroutine == null)
        {
            return;
        }
        
        CoroutineEnd();
        GAME_SPEED_VALUE = 0;
    }


    IEnumerator IncreaseGameSpeed()
    {
        GAME_SPEED_VALUE = 0;
        while (GAME_SPEED_VALUE <= Utils.GAME_SPEED_MAX)
        {
            yield return null;
            GAME_SPEED_VALUE += 0.1f * Time.deltaTime;
        }

        CoroutineEnd();

    }

    private void CoroutineEnd()
    {
        StopCoroutine(increaseCoroutine);
        increaseCoroutine = null;
    }
    
    private void SetValues(float value)
    {
        MOVEMENT_SPEED = 20f * value;
        EFFECT_DURATION = 15f * value;
        DESTROYER_FORWARD_SPEED = 20f * value;
        DESTROYER_TRAIL_EFFECT_SPAWN_SPEED = 0.5f * value;
        FOCUS_ANIMATION_SPEED = 10f * value;
        FOCUS_MOVE_SPEED = 10f * value;
        SHOOTER_SCALE_SPEED = 1f * value;
        SHOOTER_CHILD_MOVE_SPEED = 1f * value;
    }
    
    //속도증가 값
    [SerializeField]
    private static float GAME_SPEED_VALUE;

    // 속도 증가
    public static float GAME_SPEED { get; private set; } = BASE_GAME_SPEED + (GAME_SPEED_VALUE * GAME_SPEED_INCREASE_RATE);


    public static float MOVEMENT_SPEED {get; private set;} = 20f * GAME_SPEED;
    public static float EFFECT_DURATION {get; private set;} = 15f * GAME_SPEED;
    public static float DESTROYER_FORWARD_SPEED {get; private set;} = 20f * GAME_SPEED;
    public static float DESTROYER_TRAIL_EFFECT_SPAWN_SPEED {get; private set;} = 0.5f * GAME_SPEED;
    public static float FOCUS_ANIMATION_SPEED {get; private set;} = 10f * GAME_SPEED;
    public static float FOCUS_MOVE_SPEED {get; private set;} = 10f * GAME_SPEED;
    public static float SHOOTER_SCALE_SPEED {get; private set;} = 1f * GAME_SPEED;
    public static float SHOOTER_CHILD_MOVE_SPEED {get; private set;} = 1f * GAME_SPEED;
}
