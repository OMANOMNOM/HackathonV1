using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Audio;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float MaxSpeed;
    public float SprintSpeed;
    public bool OnGround = true;
    public bool DoubleJumpAvailable = true;
    public bool isSprinting = false;
    private bool Changed = false;
    public bool isSliding = false;
    public float SlideForce;
    public float JumpForce;
    private Rigidbody rb;

    [Header("Health")]
    public int Health;

    [Header("Spawn")]
    public GameObject Spawn;

    [Header("Camera")]
    public CameraController Cam;

    [Header("Settings")]
    public GameObject PauseObjects;

    [Header("Audio")]
    public AudioSource SFXAudioSource;
    public float volume;

    [Header("Audioclips")]
    public AudioClip JumpSFX;
    public AudioClip DoubleJumpSFX;

    [Header("Particles")]
    public ParticleSystem particles;

    [Header("Toxic")]
    public bool IsInToxic = false;
    public float HealthTick;
    public int ToxicDamage;
    public bool InCourotine = false;

    [Header("Radiation")]
    public bool isRadiated = false;
    public float TimeUntilRadiated;
    public int Rads;

    [Header("Dead Objects")]
    public GameObject DeadObjects;

    [Header("Win Objects")]
    public GameObject WinObjects;

    [Header("Alter Post Processing")]
    public AlterPost ap;

    public void Awake()
    {
        Time.timeScale = 1;
    }

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Movement

        float Horizontal = Input.GetAxis("Horizontal") * MaxSpeed * Time.deltaTime;
        float Vertical = Input.GetAxis("Vertical") * MaxSpeed * Time.deltaTime;

        Vector3 Movement = Cam.transform.right * Horizontal + Cam.transform.forward * Vertical;
        Movement.y = 0f;

        if (isSprinting)
        {
            rb.AddForce(Movement * SprintSpeed, ForceMode.Impulse);

            Vector2 v2 = new Vector2(rb.velocity.x, rb.velocity.z);

            Vector3 ClampedSpeed = Vector3.ClampMagnitude(v2, SprintSpeed);

            rb.velocity = new Vector3(ClampedSpeed.x, rb.velocity.y, ClampedSpeed.y);
        }

        else
        {
            rb.AddForce(Movement * MaxSpeed, ForceMode.Impulse);

            Vector2 v2 = new Vector2(rb.velocity.x, rb.velocity.z);

            Vector3 ClampedSpeed = Vector3.ClampMagnitude(v2, MaxSpeed);

            rb.velocity = new Vector3(ClampedSpeed.x, rb.velocity.y, ClampedSpeed.y);
        }

        OnGround = Physics.Raycast((new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1f, gameObject.transform.position.z)), Vector3.down, 2f, 1 << LayerMask.NameToLayer("Floor"));
        // raycast down to look for ground is not detecting ground? only works if allowing jump when grounded = false; // return "Ground" layer as layer

        if (Movement.magnitude != 0)
        {
            //SFXAudioSource.Play() -> play walk moving sfx;
        }

        if (Movement.magnitude == 0)
        {
            //SFXAudioSource.Stop() -> stop walk moving sfx;
        }

        // Rotate Player

        Quaternion CamRotation = Cam.rotation;
        CamRotation.x = 0f;
        CamRotation.z = 0f;

        transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);

        if(Changed)
        {
            StartCoroutine(WaitUntilGrounded());
        }
    }
    void Update()
    {
        //double jump
        if(OnGround)
        {
            DoubleJumpAvailable = true;
        }

        // Controls

        //sprint

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isSprinting == true)
            {
                isSprinting = false;
            }
            else
            {
                isSprinting = true;
            }
        }

        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            Changed = true;
            if(OnGround && isSprinting == false)
            {
                isSprinting = true;
                Changed = false;
            }
            if (OnGround && isSprinting == true)
            {
                isSprinting = false;
                Changed = false;
            }
            else if (OnGround == false)
            {
                StartCoroutine(WaitUntilGrounded());
            }
        }

        if(Health <= 0)
        {
            DeadObjectsDisplay();
        }

        //jump

        if (Input.GetButtonDown("Jump") && OnGround)
        {
            Jump();
        }

        if(Input.GetButtonDown("Jump") && !OnGround && DoubleJumpAvailable)
        {
            DoubleJumpAvailable = false;
            Jump();
        }

        //slide

        if(Input.GetKeyDown(KeyCode.LeftControl) && isSliding == false)
        {
            isSliding = true;

            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x/2, gameObject.transform.localScale.y/2, gameObject.transform.localScale.z/2);
            rb.velocity += transform.forward * SlideForce;
        }

        if(Input.GetKeyUp(KeyCode.LeftControl) && isSliding == true)
        {
            isSliding = false;

            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x*2, gameObject.transform.localScale.y*2, gameObject.transform.localScale.z*2);
        }

        // Toggles

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseDisplay();
        }

        //radiation


        if (IsInToxic && TimeUntilRadiated >= 0)
        {
            TimeUntilRadiated -= Time.deltaTime;
        }

        if (TimeUntilRadiated <= 0)
        {
            isRadiated = true;
        }

        if (isRadiated == true)
        {
            ap.Radiated();
            if (InCourotine == false)
            {
                StartCoroutine(HealthMinus());
            }
        }
        else
        {
            ap.NotRadiated();
        }
    }

    //gameplay functions

    IEnumerator WaitUntilGrounded()
    {
        if (OnGround == true && isSprinting == false)
        {
            Changed = false;
            isSprinting = true;
            yield return null;
        }

        if (OnGround == true && isSprinting == true)
        {
            Changed = false;
            isSprinting = false;
            yield return null;
        }
    }

    public void Jump()
    {
        rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);

        if (DoubleJumpAvailable == false)
        {
            PlayDoubleJumpSound();
        }
        else
        {
            PlayJumpSound();
        }
    }

    //toggles

    public void PauseDisplay()
    {
        if (PauseObjects.activeInHierarchy)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            PauseObjects.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            PauseObjects.SetActive(true);
        }
    }

    public void DeadObjectsDisplay()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        DeadObjects.SetActive(true);
    }

    public void WinObjectsDisplay()
    {
        if (WinObjects.activeInHierarchy)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            WinObjects.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            WinObjects.SetActive(true);
        }
    }


    private void PlayJumpSound()
    {
        //SFXAudioSource.PlayOneShot(JumpSFX, volume);
    }

    private void PlayDoubleJumpSound()
    {
        //SFXAudioSource.PlayOneShot(DoubleJumpSFX, volume);
    }

    IEnumerator HealthMinus()
    {
        while (true)
        {
            InCourotine = true;
            if (IsInToxic == true || isRadiated == true)
            {
                if(isRadiated)
                {
                    Rads++;
                }
                Health -= ToxicDamage;
                yield return new WaitForSeconds(HealthTick);
            }
            else
            {
                yield return null;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ToxicZone") && IsInToxic == false)
        {
            IsInToxic = true;
            if (InCourotine == false)
            {
                StartCoroutine(HealthMinus());
            }
            Debug.Log("Hit Toxic");
        }

        if(other.CompareTag("Cure"))
        {
            if(isRadiated == true)
            {
                isRadiated = false;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("ToxicZone") && IsInToxic == true)
        {
            InCourotine = false;
            IsInToxic = false;
            StopCoroutine(HealthMinus());
        }
    }
}
