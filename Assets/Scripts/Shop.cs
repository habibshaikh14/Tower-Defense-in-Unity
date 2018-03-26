using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {

    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;
    public TurretBlueprint laserBeamer;

    BuildManager buildmanager;

    void Start ()
    {
        buildmanager = BuildManager.instance;
    }

	public void SelectStandardTurret ()
    {
        Debug.Log("Standard Turret Selected");
        buildmanager.SelectTurretToBuild(standardTurret);
    }

    public void SelectMissileLauncher()
    {
        Debug.Log("Missile Launcher Selected");
        buildmanager.SelectTurretToBuild(missileLauncher);
    }

    public void SelectLaserBeamer()
    {
        Debug.Log("Laser Beamer Selected");
        buildmanager.SelectTurretToBuild(laserBeamer);
    }
}
