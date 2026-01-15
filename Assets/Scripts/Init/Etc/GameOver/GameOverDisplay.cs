using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class GameOverDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject barPrefab;
    
    [SerializeField]
    private GameObject content;

    [SerializeField]
    private GameObject cause;
    
    [SerializeField]
    private Texture[] textures;

    private Coroutine animationCoroutine;



    public void DisplayedGameOver(string causeString, TileRecode tileRecode)
    {
        //사유 표시
        cause.GetComponent<Text>().text = causeString;
        
        //표시될 개수 카운트
        int count = 0;
        for( int i = 0; i < tileRecode.getSize(); i++)
        {
            int value = tileRecode.GetRecord((TileType)i);

            if(value <= 0) continue;

            //슬라이드 할 수있게 콘텐츠 오브젝트에 종속
            GameObject bar = Instantiate(barPrefab, content.transform);

            RectTransform rectTransform = bar.GetComponent<RectTransform>();

            //위치 조정
            rectTransform.anchoredPosition = new Vector2(0, (-Utils.FAIL_BAR_Y_SIZE /2) + (-Utils.FAIL_BAR_Y_SIZE * count) + (-5 * count) + (-5));

            //바 표시 하기
            bar.GetComponent<Bar>().Bind(textures[i], value);

            count++;
        }

        //사이즈를 바 갯수에 맞게 조정
        RectTransform contentRect = content.GetComponent<RectTransform>();
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, (count * Utils.FAIL_BAR_Y_SIZE) + (count * 5) + 10);

        //표시 및 애니메이션
        animationCoroutine = StartCoroutine(Animation(new Vector2(1100,0), new Vector2(0,0), 1f, EaseType.OutCirc));
    }

    IEnumerator Animation(Vector2 start, Vector2 end, float t, EaseType easeType)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        float time = 0;

        while(time <= t)
        {
            Debug.Log(time);
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, EaseMoveMent.Ease(easeType, time));
            time += Time.fixedDeltaTime;

            yield return null;
        }

        //교정 
        rectTransform.anchoredPosition = end;

        coroutineEnd(ref animationCoroutine);


    }

    private void coroutineEnd(ref Coroutine coroutine)
    {
        StopCoroutine(coroutine);
        coroutine = null;
    }
}
