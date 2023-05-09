using System;
using Chimera.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace Chimera
{
    public class ActorHealthBarHUD : MonoBehaviour
    {
        public RectTransform parent;
        public Image healthAmountImg;

        private Actor _actor;

        private void Awake()
        {
            _actor = transform.parent.GetComponent<Actor>();
        }

        private void Start()
        {
            var color = _actor.Faction switch
            {
                Faction.Red => Color.red,
                Faction.Green => Color.green,
                Faction.Blue => Color.blue,
                _ => throw new NotImplementedException()
            };

            healthAmountImg.color = color;
        }

        private void Update()
        {
            var cameraTransform = UnityEngine.Camera.main?.transform;
            if (cameraTransform)
            {
                parent.rotation = cameraTransform.rotation * Quaternion.Euler(Vector3.up * 180);
            }

            var scale = healthAmountImg.transform.localScale;
            scale.x = _actor.CurrentHealth / _actor.MaxHealth;
            healthAmountImg.transform.localScale = scale;
        }
    }
}