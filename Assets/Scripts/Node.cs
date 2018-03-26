using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Node : MonoBehaviour {

    public Color hoverColor;
    public Color notEnoughMoneyColor;
    public Vector3 positionOffset;

    [HideInInspector]
	public GameObject turret;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public bool isUpgraded = false;

	private Renderer rend;
	private Color startColor;

    BuildManager buildmanager;

	void Start()
	{
		rend = GetComponent<Renderer>();
		startColor = rend.material.color;
        buildmanager = BuildManager.instance;
	}

    public Vector3 GetBuildPosition ()
    {
        return transform.position + positionOffset;
    }

	void OnMouseDown()
	{
        if (EventSystem.current.IsPointerOverGameObject())
            return;

		if(turret != null)
		{
            buildmanager.SelectNode(this);
            return;
		}

        if (!buildmanager.CanBuild)
            return;

        //Build Turret
        BuildTurret(buildmanager.GetTurretToBuild());
	}

    public void BuildTurret (TurretBlueprint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost)
        {
            Debug.Log("Not enough Money");
            return;
        }

        PlayerStats.Money -= blueprint.cost;
        GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;

        turretBlueprint = blueprint;

        GameObject effect = (GameObject)Instantiate(buildmanager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Debug.Log("Turret Build!");
    }

    public void UpgradeTurret ()
    {
        if (PlayerStats.Money < turretBlueprint.upgradeCost)
        {
            Debug.Log("Not enough Money");
            return;
        }

        PlayerStats.Money -= turretBlueprint.upgradeCost;

        //Get Rid Of Old Turret
        Destroy(turret);

        //Building Upgraded turret
        GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;

        GameObject effect = (GameObject)Instantiate(buildmanager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        isUpgraded = true;

        Debug.Log("Turret Upgraded");
    }

    public void SellTurret ()
    {
        PlayerStats.Money += turretBlueprint.GetSellAmount();

        GameObject effect = (GameObject)Instantiate(buildmanager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(turret);
        turretBlueprint = null;
    }

	void OnMouseEnter ()
	{
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!buildmanager.CanBuild)
            return;

        if(buildmanager.HasMoney)
        {
            rend.material.color = hoverColor;
        }else
        {
            rend.material.color = notEnoughMoneyColor;
        }
        
	}

	void OnMouseExit ()
	{
		rend.material.color = startColor;
	}

}
