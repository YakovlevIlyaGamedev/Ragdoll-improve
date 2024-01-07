using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigAdjusterForAnimation
{
    private const float _timeToShiftBonesToStartAnimation = 0.5f;

    private AnimationClip _clip;

    private MonoBehaviour _view;

    private List<Transform> _bones;
    private BoneTransformData[] _bonesBeforeAnimation;
    private BoneTransformData[] _bonesAtStartAnimation;

    private Coroutine _shiftBonesToStandingUpAnimation;

    public RigAdjusterForAnimation(AnimationClip clip, IEnumerable<Transform> bones, MonoBehaviour view)
    {
        _clip = clip;
        _view = view;
        _bones = new List<Transform>(bones);

        _bonesBeforeAnimation = new BoneTransformData[_bones.Count];
        _bonesAtStartAnimation = new BoneTransformData[_bones.Count];

        for (int i = 0; i < _bones.Count; i++)
        {
            _bonesBeforeAnimation[i] = new BoneTransformData();
            _bonesAtStartAnimation[i] = new BoneTransformData();
        }

        SaveBonesDataFromStartAnimation();
    }

    public void Adjust(Action callback)
    {
        SaveCurrentBonesDataTo(_bonesBeforeAnimation);

        if (_shiftBonesToStandingUpAnimation != null)
            _view.StopCoroutine(_shiftBonesToStandingUpAnimation);

        _shiftBonesToStandingUpAnimation = _view.StartCoroutine(ShiftBonesToAnimation(callback));
    }

    private IEnumerator ShiftBonesToAnimation(Action callback)
    {
        float progress = 0;

        while (progress < _timeToShiftBonesToStartAnimation)
        {
            progress += Time.deltaTime;
            float progressInPercantage = progress / _timeToShiftBonesToStartAnimation;

            for (int i = 0; i < _bones.Count; i++)
            {
                _bones[i].localPosition = Vector3.Lerp(_bonesBeforeAnimation[i].Position, _bonesAtStartAnimation[i].Position, progressInPercantage);
                _bones[i].localRotation = Quaternion.Lerp(_bonesBeforeAnimation[i].Rotation, _bonesAtStartAnimation[i].Rotation, progressInPercantage);
            }

            yield return null;
        }

        callback?.Invoke();
    }

    private void SaveCurrentBonesDataTo(BoneTransformData[] bones)
    {
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].Position = _bones[i].localPosition;
            bones[i].Rotation = _bones[i].localRotation;
        }
    }

    private void SaveBonesDataFromStartAnimation()
    {
        Vector3 initPosition = _view.transform.position;
        Quaternion initRotation = _view.transform.rotation;

        _clip.SampleAnimation(_view.gameObject, 0);
        SaveCurrentBonesDataTo(_bonesAtStartAnimation);

        _view.transform.position = initPosition;
        _view.transform.rotation = initRotation;
    }
}
