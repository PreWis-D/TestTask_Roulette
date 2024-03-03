using DG.Tweening;
using System;
using UnityEngine;

public class RouletteAnimator
{
    private Transform _pickerWheelCircle;
    private int _spinDuration;
    private float _angle;
    private float _halfPieceAngle;
    private Action _callback;

    public RouletteAnimator(Transform pickerWheelCircle, int duration, float angle, float halfPieceAngle)
    {
        _pickerWheelCircle = pickerWheelCircle;
        _spinDuration = duration;
        _angle = angle;
        _halfPieceAngle = halfPieceAngle;
    }

    public void Spin(int index, Action callback)
    {
        _callback = callback;

        float angle = -(_angle * index);
        Vector3 targetRotation = Vector3.back * (angle + 2 * 360 * _spinDuration);

        float prevAngle, currentAngle;
        prevAngle = currentAngle = _pickerWheelCircle.eulerAngles.z;

        Animate(targetRotation, prevAngle, currentAngle);
    }

    private void Animate(Vector3 targetRotation, float prevAngle, float currentAngle)
    {
        _pickerWheelCircle
            .DORotate(targetRotation, _spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutFlash, 1)
            .OnUpdate(() =>
            {
                float diff = Mathf.Abs(prevAngle - currentAngle);

                if (diff >= _halfPieceAngle)
                    prevAngle = currentAngle;

                currentAngle = _pickerWheelCircle.eulerAngles.z;
            })
            .OnComplete(() =>
            {
                _callback?.Invoke();
                _callback = null;
            });
    }
}