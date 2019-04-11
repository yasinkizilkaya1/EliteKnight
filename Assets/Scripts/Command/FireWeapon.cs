using UnityEngine;

public class FireWeapon : Command
{
    public override void Execute()
    {
        Gun gun = Character.Gun;

        if (gun.CurrentAmmo > 0 && gun.IsCanShoot == true && gun.weapon.IsAttak == true)
        {
            for (int i = gun.BarrelList.Count - 1; i >= 0; i--)
            {
                gun.CurrentAmmo--;
                GameObject Bullet = ObjectPooler.SharedInstance.GetPooledObject("bullet");

                if (Bullet != null)
                {
                    Bullet.transform.position = gun.BarrelList[i].transform.position;
                    Bullet.transform.rotation = gun.BarrelList[i].transform.rotation;
                    Bullet.SetActive(true);
                    Bullet.GetComponent<Bullet>().weapon = gun.gameObject.GetComponent<Gun>().weapon;
                    gun.mUIManager.ammoBar.BarImageList[gun.CurrentAmmo].color = Color.grey;
                }
            }
        }
        else
        {
            gun.IsCanShoot = false;
        }
    }
}