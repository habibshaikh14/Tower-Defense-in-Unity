using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

	private Transform target;
    private Enemy targetEnemy;

	[Header("General")]
	public float range = 15f;

    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 2f;
	private float fireCountdown = 0f;

    [Header("Use Laser")]
    public bool useLaser = false;

    public int damageOverTime = 20;
    public float slowRate = 0.5f;

    public LineRenderer lineRenderer;
    public ParticleSystem impact;
    public Light laserLight;

    [Header("Unity Setup Fields")]
	public string enemyTag = "Enemy";

	public Transform partToRotate;
	public float turnSpeed = 20f;

	public Transform firePoint;

	// Use this for initialization
	void Start () {
		InvokeRepeating("UpdateTarget", 0f, 0.5f);
	}

	void UpdateTarget ()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;

		foreach(GameObject enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
			if(distanceToEnemy < shortestDistance)
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy;
			}
		}

		if(nearestEnemy != null && shortestDistance <= range)
		{
			target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
		} else 
		{
			target = null;
		}
	}
	
	// Update is called once per frame
	void Update () {
        if (target == null)
        {
            if(useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impact.Stop();
                    laserLight.enabled = false;
                }
            }

            return;
        }
        LockOnTarget();

        if (useLaser)
        {
            LaserStuff();
        }
        else
        {

            if (fireCountdown <= 0)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
	}

    void LockOnTarget ()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void LaserStuff ()
    {
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        targetEnemy.Slow (slowRate);

        if(!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impact.Play();
            laserLight.enabled = true;
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        impact.transform.position = target.position + dir.normalized;

        impact.transform.rotation = Quaternion.LookRotation(dir);
    }

	void Shoot ()
	{
		GameObject bulletGO =  (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = bulletGO.GetComponent<Bullet>();

		if(bulletGO != null)
		{
			bullet.Seek(target);
		}
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}
