using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Cinemachine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerCameraController))]
public class PlayerThrow2 : MonoBehaviour
{
    #region Variables

    #region Throw - variables
    [Header("Throw")]
    [SerializeField][Tooltip("How long until projectile is ready for throwing. ")]
    private float mChargeDuration = 1f;
    private float mChargeTimer = 0f;
    [SerializeField][Tooltip("How long yo are allowed to build charge. Beyond this, it just doesn't grow anymore. ")]
    private float mChargeMax = 500f;
    private PlayerAnimations mAnimationController = null;

    [System.Serializable]
    private enum TargetRayDirectionSource
    {
        Player,
        Camera
    };
    [System.Serializable]
    private enum SelectClosestTo
    {
        Camera,
        Ray,
        Player,
        RayPlayerLinear /* Linearly selecting whichever is closest, taking into account both the player and the ray's distance away from the target.  */
    };

    #endregion

    #region Targeting - variables
    [Header("Targeting")]
    [SerializeField][Tooltip("Far plane in world space of the targeting cone, starting from the camera. ")]
    private float mTargetDistanceFar = 10f;
    [SerializeField][Tooltip("Near plane in world space of the targeting cone, starting from the camera. ")]
    private float mTargetDistanceNear = 0.1f;
    [Space(10)]
    [SerializeField]
    private TargetRayDirectionSource mTargetRaySource = TargetRayDirectionSource.Camera;
    [SerializeField]
    private SelectClosestTo mTargetSelectionMode = SelectClosestTo.Ray;
    private Transform mTarget = null;
    public Transform GetTarget() { return mTarget; }
    [SerializeField]
    private GameObject mTargetIndicator = null;
    [SerializeField]
    private Vector3 mTargetIndicatorOffset = new Vector3(0f, 3f, 0f);
    /// <summary>
    /// Dictionary of potential enemies, holding their transform and a vector going from their position onto the player's raycast. 
    /// </summary>
    private Dictionary<Transform, Vector3> mTargetDictionary = new Dictionary<Transform, Vector3>();
    private List<Transform> mTargetDictionaryCopy = new List<Transform>();
    [SerializeField]
    private Transform mCamera = null;

    #endregion

    #region Projectile - variables

    [Header("Projectile")]
    [SerializeField]
    private GameObject mProjectile = null;
    [SerializeField]
    private Transform mProjectileParent = null;
    [SerializeField]
    private Transform mHandToThrowFrom = null;
    private Player_SnowballBehavior mCurrentSnowball = null;

    #endregion

    #region Movement - variables
    [Header("Movement")]
    [SerializeField]
    private float mAimingMovementMultiplier = 1f;
    private float mMovementSpeedInitial;
    private float mMovementSpeedAiming;
    #endregion

    #region Refrences

    [Header("References")]
    [SerializeField]
    private Transform mThrowSpawnFree = null;
    [SerializeField]
    private Transform mThrowFocusFree = null;
    [SerializeField]
    private InputManager mInputManager = null;
    [SerializeField]
    private PlayerMovement mPlayerMovement = null;
    [SerializeField]
    private PlayerCameraController mCameraController = null;

    #endregion

    #region Debugging - variables
    [Header("Debug")]
    [SerializeField]
    private bool mDebug = true;
    [SerializeField]
    private Color mDebugColorEnemy = Color.red;
    [SerializeField]
    private Color mDebugColorRay = Color.blue;
#endregion

    #endregion

