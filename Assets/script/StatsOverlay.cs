
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.UI;


public class StatsOverlay : MonoBehaviour
{
    [Header("References")]
    public ElectrostaticGrid grid;
    public Transform rayOrigin;
    public TMP_Text text;
    public Transform arrow; // optional
    public LineRenderer laserLine; // assign in inspector
    public InputActionReference triggerButton;
    
   

    [Header("Ray Settings")]
    public float rayLength = 500f;

    [Header("Optional: follow head")]
    public bool followHead = false;
    public Transform followTarget; // Main Camera
    public Vector3 followOffset = new Vector3(0f, -0.15f, 0.6f);

    private bool triggerpress = false;

    private bool uienter = false;
    

    void Start()
    {
        triggerButton.action.started += TriggerPressed;
        triggerButton.action.canceled += TriggerReleased;
        text.text = "Hold down Trigger to veiw stats";

    }


    void Update()
    {

        if (triggerpress && uienter == false)
        {

            if (!grid || !rayOrigin || !text) return;

            Ray r = new Ray(rayOrigin.position, rayOrigin.forward);

            // --- Raycast ---
            Vector3 hitPoint;
            if (Physics.Raycast(r, out RaycastHit hit, rayLength))
            {
                hitPoint = hit.point;

                // --- Sample electric potential and field ---
                if (grid.TrySamplePotential(hitPoint, out float V) &&
                    grid.TrySampleField(hitPoint, out Vector2 E))
                {
                    float Emag = E.magnitude;

                    // Update the UI text
                    text.text =
                        $"V (potential): {V:F3}\n" +
                        $"|E| (steepness): {Emag:F3}\n";
                    //  $"E dir (x,z): ({E.x:F2}, {E.y:F2})";

                    // Optional: move and rotate the arrow to show field direction
                    if (arrow)
                    {
                        arrow.position = hitPoint + Vector3.up * 0.02f;
                        Vector3 dir = (grid.transform.right * E.x) + (grid.transform.forward * E.y);
                        if (dir.sqrMagnitude > 0.0001f)
                            arrow.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
                    }
                }
                else
                {
                    text.text = "OUTSIDE GRID";
                }
            }
            else
            {
                // No hit
                hitPoint = rayOrigin.position + r.direction * rayLength;
                text.text = "NO HIT";
            }

            // --- Update LineRenderer for visible laser ---
            if (laserLine)
            {
                laserLine.positionCount = 2;
                laserLine.SetPosition(0, rayOrigin.position);
                laserLine.SetPosition(1, hitPoint);
            }
        }
        else
        {
            if (laserLine)
            {
                laserLine.positionCount = 0;
                
            }
        }
        
    }

    public void OnUIHoverEntered(UIHoverEventArgs args)
    {
        Debug.Log("ui entered");
        uienter = true;
    }

    public void OnUIHoverExited(UIHoverEventArgs args)
    {
        Debug.Log("ui exit");
        uienter = false;
    }

    void TriggerPressed(InputAction.CallbackContext context)
    {

        triggerpress = true;
   

    }

    void TriggerReleased(InputAction.CallbackContext context)
    {

        triggerpress = false;
      

    }

  

    void LateUpdate()
    {
        if (!followHead || !followTarget) return;

        transform.position = followTarget.TransformPoint(followOffset);
        transform.rotation = Quaternion.LookRotation(transform.position - followTarget.position);
    }
}