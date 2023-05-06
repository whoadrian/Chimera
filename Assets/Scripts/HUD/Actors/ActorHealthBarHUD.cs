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
            Color color;
            switch (_actor.Faction)
            {
                case Faction.Red: 
                    color = Color.red;
                    break;
                case Faction.Green: 
                    color = Color.green;
                    break;
                case Faction.Blue: 
                    color = Color.blue;
                    break;
                default:
                    throw new NotImplementedException();
            }

            healthAmountImg.color = color;
        }

        private void Update()
        {
            if (UnityEngine.Camera.main?.transform)
            {
                parent.LookAt(UnityEngine.Camera.main.transform);
            }

            var healthPercentage = _actor.CurrentHealth / _actor.MaxHealth;
            healthAmountImg.fillAmount = healthPercentage;
        }
    }
}