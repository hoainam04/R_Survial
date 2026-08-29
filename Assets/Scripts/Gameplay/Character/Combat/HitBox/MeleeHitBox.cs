// using System;
// using UnityEngine;
// using PROJ.Attributes;
// public class MeleeHitBox : MonoBehaviour
// {
//     [SerializeField] private float damage;
//     [SerializeField] private float knockbackForce = 5f;
//     [SerializeField] private float knockbackDuration = 0.5f;
//     public void InitAtribute(float damage)
//     {
//         damage = this.damage;
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         // Kiểm tra xem đối tượng va chạm có phải là kẻ thù hay không
//         if (other.CompareTag("Enemy"))
//         {
//             // Gây sát thương cho kẻ thù
//             var enemyHealth = other.GetComponent<CharacterAttributeManager>();
//             if (enemyHealth != null)
//             {
//                 enemyHealth.ApplyDamage(damage);
//             }
//         }
//     }
// }