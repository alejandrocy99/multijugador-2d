using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{
    private float movimientoHorizontal = 0f;
    private Rigidbody2D rb;
    private Animator animator;
    public float velocidadMovimiento = 10f;
    private bool mirandoDerecha = true;
    private Vector3 velocidad = Vector3.zero;

    public float fuerzaSalto;
    public LayerMask suelo;
    public Transform comprobarSuelo;

    public  Vector3 dimesionesCaja;
    public bool enSuelo;
    private bool salto = false;
    private PhotonView photonView;

    void Start()
{
    photonView = GetComponent<PhotonView>();
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.position = transform.position + (Vector3.up * 2) + (transform.forward * -10);

    if (photonView == null)
    {
        Debug.LogError("PhotonView no encontrado en el objeto: " + gameObject.name);
    }

    if (animator == null)
    {
        Debug.LogError("Animator no encontrado en el objeto: " + gameObject.name);
    }

    if (!photonView.IsMine)
    {
        return;
    }
}
    void Update()
    {
        if(photonView.IsMine){
        movimientoHorizontal = Input.GetAxis("Horizontal") * velocidadMovimiento;
        animator.SetFloat("velocityX", Mathf.Abs(movimientoHorizontal));

        if(Input.GetButtonDown("Jump") ){
            salto = true;
         }
    }
    }

    private void FixedUpdate(){
        if (!photonView.IsMine) return; 
        enSuelo = Physics2D.OverlapBox(comprobarSuelo.position, dimesionesCaja, 0f, suelo);
        Debug.Log(enSuelo);
        animator.SetBool( "enSuelo", enSuelo);
        Mover(movimientoHorizontal * Time.fixedDeltaTime,salto);
        salto = false;
    }

    private void Mover(float mover,bool saltar){
        Vector3 velocidad = new Vector3(mover, rb.velocity.y, 0);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, velocidad, ref velocidad, 0.05f);

        if (mover > 0 && !mirandoDerecha){
            Girar();
        } else if (mover < 0 && mirandoDerecha){
            Girar();
        }

        if(enSuelo && saltar){
            enSuelo = false;
            rb.AddForce(new Vector2(0f, fuerzaSalto));
        }
    }


    public void Girar()
{
    if (photonView.IsMine)
    {
        photonView.RPC("RPC_Girar", RpcTarget.All);
    }
}

    private void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(comprobarSuelo.position, dimesionesCaja);
    }
    [PunRPC]
    public void RPC_Girar()
{
    mirandoDerecha = !mirandoDerecha;
    Vector3 escala = transform.localScale;
    escala.x *= -1;
    transform.localScale = escala;
}


    
}