    #region Start/Awake
    private void Start ()
	{
        mAnimationController = transform.GetChild(0).GetComponent<PlayerAnimations>();

        if (mThrowSpawnFree == null)
            mThrowSpawnFree = transform
                .GetChild(3).transform
                .GetChild(1).transform
                .GetChild(1).transform;

        if (mThrowFocusFree == null)
            mThrowFocusFree = transform
                .GetChild(3).transform
                .GetChild(1).transform
                .GetChild(1).transform
                .GetChild(0).transform;

        if (mProjectileParent == null)
            mProjectileParent = GameObject.Find("Projectiles").transform;

        if (mInputManager == null)
            mInputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();

        if (mPlayerMovement == null)
            mPlayerMovement = GetComponent<PlayerMovement>();
        if (mCameraController == null)
            mCameraController = GetComponent<PlayerCameraController>();

        mTargetIndicator = GameObject.Find("Visualizer").transform.GetChild(0).gameObject;

        mHandToThrowFrom = transform.GetChild(0)
            .GetChild(0)
            .GetChild(2)
            .GetChild(0)
            .GetChild(0)
            .GetChild(2)
            .GetChild(2)
            .GetChild(0)
            .GetChild(0).transform;

        if (mCamera == null)
            mCamera = Camera.main.transform;

        // Storing movement variables upon start
        mMovementSpeedInitial = mPlayerMovement.GetMovementSpeedMax();
        mMovementSpeedAiming = mMovementSpeedInitial * mAimingMovementMultiplier; 
    }
    #endregion

