using UnityEngine;

namespace Assets.Code
{
    public interface ICanDig
    {
        Color MyColor { get; }

        Transform Transform { get; }

        float DigProgress { get; }

        void Dig();
    }
}
