using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public healthBar healthBar;

    public GameObject deathVFX;
    public LayerMask BossMask;
    public float playerHealth = 200f;
    public GameObject idle;
    bool touchedJombie , touchedSpider;
    public bool playerOn;
    bool availableDamage;
    public LayerMask spiderMask;
    [Header("camera shake")]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeTime;
    public Collider2D col;
    bool touchedBoss;

    [Header("Flash")]
    // The SpriteRenderer that should flash.
    public SpriteRenderer spriteRenderer;

    // The material that was in use, when the script started.
    private Material originalMaterial;

    // The currently running coroutine.
    private Coroutine flashRoutine;

    [Tooltip("Material to switch to during the flash.")]
    [SerializeField] private Material flashMaterial;

    [Tooltip("Duration of the flash.")]
    [SerializeField] private float duration;

    [Space]
    public LayerMask jombieMask;

    public GameObject coinPickupVFX;

    int coroutineOnce = 0;
    //another for spider.. damage value different..
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Missile"))
        {
            TakeDamage(20f); // enemy missile.. 
            
        }

        if (collision.tag == "FinalKey")
        {
            audio_Manager.instance.Play("key");

            Destroy(collision.gameObject);

            Instantiate(coinPickupVFX, collision.transform.position, Quaternion.identity);

            GameManager.instance.FinalKeyCount++;

            UIManager.UpdateFinalKeyUI(1 - GameManager.instance.FinalKeyCount);

            if (coroutineOnce == 0)
            {
                StartCoroutine(FinishGame());
                coroutineOnce = 1;

            }

        }
    }

    IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(1.5f);

        TransitionManager.instance.PlayANimLoad("transition");

        yield return new WaitForSeconds(0.7f);

        //destroy existing game manager..

        SceneManager.LoadScene(4);

        Destroy(GameManager.instance.gameObject);
    }

    private void Start()
    {
        healthBar.SetMaxHealth(playerHealth);
        playerOn = true;
        availableDamage = true;
        // Get the material that the SpriteRenderer uses, 
        // so we can switch back to it after the flash ended.
        originalMaterial = spriteRenderer.material;
    }

    private void Update()
    {
        Damage();
    }
    public void TakeDamage(float damageAmount)
    {
        CinemachineShake.Instance.ShakeCamera(shakeIntensity, shakeTime);

        Flash();

        playerHealth -= damageAmount;
        healthBar.SetHealth(playerHealth);

        if (playerHealth <= 0)
        {
            playerOn = false;

            idle.GetComponent<SpriteRenderer>().enabled = false;
            idle.GetComponent<Animator>().enabled = false;

            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
            col.enabled = false;

            StartCoroutine(HandleDeath());

            audio_Manager.instance.Play("dead");

            CinemachineShake.Instance.ShakeCamera(50, 0.2f);
        }
    }

    IEnumerator HandleDeath()
    {
        GameManager.instance.isGameOver = true;

        GameObject playerVFX = Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(playerVFX, 4f);

        yield return new WaitForSeconds(1.1f);

        TransitionManager.instance.PlayANimLoad("transition");


        yield return new WaitForSeconds(0.7f);



        if (SceneManager.GetActiveScene().buildIndex == 3)
            SceneManager.LoadScene(3);
        else
        {
            Destroy(GameManager.instance.gameObject);
            SceneManager.LoadScene(1);
        }

    }

    public void Flash()
    {
        // If the flashRoutine is not null, then it is currently running.
        if (flashRoutine != null)
        {
            // In this case, we should stop it first.
            // Multiple FlashRoutines the same time would cause bugs.
            StopCoroutine(flashRoutine);
        }

        // Start the Coroutine, and store the reference for it.
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Swap to the flashMaterial.
        spriteRenderer.material = flashMaterial;

        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(duration);

        // After the pause, swap back to the original material.
        spriteRenderer.material = originalMaterial;

        // Set the routine to null, signaling that it's finished.
        flashRoutine = null;
    }

    void Damage()
    {
        touchedJombie = Physics2D.OverlapCircle(transform.position, 1f, jombieMask, Mathf.Infinity * -1, Mathf.Infinity);
        touchedSpider = Physics2D.OverlapCircle(transform.position, 1f, spiderMask, Mathf.Infinity * -1, Mathf.Infinity);
        touchedBoss = Physics2D.OverlapCircle(transform.position, 1f, BossMask, Mathf.Infinity * -1, Mathf.Infinity);
        /*if (touchedJombie)
        {

            // use corouting...
            Debug.Log("touched");
            time += Time.fixedDeltaTime;
            if (time >= 1f)
            {
                i++;
                time = 0;
            }
            if (i % 10 == 0)
            {
                TakeDamage(5f);
                i++;
            }
            if (i >= 10)
            {
                i = 1;
            }
        }*/
        if ((touchedJombie || touchedSpider || touchedBoss)  && availableDamage)
        {
            if (touchedSpider)
            {
                StartCoroutine(TakeDamg(10f));
            }
            if (touchedJombie)
            {
                StartCoroutine(TakeDamg(5f));
            }
            if (touchedBoss)
            {
                StartCoroutine(TakeDamg(80f));
            }
            
        }
        
    }

    IEnumerator TakeDamg(float damage)
    {
        TakeDamage(damage);
        availableDamage = false;
        yield return new WaitForSeconds(1f);
        availableDamage = true;

    }
}
