using UnityEngine;

public class FireWeapon : Command
{
    public override void Execute()
    {
        if (Character.Gun != null)
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
                        Bullet.GetComponent<Bullet>().Weapon = gun.gameObject.GetComponent<Gun>().Weapon;
                        gun.mUIManager.AmmoBar.BarImages[gun.CurrentAmmo].color = Color.grey;
                        Bullet.transform.position = gun.Barrels[i].transform.position;
                        Bullet.transform.rotation = gun.Barrels[i].transform.rotation;
                        Bullet.SetActive(true);
                    }
                }
            }
            else
            {
                gun.IsCanShoot = false;
            }
        }
        else
        {
            Character.knife.IsAttack = true;
        }
    }
}