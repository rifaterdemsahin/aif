using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyFly : MonoBehaviour {

	public void OnMoveComplete()
    {
        iTween.ScaleTo(gameObject, Vector3.zero, 0.15f);
        Destroy(gameObject, 0.2f);

        CurrencyController.CreditBalance(1);
    }
}
