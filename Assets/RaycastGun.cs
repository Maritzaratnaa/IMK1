using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastGun : MonoBehaviour
{
    [Header("Camera & Laser Setup")]
    public Camera playerCamera;
    public Transform laserOrigin;
    public float gunRange = 50f;
    public float fireRate = 0.2f;
    public float laserDuration = 0.05f;

    [Header("Effects & Audio")]
    public GameObject muzzleFlashPrefab; 
    public AudioSource shotAudio;      

    LineRenderer laserLine;
    float fireTimer;

    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
    }

    void Start()
    {
        if (laserLine != null)
        {
            laserLine.enabled = false;
        }
    }

    void Update()
    {
        fireTimer += Time.deltaTime;
        
        if (Input.GetButtonDown("Fire1") && fireTimer > fireRate)
        {
            fireTimer = 0;
            
            if (muzzleFlashPrefab != null)
            {
                GameObject flashInstance = Instantiate(muzzleFlashPrefab, laserOrigin);
                flashInstance.transform.SetParent(laserOrigin);
                Destroy(flashInstance, 0.2f);
            }

            if (shotAudio != null)
            {
                shotAudio.Play();
            }

            laserLine.SetPosition(0, laserOrigin.position);
            
            Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            
            if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, gunRange))
            {
                laserLine.SetPosition(1, hit.point);
                
                if (hit.transform.gameObject.name != "Floor" && hit.transform.gameObject.name != "Wall")
                {
                    if (hit.transform.gameObject.name.Contains("Target1"))
                    {
                        float randomX = Random.Range(-4f, 4f); 
                        float randomY = Random.Range(-0.5f, 1f);    
                        float randomZ = Random.Range(2f, 5f);   
                        
                        hit.transform.position = new Vector3(randomX, randomY, randomZ);
                    }
                    else
                    {
                        Destroy(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (playerCamera.transform.forward * gunRange));
            }
            
            StartCoroutine(ShootLaser());
        }
    }

    IEnumerator ShootLaser()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }
}