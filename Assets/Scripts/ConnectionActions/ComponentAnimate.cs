using UnityEngine;

namespace ConnectionActions
{
    public class ComponentAnimate : MonoBehaviour, IComponentAction
    {
        private static readonly int RunAnimation = Animator.StringToHash("RunAnimation");
        public Animator animator;
        public void OnValidConnection() => animator.SetBool(RunAnimation, true);

        public void OnInvalidConnection() => animator.SetBool(RunAnimation, false);
    }
}
