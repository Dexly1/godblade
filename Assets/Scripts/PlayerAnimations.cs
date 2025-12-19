using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public static PlayerAnimations single;
    public Animation _animator;
    public AnimationType _animationType;
    public string[] _namesOfAnimations;

    public enum AnimationType
    {
        Idle,
        Run,
        Attack
    }

    private void Awake()
    {
        single = this;
        ChangeAnimation(AnimationType.Idle);
    }

    public void ChangeAnimation(AnimationType at, string addName = "")
    {
        _animationType = at;

        switch (_animationType)
        {
            case AnimationType.Idle:
                _animator.Play(_namesOfAnimations[0]);
                break;
            case AnimationType.Run:
                _animator.Play(_namesOfAnimations[1]);
                break;
            case AnimationType.Attack:
                _animator.Play(_namesOfAnimations[2]);
                //_animator.SetTrigger(_namesOfAnimations[2]+addName);
                break;
        }

        //Debug.Log(at);
    }
}
