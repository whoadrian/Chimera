using System;
using Chimera.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace Chimera
{
    /// <summary>
    /// Manages the health bar above actors. Needs to be a scene child of an actor component.
    /// </summary>
    public class ActorHealthBarHUD : MonoBehaviour
    {
        // Children components
        public RectTransform parent;
        public Image healthAmountImg;

        private Actor _actor;

        private void Awake()
        {
            _actor = transform.parent.GetComponent<Actor>();
        }

        private void Start()
        {
            // Set health bar color based on actor faction
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
            // Rotate towards camera
            var cameraTransform = UnityEngine.Camera.main?.transform;
            if (cameraTransform)
            {
                parent.rotation = cameraTransform.rotation * Quaternion.Euler(Vector3.up * 180);
            }

            // Set health bar size based on actor's health percentage
            var scale = healthAmountImg.transform.localScale;
            scale.x = _actor.CurrentHealth / _actor.MaxHealth;
            healthAmountImg.transform.localScale = scale;
        }
    }
}