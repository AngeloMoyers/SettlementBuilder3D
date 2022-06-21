using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float m_speed;

    [SerializeField] float m_minZoomThreshold = 5f;
    [SerializeField] float m_maxZoomThreshold = 8.5f;
    [SerializeField] float m_zoomSpeed = 5f;

    Camera m_cam;

    private bool m_lockedMode = false;
    private float m_camStartY;
    private float m_minZoom;
    private float m_maxZoom;

    //Managers
    CharacterManager m_characterManager;
    GameObject m_targetedCharacter = null;

    private void Awake()
    {
        m_characterManager = GameObject.Find("CharacterManager").GetComponent<CharacterManager>();
    }

    private void Start()
    {
        m_cam = GetComponent<Camera>();
        m_camStartY = m_cam.transform.position.y;
        m_minZoom = m_camStartY - m_minZoomThreshold;
        m_maxZoom = m_camStartY + m_maxZoomThreshold;
    }

    void Update()
    {
        if (m_lockedMode && m_targetedCharacter != null)
        {
            //follow targeted character
            Vector3 pos = new Vector3(m_targetedCharacter.transform.position.x, m_targetedCharacter.transform.position.y, transform.position.z);
            transform.position = pos;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_lockedMode = false;
            Move(new Vector3(0,0, m_speed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            m_lockedMode = false;
            Move(new Vector3(0, 0, -m_speed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_lockedMode = false;
            Move(new Vector3(m_speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_lockedMode = false;
            Move(new Vector3(-m_speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            m_targetedCharacter = m_characterManager.GetActiveCharacterGO();
            if (m_targetedCharacter != null && !m_lockedMode) m_lockedMode = true;
            else m_lockedMode = false;
        }

        Zoom(-Input.GetAxis("Mouse ScrollWheel") * m_zoomSpeed);
    }

    private void Move(Vector3 mov)
    {
        float m_camY = transform.position.y;
        Vector3 tempMove = transform.position + mov;
        Vector3 move = new Vector3(tempMove.x, m_camY, tempMove.z);
        transform.position = move;
    }

    public void Zoom(float val)
    {
        m_cam.transform.position = new Vector3(m_cam.transform.position.x, m_cam.transform.position.y + val, m_cam.transform.position.z);
        if (m_cam.transform.position.y > m_maxZoom)
        {
            m_cam.transform.position = new Vector3(m_cam.transform.position.x, m_maxZoom, m_cam.transform.position.z);
        }
        if (m_cam.transform.position.y < m_minZoom)
        {
            m_cam.transform.position = new Vector3(m_cam.transform.position.x, m_minZoom, m_cam.transform.position.z);
        }
    }
}
