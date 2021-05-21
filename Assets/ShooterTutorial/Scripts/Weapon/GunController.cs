using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopShooter
{
    public class GunController : MonoBehaviour
    {
        public Transform weaponHold;
        public Gun[] allGuns;
        private Gun equippedGun;

        public void EquipGun(Gun newGun)// ciekawe podejście na dwie metody
        {
            if (equippedGun != null)
            {
                Destroy(equippedGun.gameObject);
            }
            equippedGun = Instantiate(newGun, weaponHold.position, weaponHold.rotation) as Gun;
            equippedGun.transform.parent = weaponHold;
        }

        public void EquipGun(int weaponIndex)
        {
            EquipGun(allGuns[weaponIndex]);
        }

        public void OnTriggerHold()
        {
            if (equippedGun != null)
            {
                equippedGun.OnTriggerHold();
            }
        }

        public void OnTriggerRelease()
        {
            if (equippedGun != null)
            {
                equippedGun.OnTriggerRelease();
            }
        }

        public float GunHeight
        {
            get
            {
                return weaponHold.position.y;
            }
        }

        public void Aim(Vector3 aimPoint)
        {
            if (equippedGun != null)
            {
                equippedGun.Aim(aimPoint);
            }
        }

        public void Reload()
        {
            if (equippedGun != null)
            {
                equippedGun.Reload();
            }
        }
    }
}
