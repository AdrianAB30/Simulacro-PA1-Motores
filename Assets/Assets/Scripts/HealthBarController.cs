using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private int maxValue;
    [Header("Health Bar Visual Components")] 
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private RectTransform modifiedBar;
    [SerializeField] private float changeSpeed;

    private int currentValue;
    private float _fullWidth;
    private float TargetWidth => currentValue * _fullWidth / maxValue;
    private Coroutine updateHealthBarCoroutine;

    private void Start() {
        currentValue = maxValue;
        _fullWidth = healthBar.rect.width;
    }

    /// <summary>
    /// Metodo <c>UpdateHealth</c> actualiza la vida del personaje de manera visual. Recibe una cantidad de vida modificada.
    /// </summary>
    /// <param name="amount">El valor de vida modificada.</param>
    public void UpdateHealth(int amount){
        currentValue = Mathf.Clamp(currentValue + amount, 0, maxValue);

        if(updateHealthBarCoroutine != null){
            StopCoroutine(updateHealthBarCoroutine);
        }
        updateHealthBarCoroutine = StartCoroutine(AdjustWidthBar(amount));
    }

    IEnumerator AdjustWidthBar(int amount){
        RectTransform targetBar = amount >= 0 ? modifiedBar : healthBar;
        RectTransform animatedBar = amount >= 0 ? healthBar : modifiedBar;

        targetBar.sizeDelta = SetWidth(targetBar,TargetWidth);

        while(Mathf.Abs(targetBar.rect.width - animatedBar.rect.width) > 1f){
            animatedBar.sizeDelta = SetWidth(animatedBar,Mathf.Lerp(animatedBar.rect.width, TargetWidth, Time.deltaTime * changeSpeed));
            yield return null;
        }

        animatedBar.sizeDelta = SetWidth(animatedBar,TargetWidth);
    }

    private Vector2 SetWidth(RectTransform t, float width){
        return new Vector2(width, t.rect.height);
    }

    private void Update() {

        if(Input.GetMouseButtonDown(0)){
            UpdateHealth(20);
        }else if(Input.GetMouseButtonDown(1)){
            UpdateHealth(-20);
        }
    }
}
