using System;
using System.Collections;
using UnityEngine;

namespace ConnectionActions
{
    public class ComponentChangeMaterial : MonoBehaviour, IComponentAction
    {
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        public Material targetMaterial;
        public Texture onValidEventTexture;
        public Texture onInvalidEventTexture;

        public void OnValidConnection() => targetMaterial.SetTexture(MainTex, onValidEventTexture);

        public void OnInvalidConnection() => targetMaterial.SetTexture(MainTex, onInvalidEventTexture);
    }
}