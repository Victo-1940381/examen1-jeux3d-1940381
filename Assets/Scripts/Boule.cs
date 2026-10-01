using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Objet représentant une boule contrôlée par le joueur.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Boule : MonoBehaviour
{
    [SerializeField, Tooltip("La cible pour le suvi de la caméra")]
    private Transform cibleCamera;

    [SerializeField, Tooltip("Force de déplacement de la boule.")]
    private float forceDeplacement;

    // Force appliquée à la boule pour le déplacement à chaque frame.
    private Vector3 forceAppliquee;

    // Référence au Rigidbody de la boule pour appliquer la physique.
    private Rigidbody rigidbody;

    private bool AccelActif;
    /// <summary>
    /// Obtient la vélocité actuelle de la boule.
    /// </summary>
    public Vector3 Velocite => rigidbody.linearVelocity;
    private PlayerInput controles;
    [field:SerializeField]
    public int NombreAccel { get;private set; }

    [SerializeField]
    private TextMeshProUGUI nombreAccel;


    private void Start()
    {
        NombreAccel = 0;
        AccelActif = false;
        rigidbody = GetComponent<Rigidbody>();
        controles = ControleurJeu.Instance.Controles;
        InputAction actionCommencer = controles.actions.FindAction("player/Commencer");
        actionCommencer.performed += CommencerJeu; 
    }

    private void OnDestroy()
    {
        if (ControleurJeu.Instance == null)
            return;

        PlayerInput controles = ControleurJeu.Instance.Controles;

        if (controles == null) 
            return;

        controles.actions.FindAction("Diriger").performed -= CommencerDirection;
        controles.actions.FindAction("Diriger").canceled -= ArreterDirection;
    }

    private void Update()
    {
        nombreAccel.text = NombreAccel.ToString();
        if (cibleCamera != null)
        {
            cibleCamera.position = rigidbody.position;
        }
        
    }

    private void FixedUpdate()
    {
        Diriger();
        
    }

    private void CommencerDirection(InputAction.CallbackContext contexte)
    {
        forceAppliquee += contexte.ReadValue<float>() * forceDeplacement * Vector3.right;
    }

    private void ArreterDirection(InputAction.CallbackContext contexte)
    {
        forceAppliquee = Vector3.zero;
    }

    private void Diriger()
    {
        if(!Mathf.Approximately(forceAppliquee.sqrMagnitude, 0.0f))
        {
            rigidbody.AddForce(forceAppliquee, ForceMode.Force);
        }
    }
    private void CommencerJeu(InputAction.CallbackContext contexte)
    {
        rigidbody.useGravity = true;
        controles.actions.FindAction("Diriger").performed += CommencerDirection;
        controles.actions.FindAction("Diriger").canceled += ArreterDirection;
        controles.actions.FindAction("Acceleration").started += CommencerAccel;

    }
    public void AjouterAccel()
    {
        NombreAccel += 1;
    }
    private void CommencerAccel(InputAction.CallbackContext contexte)
    {
        Acceleration();
    }
    private void Acceleration()
    {
        AccelActif = true;
        if (NombreAccel > 0 && AccelActif)
        { 
            this.rigidbody.AddForce(Vector3.forward * 15, ForceMode.Acceleration);
            NombreAccel -= 1;
            AccelActif= false;
        }
    }


}
