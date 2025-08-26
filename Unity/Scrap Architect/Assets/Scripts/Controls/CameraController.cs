using UnityEngine;
using System.Collections;
using ScrapArchitect.Core;

namespace ScrapArchitect.Controls
{
    /// <summary>
    /// Контроллер камеры - орбитальная камера с зумом и следованием
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Settings")]
        public float rotationSpeed = 2f;
        public float zoomSpeed = 5f;
        public float panSpeed = 10f;
        public float smoothSpeed = 5f;
        
        [Header("Zoom Limits")]
        public float minZoom = 2f;
        public float maxZoom = 20f;
        
        [Header("Target Settings")]
        public Transform target;
        public Vector3 offset = new Vector3(0, 5, -10);
        public bool followTarget = true;
        
        [Header("Input Settings")]
        public KeyCode rotateKey = KeyCode.Mouse1;
        public KeyCode panKey = KeyCode.Mouse2;
        public KeyCode resetViewKey = KeyCode.Space;
        
        // Приватные переменные
        private Vector3 currentRotation;
        private float currentZoom;
        private Vector3 currentPosition;
        private bool isRotating = false;
        private bool isPanning = false;
        private Vector3 lastMousePosition;
        
        private void Start()
        {
            InitializeCamera();
        }
        
        private void Update()
        {
            if (Core.GameManager.Instance != null && 
                Core.GameManager.Instance.currentGameState != GameState.Paused)
            {
                HandleInput();
                UpdateCamera();
            }
        }
        
        /// <summary>
        /// Инициализация камеры
        /// </summary>
        private void InitializeCamera()
        {
            currentRotation = transform.eulerAngles;
            currentZoom = offset.magnitude;
            currentPosition = transform.position;
            
            // Устанавливаем начальную позицию
            if (target != null)
            {
                transform.position = target.position + offset;
                transform.LookAt(target);
            }
            
            Debug.Log("CameraController initialized");
        }
        
        /// <summary>
        /// Обработка ввода
        /// </summary>
        private void HandleInput()
        {
            // Вращение камеры
            if (Input.GetKeyDown(rotateKey))
            {
                isRotating = true;
                lastMousePosition = Input.mousePosition;
            }
            
            if (Input.GetKeyUp(rotateKey))
            {
                isRotating = false;
            }
            
            // Перемещение камеры
            if (Input.GetKeyDown(panKey))
            {
                isPanning = true;
                lastMousePosition = Input.mousePosition;
            }
            
            if (Input.GetKeyUp(panKey))
            {
                isPanning = false;
            }
            
            // Зум
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                currentZoom -= scroll * zoomSpeed;
                currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            }
            
            // Сброс вида
            if (Input.GetKeyDown(resetViewKey))
            {
                ResetView();
            }
        }
        
        /// <summary>
        /// Обновление камеры
        /// </summary>
        private void UpdateCamera()
        {
            if (isRotating)
            {
                Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
                currentRotation.y += mouseDelta.x * rotationSpeed * Time.deltaTime;
                currentRotation.x -= mouseDelta.y * rotationSpeed * Time.deltaTime;
                currentRotation.x = Mathf.Clamp(currentRotation.x, -80f, 80f);
                lastMousePosition = Input.mousePosition;
            }
            
            if (isPanning)
            {
                Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
                Vector3 panDirection = new Vector3(-mouseDelta.x, -mouseDelta.y, 0);
                
                if (target != null)
                {
                    Vector3 right = transform.right * panDirection.x;
                    Vector3 up = transform.up * panDirection.y;
                    target.position += (right + up) * panSpeed * Time.deltaTime;
                }
                
                lastMousePosition = Input.mousePosition;
            }
            
            // Применяем трансформации
            if (target != null && followTarget)
            {
                Vector3 targetPosition = target.position;
                Vector3 desiredPosition = targetPosition + GetOrbitalOffset();
                
                currentPosition = Vector3.Lerp(currentPosition, desiredPosition, smoothSpeed * Time.deltaTime);
                transform.position = currentPosition;
                transform.LookAt(targetPosition);
            }
            else
            {
                transform.rotation = Quaternion.Euler(currentRotation);
            }
        }
        
        /// <summary>
        /// Получить орбитальное смещение
        /// </summary>
        private Vector3 GetOrbitalOffset()
        {
            float angleX = currentRotation.x * Mathf.Deg2Rad;
            float angleY = currentRotation.y * Mathf.Deg2Rad;
            
            float x = currentZoom * Mathf.Sin(angleX) * Mathf.Sin(angleY);
            float y = currentZoom * Mathf.Cos(angleX);
            float z = currentZoom * Mathf.Sin(angleX) * Mathf.Cos(angleY);
            
            return new Vector3(x, y, z);
        }
        
        /// <summary>
        /// Сбросить вид камеры
        /// </summary>
        public void ResetView()
        {
            currentRotation = Vector3.zero;
            currentZoom = offset.magnitude;
            
            if (target != null)
            {
                currentPosition = target.position + offset;
                transform.position = currentPosition;
                transform.LookAt(target);
            }
            
            Debug.Log("Camera view reset");
        }
        
        /// <summary>
        /// Установить цель для следования
        /// </summary>
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            Debug.Log($"Camera target set to: {newTarget?.name ?? "null"}");
        }
        
        /// <summary>
        /// Включить/выключить следование за целью
        /// </summary>
        public void SetFollowTarget(bool follow)
        {
            followTarget = follow;
            Debug.Log($"Camera follow target: {follow}");
        }
        
        /// <summary>
        /// Переместить камеру к позиции
        /// </summary>
        public void MoveToPosition(Vector3 position, float duration = 1f)
        {
            StartCoroutine(MoveToPositionCoroutine(position, duration));
        }
        
        /// <summary>
        /// Корутина для плавного перемещения камеры
        /// </summary>
        private IEnumerator MoveToPositionCoroutine(Vector3 targetPosition, float duration)
        {
            Vector3 startPosition = transform.position;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                t = Mathf.SmoothStep(0f, 1f, t);
                
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                yield return null;
            }
            
            transform.position = targetPosition;
        }
        
        /// <summary>
        /// Получить луч от камеры через точку экрана
        /// </summary>
        public Ray GetScreenRay(Vector3 screenPosition)
        {
            return Camera.main.ScreenPointToRay(screenPosition);
        }
        
        /// <summary>
        /// Получить точку в мире под курсором
        /// </summary>
        public Vector3 GetWorldPointUnderCursor(float height = 0f)
        {
            Ray ray = GetScreenRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, Vector3.up * height);
            
            if (plane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }
            
            return Vector3.zero;
        }
    }
}
