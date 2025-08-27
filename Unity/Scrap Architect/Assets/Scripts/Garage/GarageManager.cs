using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ScrapArchitect.Garage
{
    public class GarageManager : MonoBehaviour
    {
        [Header("Camera Settings")]
        public Camera playerCamera;
        public float mouseSensitivity = 2f;
        public float walkSpeed = 5f;
        public float runSpeed = 8f;
        
        [Header("Garage Zones")]
        public Transform workbenchZone;
        public Transform computerZone;
        public Transform bulletinBoardZone;
        public Transform safeZone;
        public Transform doorZone;
        
        [Header("UI Elements")]
        public GameObject interactionUI;
        public Text interactionText;
        
            [Header("Audio")]
    public AudioSource ambientAudio;
    public AudioClip[] garageAmbientSounds;
    
    [Header("Menu System")]
    public GarageMenuManager menuManager;
    
    // Private variables
    private float verticalRotation = 0f;
    private CharacterController characterController;
    private bool isInteracting = false;
    private bool isInMenuMode = false;
    private GarageZone currentZone = null;
    
    // Garage zones
    private List<GarageZone> zones = new List<GarageZone>();
        
        void Start()
        {
            InitializeGarage();
            SetupCamera();
            SetupAudio();
        }
        
            void Update()
    {
        if (!isInteracting && !isInMenuMode)
        {
            HandleMovement();
            HandleMouseLook();
            HandleInteraction();
        }
        
        // Обработка закрытия меню
        if (isInMenuMode && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }
        
        void InitializeGarage()
        {
            // Get character controller
            characterController = GetComponent<CharacterController>();
            if (characterController == null)
            {
                characterController = gameObject.AddComponent<CharacterController>();
            }
            
            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            // Initialize zones
            InitializeZones();
        }
        
        void SetupCamera()
        {
            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }
            
            // Position camera at head level
            playerCamera.transform.localPosition = new Vector3(0, 1.8f, 0);
        }
        
        void SetupAudio()
        {
            if (ambientAudio == null)
            {
                ambientAudio = gameObject.AddComponent<AudioSource>();
            }
            
            // Play random ambient sound
            if (garageAmbientSounds.Length > 0)
            {
                ambientAudio.clip = garageAmbientSounds[Random.Range(0, garageAmbientSounds.Length)];
                ambientAudio.loop = true;
                ambientAudio.Play();
            }
        }
        
        void InitializeZones()
        {
            // Create zones if they don't exist
            if (workbenchZone != null)
            {
                var workbench = workbenchZone.gameObject.AddComponent<GarageZone>();
                workbench.Initialize("Верстак", "Нажмите E для доступа к контрактам", ZoneType.Workbench);
                zones.Add(workbench);
            }
            
            if (computerZone != null)
            {
                var computer = computerZone.gameObject.AddComponent<GarageZone>();
                computer.Initialize("Компьютер", "Нажмите E для доступа к Steam Workshop", ZoneType.Computer);
                zones.Add(computer);
            }
            
            if (bulletinBoardZone != null)
            {
                var board = bulletinBoardZone.gameObject.AddComponent<GarageZone>();
                board.Initialize("Доска объявлений", "Нажмите E для просмотра контрактов", ZoneType.BulletinBoard);
                zones.Add(board);
            }
            
            if (safeZone != null)
            {
                var safe = safeZone.gameObject.AddComponent<GarageZone>();
                safe.Initialize("Сейф", "Нажмите E для доступа к магазину", ZoneType.Safe);
                zones.Add(safe);
            }
            
            if (doorZone != null)
            {
                var door = doorZone.gameObject.AddComponent<GarageZone>();
                door.Initialize("Дверь", "Нажмите E для выхода на тестовый полигон", ZoneType.Door);
                zones.Add(door);
            }
        }
        
        void HandleMovement()
        {
            if (characterController == null) return;
            
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            
            Vector3 movement = transform.right * horizontal + transform.forward * vertical;
            characterController.Move(movement * speed * Time.deltaTime);
            
            // Debug movement
            if (horizontal != 0 || vertical != 0)
            {
                Debug.Log($"Движение: H={horizontal}, V={vertical}, Speed={speed}");
            }
        }
        
        void HandleMouseLook()
        {
            if (playerCamera == null) return;
            
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            
            // Rotate player left/right
            transform.Rotate(Vector3.up * mouseX);
            
            // Rotate camera up/down
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }
        
        void HandleInteraction()
        {
            // Check for interaction zones
            GarageZone nearestZone = GetNearestZone();
            
            if (nearestZone != null && nearestZone.IsInRange())
            {
                ShowInteractionUI(nearestZone.GetInteractionText());
                
                if (Input.GetKeyDown(KeyCode.E))
                {
                    InteractWithZone(nearestZone);
                }
            }
            else
            {
                HideInteractionUI();
            }
        }
        
        GarageZone GetNearestZone()
        {
            GarageZone nearest = null;
            float nearestDistance = float.MaxValue;
            
            foreach (var zone in zones)
            {
                float distance = Vector3.Distance(transform.position, zone.transform.position);
                if (distance < nearestDistance && distance <= zone.interactionRange)
                {
                    nearestDistance = distance;
                    nearest = zone;
                }
            }
            
            return nearest;
        }
        
        void ShowInteractionUI(string text)
        {
            if (interactionUI != null)
            {
                interactionUI.SetActive(true);
                if (interactionText != null)
                {
                    interactionText.text = text;
                }
            }
        }
        
        void HideInteractionUI()
        {
            if (interactionUI != null)
            {
                interactionUI.SetActive(false);
            }
        }
        
        void InteractWithZone(GarageZone zone)
        {
            isInteracting = true;
            currentZone = zone;
            
            switch (zone.zoneType)
            {
                case ZoneType.Workbench:
                    OpenWorkbench();
                    break;
                case ZoneType.Computer:
                    OpenComputer();
                    break;
                case ZoneType.BulletinBoard:
                    OpenBulletinBoard();
                    break;
                case ZoneType.Safe:
                    OpenSafe();
                    break;
                case ZoneType.Door:
                    OpenDoor();
                    break;
            }
        }
        
        void OpenWorkbench()
        {
            // Находим менеджер меню если не найден
            if (menuManager == null)
            {
                menuManager = FindObjectOfType<GarageMenuManager>();
            }
            
            if (menuManager != null)
            {
                menuManager.OpenMenu(ZoneType.Workbench);
                isInMenuMode = true;
            }
            else
            {
                Debug.Log("Открываем верстак - доступ к контрактам");
            }
            isInteracting = false;
        }
        
        void OpenComputer()
        {
            // Находим менеджер меню если не найден
            if (menuManager == null)
            {
                menuManager = FindObjectOfType<GarageMenuManager>();
            }
            
            if (menuManager != null)
            {
                menuManager.OpenMenu(ZoneType.Computer);
                isInMenuMode = true;
            }
            else
            {
                Debug.Log("Открываем компьютер - Steam Workshop");
            }
            isInteracting = false;
        }
        
        void OpenBulletinBoard()
        {
            // Находим менеджер меню если не найден
            if (menuManager == null)
            {
                menuManager = FindObjectOfType<GarageMenuManager>();
            }
            
            if (menuManager != null)
            {
                menuManager.OpenMenu(ZoneType.BulletinBoard);
                isInMenuMode = true;
            }
            else
            {
                Debug.Log("Открываем доску объявлений - просмотр контрактов");
            }
            isInteracting = false;
        }
        
        void OpenSafe()
        {
            // Находим менеджер меню если не найден
            if (menuManager == null)
            {
                menuManager = FindObjectOfType<GarageMenuManager>();
            }
            
            if (menuManager != null)
            {
                menuManager.OpenMenu(ZoneType.Safe);
                isInMenuMode = true;
            }
            else
            {
                Debug.Log("Открываем сейф - магазин деталей");
            }
            isInteracting = false;
        }
        
        void OpenDoor()
        {
            // Находим менеджер меню если не найден
            if (menuManager == null)
            {
                menuManager = FindObjectOfType<GarageMenuManager>();
            }
            
            if (menuManager != null)
            {
                menuManager.OpenMenu(ZoneType.Door);
                isInMenuMode = true;
            }
            else
            {
                Debug.Log("Выходим на тестовый полигон");
            }
            isInteracting = false;
        }
        
        public void SetInteractionMode(bool inMenu)
        {
            isInMenuMode = inMenu;
        }
        
        public void CloseMenu()
        {
            if (menuManager != null)
            {
                menuManager.CloseCurrentMenu();
            }
            isInMenuMode = false;
        }
        
        public void ExitInteraction()
        {
            isInteracting = false;
            currentZone = null;
        }
    }
    
    public enum ZoneType
    {
        Workbench,
        Computer,
        BulletinBoard,
        Safe,
        Door
    }
}
