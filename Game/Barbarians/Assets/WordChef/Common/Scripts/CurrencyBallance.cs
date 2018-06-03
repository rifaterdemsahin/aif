using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrencyBallance : MonoBehaviour {
    private void Start()
    {
        UpdateBalance();
        CurrencyController.onBalanceChanged += OnBalanceChanged;
    }

    private void UpdateBalance()
    {
        gameObject.SetText(CurrencyController.GetBalance().ToString());
    }

    private void OnBalanceChanged()
    {
        UpdateBalance();
    }

    private void OnDestroy()
    {
        CurrencyController.onBalanceChanged -= OnBalanceChanged;
    }
}
