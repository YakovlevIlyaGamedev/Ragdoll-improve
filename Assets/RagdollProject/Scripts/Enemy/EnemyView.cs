using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyView : MonoBehaviour
{
    private const string IsRunningKey = "IsRunning";

    private Animator _animator;

    public void Initialize() => _animator = GetComponent<Animator>();

    public void StartRunning() => _animator.SetBool(IsRunningKey, true);
    public void StopRunning() => _animator.SetBool(IsRunningKey, false);

    public void DisableAnimator() => _animator.enabled = false;
    public void EnableAnimator() => _animator.enabled = true;

    public Transform GetBone(HumanBodyBones bone) => _animator.GetBoneTransform(bone);
}
