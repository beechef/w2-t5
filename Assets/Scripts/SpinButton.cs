using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinButton : MonoBehaviour
{
    SpinnerController spinnerController;
    void Start()
    {
        spinnerController = transform.parent.GetComponent<SpinnerController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Click();
            }
        }
    }
    public void Click()
    {
        spinnerController.StartSpin();
    }
}
