using System;
using System.ComponentModel;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip m_DoorHit;

        [SerializeField] private AudioClip[] m_FootstepSounds;
            // an array of footstep sounds that will be randomly selected from.

        [SerializeField] private GameObject m_WallHitPrefab;
            // an array of footstep sounds that will be randomly selected from.

        [SerializeField] private AudioClip m_DeathByArrow;
            // an array of footstep sounds that will be randomly selected from.

        [SerializeField] private AudioClip m_DeathByWater;

        [SerializeField] private AudioClip m_DeathByCrush;

        [SerializeField] private AudioClip m_ItMightBeAWall;

        [SerializeField] private AudioClip m_InWater1;
        [SerializeField] private AudioClip m_InWater2;
        [SerializeField] private AudioClip m_InWater3;
        [SerializeField] private AudioClip m_DeathByMonk;

        [SerializeField] private AudioClip m_DrinkPotion;
        public static int howMuchInWater;


        private bool isChained = true; // an array of footstep sounds that will be randomly selected from.

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;
        private bool isDead = false;
        private bool isWallKeyPickedUp = false;
        private bool isMainKeyPickedUp = false;
        private int wallhitcount;


        // Use this for initialization
        private void Start()
        {
            isChained = true;
            m_WalkSpeed = 0;
            m_CharacterController = GetComponent<CharacterController>();
            m_CharacterController.detectCollisions = false;
            m_Camera = Camera.main;
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            m_MouseLook.Init(transform, m_Camera.transform);
            wallhitcount = 0;
        }


        // Update is called once per frame
        private void Update()
        {
            RotateView();
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
            if (isDead)
                Invoke("NewGameStart", 5);
        }

        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);

            if (!isDead)
            {
                if (isChained && speed == 0)
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        isChained = false;
                        m_WalkSpeed = 1;
                    }
                }
                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

                // get a normal for the surface that is being touched to move along it
                RaycastHit hitInfo;
                Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                    m_CharacterController.height/2f);
                desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

                m_MoveDir.x = desiredMove.x*speed;
                m_MoveDir.z = desiredMove.z*speed;


                if (m_CharacterController.isGrounded)
                {
                    m_MoveDir.y = -m_StickToGroundForce;
                }
                else
                {
                    m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
                }
                m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

                ProgressStepCycle(speed);
            }
        }

        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude +
                                (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                               Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }

        private void NewGameStart()
        {
            isDead = false;
            Application.LoadLevel("main");
        }

        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            if (howMuchInWater == 1)
            {
                m_AudioSource.clip = m_InWater1;
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
            }
            else if (howMuchInWater == 2)
            {
                m_AudioSource.clip = m_InWater2;
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
            }
            else if (howMuchInWater == 3)
            {
                m_AudioSource.clip = m_InWater3;
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
            }
            else
            {
                int n = Random.Range(1, m_FootstepSounds.Length);
                m_AudioSource.clip = m_FootstepSounds[n];

                m_AudioSource.PlayOneShot(m_AudioSource.clip);

                m_FootstepSounds[n] = m_FootstepSounds[0];
                m_FootstepSounds[0] = m_AudioSource.clip;
            }
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);
            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }
        }


        private void RotateView()
        {
            if (!EndSceneCameraScript.isCloseUpStarted)
            {
                m_MouseLook.LookRotation(transform, m_Camera.transform);
            }
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "ExternalWall")
            {
                Debug.Log("ExternalWall");
                GameObject clone = (GameObject) Instantiate(m_WallHitPrefab, transform.position, transform.rotation);
                Destroy(clone, 1);
                if (wallhitcount < 5)
                {
                    wallhitcount++;
                }
                else
                {
                    wallhitcount = 0;
                    GameObject clone1 =
                        (GameObject) Instantiate(m_WallHitPrefab, transform.position, transform.rotation);
                    clone1.GetComponent<AudioSource>().clip = m_ItMightBeAWall;
                    clone1.GetComponent<AudioSource>().Play();
                    Destroy(clone1, 4);
                }
            }
            else if (other.gameObject.name == "WallKey")
            {
                isWallKeyPickedUp = true;
            }
            else if (other.gameObject.name == "MainKey")
            {
                isMainKeyPickedUp = true;
            }
            else if (other.gameObject.name == "ArrowTrap")
            {
                Debug.Log("Arrow hit me");
                Destroy(other.gameObject);
                m_AudioSource.clip = m_DeathByArrow;
                m_AudioSource.Play(0);
                isDead = true;
            }
            else if (other.gameObject.name == "MovingWall")
            {
                m_AudioSource.clip = m_DeathByCrush;
                m_AudioSource.Play(0);
                isDead = true;
            }
            else if (other.gameObject.name == "MovingWallDoor" && isWallKeyPickedUp)
            {
                other.gameObject.GetComponent<AudioSource>().clip =
                    other.gameObject.GetComponent<DoorScript>().m_DoorOpenedSound;
                other.gameObject.GetComponent<AudioSource>().Play();
                other.gameObject.GetComponent<Collider>().enabled = false;
            }
            else if (other.gameObject.name == "MovingWallDoor" && !isWallKeyPickedUp)
            {
                GameObject clone2 = (GameObject) Instantiate(m_WallHitPrefab, transform.position, transform.rotation);
                clone2.GetComponent<AudioSource>().clip = m_DoorHit;
                clone2.GetComponent<AudioSource>().Play();
                Destroy(clone2, 4);
            }
            else if (other.gameObject.name == "MainDoor" && !isMainKeyPickedUp)
            {
                GameObject clone2 = (GameObject) Instantiate(m_WallHitPrefab, transform.position, transform.rotation);
                clone2.GetComponent<AudioSource>().clip = m_DoorHit;
                clone2.GetComponent<AudioSource>().Play();
                Destroy(clone2, 4);
            }
            else if (other.gameObject.name == "MainDoor" && isMainKeyPickedUp)
            {
                other.gameObject.GetComponent<AudioSource>().clip =
                    other.gameObject.GetComponent<DoorScript>().m_DoorOpenedSound;
                other.gameObject.GetComponent<AudioSource>().Play();
                Invoke("LoadGoodEnd", 2);
            }
            else if (other.gameObject.name == "WaterTrigger1")
            {
                howMuchInWater = 1;
            }
            else if (other.gameObject.name == "WaterTrigger2")
            {
                howMuchInWater = 2;
            }
            else if (other.gameObject.name == "WaterTrigger3")
            {
                howMuchInWater = 3;
            }
            else if (other.gameObject.name == "WaterTrigger4")
            {
                isDead = true;
                howMuchInWater = 4;
                m_AudioSource.clip = m_DeathByWater;
                m_AudioSource.Play();

            }
            else if (other.gameObject.name == "WaterTrigger0")
            {
                howMuchInWater = 0;
            }
            else if (other.gameObject.name == "MovingBall")
            {
                isDead = true;
                m_AudioSource.clip = m_DeathByCrush;
                m_AudioSource.Play();
            }
            else if (other.gameObject.tag == "Monk")
            {
                isDead = true;
                m_AudioSource.clip = m_DeathByMonk;
                m_AudioSource.Play();
                GameObject.Find("Monk").GetComponent<AudioSource>().Stop();
                GameObject.Find("Monk (1)").GetComponent<AudioSource>().Stop();
                GameObject.Find("Monk (2)").GetComponent<AudioSource>().Stop();
                GameObject.Find("Monk (3)").GetComponent<AudioSource>().Stop();
            }
        }

        private void LoadGoodEnd()
        {
            Application.LoadLevel("final_scene_good");
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.name == "FinalRoomMonk")
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("space!");
                    m_AudioSource.clip = m_DrinkPotion;
                    m_AudioSource.Play();
                    isDead = true;
                    Invoke("LoadScene", 2.0f);
                }
                Debug.Log("Collided");
            }
        }

        private void LoadScene()
        {
            Application.LoadLevel("final_scene_bad");
        }
    }
}
