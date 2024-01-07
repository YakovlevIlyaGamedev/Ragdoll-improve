using System;
using System.Collections;
using UnityEngine;

public class RigStandUpHandler: MonoBehaviour
{
    private const float _timeToShiftBonesToStandingUpAnimation = 0.5f;

    [SerializeField] private AnimationClip _backStandingUpClip;
    [SerializeField] private AnimationClip _frontStandingUpClip;

    private Transform _parent;
    private Transform _hipsBone;

    private Transform[] _bones;
    private BoneTransformData[] _bonesBeforeStandingUpAnimation;
    private BoneTransformData[] _bonesAtStartBackStandingUpAnimation;
    private BoneTransformData[] _bonesAtStartFrontStandingUpAnimation;

    private Coroutine _shiftBonesToStandingUpAnimation;

    public bool IsFrontUp => Vector3.Dot(_hipsBone.up, Vector3.up) > 0;

    public void Initialize(Transform parent, Transform hipsBone)
    {
        _parent = parent;
        _hipsBone = hipsBone;

        _bones = _hipsBone.GetComponentsInChildren<Transform>();
        _bonesBeforeStandingUpAnimation = new BoneTransformData[_bones.Length];
        _bonesAtStartBackStandingUpAnimation = new BoneTransformData[_bones.Length];
        _bonesAtStartFrontStandingUpAnimation = new BoneTransformData[_bones.Length];

        for (int i = 0; i < _bones.Length; i++)
        {
            _bonesBeforeStandingUpAnimation[i] = new BoneTransformData();
            _bonesAtStartBackStandingUpAnimation[i] = new BoneTransformData();
            _bonesAtStartFrontStandingUpAnimation[i] = new BoneTransformData();
        }

        SaveBonesDataFromStartStandingUpAnimation(_backStandingUpClip, _bonesAtStartBackStandingUpAnimation);
        SaveBonesDataFromStartStandingUpAnimation(_frontStandingUpClip, _bonesAtStartFrontStandingUpAnimation);
    }

    public void AdjustParentTransformToRagdoll(Action callback)
    {
        AdjustParentRotationToRagdoll();
        AdjustParentPositionToRagdoll();

        SaveCurrentBonesTransformDataTo(_bonesBeforeStandingUpAnimation);

        if (_shiftBonesToStandingUpAnimation != null)
            StopCoroutine(_shiftBonesToStandingUpAnimation);

        BoneTransformData[] targetBones = GetBonesAtStartStandingUpAnimation();

        _shiftBonesToStandingUpAnimation = StartCoroutine(ShiftBonesToStandingUpAnimation(callback, targetBones));
    }

    private IEnumerator ShiftBonesToStandingUpAnimation(Action callback, BoneTransformData[] targetBones)
    {
        float progress = 0;

        while (progress < _timeToShiftBonesToStandingUpAnimation)
        {
            progress += Time.deltaTime;
            float progressInPercantage = progress / _timeToShiftBonesToStandingUpAnimation;

            for (int i = 0; i < _bones.Length; i++)
            {
                _bones[i].localPosition = Vector3.Lerp(_bonesBeforeStandingUpAnimation[i].Position, targetBones[i].Position, progressInPercantage);
                _bones[i].localRotation = Quaternion.Lerp(_bonesBeforeStandingUpAnimation[i].Rotation, targetBones[i].Rotation, progressInPercantage);
            }

            yield return null;
        }

        callback?.Invoke();
    }

    private void AdjustParentPositionToRagdoll()
    {
        Vector3 initHipsPosition = _hipsBone.position;
        _parent.position = initHipsPosition;

        Vector3 positionOffset = GetBonesAtStartStandingUpAnimation()[0].Position;
        positionOffset.y = 0;
        _parent.position -= _parent.transform.rotation * positionOffset;

        AdjustParentPositionRelativeGround();

        _hipsBone.position = initHipsPosition;
    }

    private BoneTransformData[] GetBonesAtStartStandingUpAnimation() => IsFrontUp ? _bonesAtStartFrontStandingUpAnimation : _bonesAtStartBackStandingUpAnimation;

    private void SaveCurrentBonesTransformDataTo(BoneTransformData[] bones)
    {
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].Position = _bones[i].localPosition;
            bones[i].Rotation = _bones[i].localRotation;
        }
    }

    private void SaveBonesDataFromStartStandingUpAnimation(AnimationClip clip, BoneTransformData[] bones)
    {
        Vector3 initPosition = transform.position;
        Quaternion initRotation = transform.rotation;

        clip.SampleAnimation(gameObject, 0);
        SaveCurrentBonesTransformDataTo(bones);

        transform.position = initPosition;
        transform.rotation = initRotation;
    }

    //Убрать слой в константу
    private void AdjustParentPositionRelativeGround()
    {
        if (Physics.Raycast(_parent.position, Vector3.down, out RaycastHit hit, 5, 1 << LayerMask.NameToLayer("Ground")))
            _parent.position = new Vector3(_parent.position.x, hit.point.y, _parent.position.z);
    }

    //скорректировать
    private void AdjustParentRotationToRagdoll()
    {
        Vector3 initHipsPosition = _hipsBone.position;
        Quaternion initHipsRotation = _hipsBone.rotation;

        Vector3 directionForRotate = _hipsBone.up;

        if (IsFrontUp == false)
            directionForRotate *= -1;

        directionForRotate.y = 0;

        Quaternion correctionRotation = Quaternion.FromToRotation(_parent.forward, directionForRotate.normalized);
        _parent.rotation *= correctionRotation;

        _hipsBone.position = initHipsPosition;
        _hipsBone.rotation = initHipsRotation;
    }
}
