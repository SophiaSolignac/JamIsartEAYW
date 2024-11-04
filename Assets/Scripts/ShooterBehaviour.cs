using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShooterBehaviour : MonoBehaviour
{

    #region StateMachine
    private delegate void Action();
    Action _Action;

    private void SetModeVoid()
    {
        _Action = DoVoid;
    }
    private void DoVoid() {}
    private void SetModeSeek(bool resetAction)
    {
        if (resetAction) { _Action = null; }
        _Action -= DoSeek;
        _Action += DoSeek;
    }
    private void DoSeek()
    {

    }
    private void SetModeOrbit(bool resetAction)
    {
        if (resetAction) { _Action = null; }
        _Action -= DoOrbit; 
        _Action += DoOrbit;
    }
    private void DoOrbit()
    {

    }
    private void SetModeShoot(bool resetAction)
    {
        if (resetAction) { _Action = null; }
        _Action -= DoShoot;
        _Action += DoShoot;
    }
    private void DoShoot()
    {

    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        SetModeVoid();
    }

    // Update is called once per frame
    void Update()
    {
        _Action();
    }
}