    #region Update
    private void Update ()
	{
        // Charging
        if (mPlayerMovement.GetState() == PlayerMovement.State.Locomotion)
        {
            // Resetting target before checking for new ones
            if (mChargeTimer == 0)
            {
                mTarget = null;
            }

            if (!mPlayerMovement.GetImmobile())
            {
                if (mInputManager.GetTriggers().y != 0.0f)
                {
                    // Charge timer
                    if (mChargeTimer == 0)
                    {
                        // Animation and movement
                        mAnimationController.Animation_IsStartingToShoot();
                        mPlayerMovement.SetMovementSpeedMax(mMovementSpeedAiming);

                        if(mCurrentSnowball != null)
                        {
                            mCurrentSnowball.transform.GetChild(1).GetComponent<MeshRenderer>().materials[0].SetFloat("_OpacityMask", 0f);
                            mCurrentSnowball.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().materials[0].SetFloat("_OpacityMask", 0f);
                            mCurrentSnowball.transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().materials[0].SetFloat("_OpacityMask", 0f);

                            mCurrentSnowball.transform.GetChild(1).GetComponent<MeshRenderer>().materials[0].SetColor("_ReadyColor", Color.blue);
                            mCurrentSnowball.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().materials[0].SetColor("_ReadyColor", Color.blue);
                            mCurrentSnowball.transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().materials[0].SetColor("_ReadyColor", Color.blue);
                        }
                        mChargeTimer += Time.deltaTime;
                    }

                    if (mChargeTimer < mChargeMax)
                    {
                        mChargeTimer += Time.deltaTime;

                        if (mCurrentSnowball != null && (mChargeTimer / mChargeDuration) < 1f)
                        {
                            if(!mCurrentSnowball.transform.GetChild(1).gameObject.activeSelf)
                                mCurrentSnowball.transform.GetChild(1).gameObject.SetActive(true);

                            mCurrentSnowball.transform.GetChild(1).GetComponent<MeshRenderer>().materials[0].SetFloat("_OpacityMask", (mChargeTimer / mChargeDuration));
                            mCurrentSnowball.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().materials[0].SetFloat("_OpacityMask", (mChargeTimer / mChargeDuration));
                            mCurrentSnowball.transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().materials[0].SetFloat("_OpacityMask", (mChargeTimer / mChargeDuration));
                        }

                        if (mCurrentSnowball != null && (mChargeTimer / mChargeDuration) >= 1f && !mCurrentSnowball.transform.GetChild(2).gameObject.activeSelf)
                        {
                            mCurrentSnowball.transform.GetChild(1).GetComponent<MeshRenderer>().materials[0].SetFloat("_ReadyStrenght", 0.5f);
                            mCurrentSnowball.transform.GetChild(2).gameObject.SetActive(true);
                            transform.GetChild(0).GetComponent<AudioAvatar>().Play_snowballThrowChargeReadyCue();
                        }
                    }

                    // Enabling snowball
                    if (mCurrentSnowball == null)
                    {
                        SpawnSnowball();
                    }

                    // Enabling reticle
                    if (!mTargetIndicator.activeSelf)
                    {
                        mTargetIndicator.SetActive(true);
                    }

                    // Getting nearby enemies
                    GetNearbyEnemies(mTargetDistanceFar);

                    // Selecting target
                    float ruler = mTargetDistanceFar * mTargetDistanceFar;
                    foreach (KeyValuePair<Transform, Vector3> kvp in mTargetDictionary)
                    {
                        float sqrMag = kvp.Value.sqrMagnitude;
                        if (sqrMag < ruler && Vector3.Dot(kvp.Key.position - transform.position, Camera.main.transform.forward) > 0f)
                        {
                            ruler = sqrMag;
                            mTarget = kvp.Key;
                        }
                    }

                    // Removing target if you walk away after having targeted enemy
                    if ((mTarget.transform.position - transform.position).magnitude >= mTargetDistanceFar)
                    {
                        mTarget = null;
                    }

                    // Target indicator
                    if (mTarget != null)
                    {
                        mTargetIndicator.SetActive(true);
                        mTargetIndicator.transform.position = mTarget.transform.position + mTargetIndicatorOffset;
                        mTargetIndicator.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - mTargetIndicator.transform.position);
                    }
                    else
                    {
                        mTargetIndicator.SetActive(true);

                        Ray ray = new Ray(transform.position + transform.forward * mTargetDistanceFar + Vector3.up * 6f, Vector3.down);

                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, 12f))
                        {
                            mTargetIndicator.transform.position = hit.point;

                            mTargetIndicator.transform.forward = hit.normal;

                        }
                    }
                }
                // Throwing
                else
                {
                    if (mCurrentSnowball != null)
                    {
                        mCurrentSnowball.transform.GetChild(1).GetComponent<MeshRenderer>().materials[0].SetColor("_ReadyColor", Color.white);
                        mCurrentSnowball.transform.GetChild(1).GetComponent<MeshRenderer>().materials[0].SetFloat("_ReadyStrenght", 0f);
                        mCurrentSnowball.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().materials[0].SetFloat("_ReadyStrenght", 0f);
                        mCurrentSnowball.transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().materials[0].SetFloat("_ReadyStrenght", 0f);
                    }
                    // Resetting reticle
                    if (mTargetIndicator.activeSelf)
                    {
                        mTargetIndicator.SetActive(false);
                    }

                    // Resetting movement speed
                    mPlayerMovement.SetMovementSpeedMax(mMovementSpeedInitial);

                    // If elgible to throw...
                    if (mChargeTimer >= mChargeDuration)
                    {
                        // Debug
                        if (mDebug)
                        {
                            print("BUTTON RELEASE: \t RT");
                        }
                        //SpawnSnowball();
                        if (mTarget != null)
                        {
                            mAnimationController.Animation_ThrowSnowball();
                            mCurrentSnowball.SetTargetPositionAndChangeState(mTarget.position);
                        
                        }
                        else
                        {
                            mAnimationController.Animation_ThrowSnowball();
                            mCurrentSnowball.SetTargetPositionAndChangeState(transform.position + transform.forward * mTargetDistanceFar);
                        }
                        
                        // Resetting variables
                        mChargeTimer = 0f;
                        mCurrentSnowball = null;

                        // Debug
                        if (mDebug)
                        {
                            print("Aimed Throw!");
                            mTargetDictionary.Clear();
                            mTargetDictionaryCopy.Clear();
                        }
                    }
                    // Else abort charge
                    else if (mChargeTimer > 0f)
                    {
                        mAnimationController.Animation_CancelShooting();
                        mTargetIndicator.SetActive(false);
                        if (mCurrentSnowball != null)
                        {
                            mCurrentSnowball.transform.GetChild(1).gameObject.SetActive(false);
                        }
                        transform.GetChild(0).GetComponent<AudioAvatar>().Play_snowballNotReady();
                        mCurrentSnowball.DropSnowball();
                        mCurrentSnowball = null;
                        mChargeTimer = 0f;
                        mPlayerMovement.SetMovementSpeedInfluence(0f, 0f, false);
                    }
                }
            }
        }
        // If not in Locomotion state
        else
        {
            // Despawn snowball
            if (mCurrentSnowball != null)
                Destroy(mCurrentSnowball.gameObject);
            mChargeTimer = 0f;
        }

        // Debugging
        VisualDebug();
	}
    #endregion

    #region Snowball
    private void SpawnSnowball()
    {
        // GameObject
        GameObject snowball = Instantiate(
            mProjectile,
            mHandToThrowFrom.transform.position,
            Quaternion.identity,
            mProjectileParent);
        mCurrentSnowball = snowball.GetComponent<Player_SnowballBehavior>();
        mCurrentSnowball.SetThrowJoint(mHandToThrowFrom);
    }

    #endregion

    #region Find Enemy

    private void GetNearbyEnemies(float radius)
    {
        mTargetDictionary.Clear();

        // Create the ray direction
        Vector3 rayDir = Vector3.zero;
        switch (mTargetRaySource)
        {
            case TargetRayDirectionSource.Camera:
                {
                    rayDir = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
                    break;
                }
            case TargetRayDirectionSource.Player:
                {
                    rayDir = transform.forward;
                    break;
                }
            default:
                break;
        }
        Vector3 closePoint = Vector3.zero;
        switch (mTargetSelectionMode)
        {
            case SelectClosestTo.Camera:
                {
                    closePoint = transform.position + Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized * mTargetDistanceNear;
                    break;
                }
            case SelectClosestTo.Player:
                {
                    closePoint = transform.position + transform.forward * mTargetDistanceNear;
                    break;
                }
            case SelectClosestTo.Ray:
                {
                    closePoint = transform.position + transform.forward * mTargetDistanceNear;
                    break;
                }
            case SelectClosestTo.RayPlayerLinear:
                {
                    closePoint = transform.position + transform.forward * mTargetDistanceNear;
                    break;
                }
            default:
                break;
        }
        // Reduce framerate, use sqrMagnitude
        // 11: Enemy
        // Get the enemies within the far plane...
        Collider[] enemies = Physics.OverlapSphere(transform.position, mTargetDistanceFar, 1 << 11);
        foreach (Collider col in enemies)
        {
            // Get the enemies beyond the near radius...
            if (Vector3.Dot(col.transform.position - closePoint, rayDir) > 0 && !col.transform.GetComponent<TargetBehavior>().IsTargetDead)
            {
                // Calculating rejection of avatar-to-enemy onto avatar-forward...
                Vector3 E = closePoint - col.transform.position;   // Enemy-to-avatar
                Vector3 F = rayDir;  // Avatar forward
                Vector3 proj = Vector3.Dot(F, E) * F;
                Vector3 rej = E - proj;

                // Adding to dictionary
                mTargetDictionary.Add(col.transform, rej);
            }
        }
    }
    #endregion

    #region On Validate
    private void OnValidate()
    {
        mMovementSpeedInitial = mPlayerMovement.GetMovementSpeedMaxInitial();
        mMovementSpeedAiming = mMovementSpeedInitial * mAimingMovementMultiplier;
    }
    #endregion

    #region Debugging
    private void VisualDebug()
    {
        if (mDebug)
        {
            #region Look Ray
            switch (mTargetRaySource)
            {
                case TargetRayDirectionSource.Camera:
                    {
                        Debug.DrawRay(
                            transform.position + Vector3.ProjectOnPlane(mCamera.forward, Vector3.up).normalized * mTargetDistanceNear,
                            Vector3.ProjectOnPlane(mCamera.forward, Vector3.up).normalized * mTargetDistanceFar,
                            mDebugColorRay);

                        break;
                    }
                case TargetRayDirectionSource.Player:
                    {
                        Debug.DrawRay(
                            transform.position + transform.forward * mTargetDistanceNear,
                            transform.forward * (mTargetDistanceFar/* - mTargetDistanceNear*/),
                            mDebugColorRay);

                        break;
                    }
                default:
                    break;
            }
            #endregion

            #region Target Dictionary
            if (mTargetDictionary.Count > 0)
            {
                foreach (KeyValuePair<Transform, Vector3> kvp in mTargetDictionary)
                {
                    Debug.DrawLine(
                        kvp.Key.position,
                        kvp.Key.position + kvp.Value,
                        mDebugColorEnemy);
                }
            }
            #endregion
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 0f, 0f, 0.2f);
        Gizmos.DrawSphere(Camera.main.transform.position, mTargetDistanceFar);
    }
#endregion
}