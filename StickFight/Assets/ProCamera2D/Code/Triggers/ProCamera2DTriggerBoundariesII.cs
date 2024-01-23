using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProCamera2DTriggerBoundariesII : ProCamera2DTriggerBoundaries {
    //[SerializeField] private float sizeBonus = 40;
    [SerializeField] private float sizeOrthCamera;
    protected override void EnteredTrigger() {
        base.EnteredTrigger();
        NumericBoundaries.UseNumericBoundaries = true;
        ProcameraController.Instance.SetOrthographic(sizeOrthCamera);
    }

    protected override void ExitedTrigger() {
        base.ExitedTrigger();
        NumericBoundaries.UseNumericBoundaries = false;
        ProcameraController.Instance.SetOrthographic(sizeOrthCamera);
    }

    protected override void OnDisable() {
        base.OnDisable();
        NumericBoundaries.UseNumericBoundaries = false;
    }
}
