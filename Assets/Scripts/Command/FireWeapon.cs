using UnityEngine;

public class FireWeapon : Command
{
    public override void Execute()
    {
        Gun gun = Character.Gun;

        if (gun.CurrentAmmo > 0 && gun.IsCanShoot == true && gun.Weapon.IsAttak == true)
        {
            for (int i = gun.Barrels.Count - 1; i >= 0; i--)
            {
                gun.CurrentAmmo--;
                GameObject Bullet = ObjectPooler.SharedInstance.GetPooledObject("bullet");

                if (Bullet != null)
                {
                    Bullet.transform.position = gun.Barrels[i].transform.position;
                    Bullet.transform.rotation = gun.Barrels[i].transform.rotation;
                    Bullet.SetActive(true);
                    Bullet.GetComponent<Bullet>().Weapon = gun.gameObject.GetComponent<Gun>().Weapon;
                    gun.mUIManager.AmmoBar.BarImages[gun.CurrentAmmo].color = Color.grey;
                }
            }
        }
        else
        {
            gun.IsCanShoot = false;
        }
    }
}