using UnityEngine;

namespace ScrapArchitect.Garage
{
    public class GarageObjectCreator : MonoBehaviour
    {
        [Header("Materials")]
        public Material wallMaterial;
        public Material floorMaterial;
        public Material ceilingMaterial;
        public Material metalMaterial;
        public Material woodMaterial;
        
        [Header("Colors")]
        public Color wallColor = new Color(0.8f, 0.7f, 0.6f); // Бежевый
        public Color floorColor = new Color(0.6f, 0.5f, 0.4f); // Коричневый
        public Color ceilingColor = new Color(0.9f, 0.85f, 0.8f); // Светло-бежевый
        public Color metalColor = new Color(0.7f, 0.7f, 0.7f); // Серый металл
        public Color woodColor = new Color(0.5f, 0.3f, 0.2f); // Темное дерево
        
        public static GameObject CreateLowPolyCube(string name, Vector3 position, Vector3 scale, Material material)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            cube.transform.position = position;
            cube.transform.localScale = scale;
            
            if (material != null)
            {
                cube.GetComponent<Renderer>().material = material;
            }
            
            return cube;
        }
        
        public static GameObject CreateLowPolyPlane(string name, Vector3 position, Vector3 scale, Material material)
        {
            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.name = name;
            plane.transform.position = position;
            plane.transform.localScale = scale;
            
            if (material != null)
            {
                plane.GetComponent<Renderer>().material = material;
            }
            
            return plane;
        }
        
        public static GameObject CreateWorkbench(Vector3 position, Material material)
        {
            GameObject workbench = new GameObject("Workbench");
            workbench.transform.position = position;
            
            // Основная поверхность верстака
            GameObject surface = CreateLowPolyCube("Surface", position + Vector3.up * 0.9f, new Vector3(2f, 0.1f, 1f), material);
            surface.transform.SetParent(workbench.transform);
            
            // Ножки верстака
            GameObject leg1 = CreateLowPolyCube("Leg1", position + new Vector3(-0.8f, 0.45f, -0.4f), new Vector3(0.1f, 0.9f, 0.1f), material);
            leg1.transform.SetParent(workbench.transform);
            
            GameObject leg2 = CreateLowPolyCube("Leg2", position + new Vector3(0.8f, 0.45f, -0.4f), new Vector3(0.1f, 0.9f, 0.1f), material);
            leg2.transform.SetParent(workbench.transform);
            
            GameObject leg3 = CreateLowPolyCube("Leg3", position + new Vector3(-0.8f, 0.45f, 0.4f), new Vector3(0.1f, 0.9f, 0.1f), material);
            leg3.transform.SetParent(workbench.transform);
            
            GameObject leg4 = CreateLowPolyCube("Leg4", position + new Vector3(0.8f, 0.45f, 0.4f), new Vector3(0.1f, 0.9f, 0.1f), material);
            leg4.transform.SetParent(workbench.transform);
            
            return workbench;
        }
        
        public static GameObject CreateComputer(Vector3 position, Material material)
        {
            GameObject computer = new GameObject("Computer");
            computer.transform.position = position;
            
            // Монитор
            GameObject monitor = CreateLowPolyCube("Monitor", position + Vector3.up * 1.2f, new Vector3(0.8f, 0.6f, 0.1f), material);
            monitor.transform.SetParent(computer.transform);
            
            // Подставка монитора
            GameObject stand = CreateLowPolyCube("Stand", position + Vector3.up * 0.9f, new Vector3(0.1f, 0.6f, 0.1f), material);
            stand.transform.SetParent(computer.transform);
            
            // Клавиатура
            GameObject keyboard = CreateLowPolyCube("Keyboard", position + Vector3.up * 0.8f, new Vector3(0.6f, 0.05f, 0.2f), material);
            keyboard.transform.SetParent(computer.transform);
            
            return computer;
        }
        
        public static GameObject CreateBulletinBoard(Vector3 position, Material material)
        {
            GameObject board = new GameObject("BulletinBoard");
            board.transform.position = position;
            
            // Основная доска
            GameObject mainBoard = CreateLowPolyCube("MainBoard", position + Vector3.up * 1.5f, new Vector3(1.5f, 1f, 0.05f), material);
            mainBoard.transform.SetParent(board.transform);
            
            // Рамка доски
            GameObject frame = CreateLowPolyCube("Frame", position + Vector3.up * 1.5f, new Vector3(1.6f, 1.1f, 0.1f), material);
            frame.transform.SetParent(board.transform);
            
            return board;
        }
        
        public static GameObject CreateSafe(Vector3 position, Material material)
        {
            GameObject safe = new GameObject("Safe");
            safe.transform.position = position;
            
            // Основной корпус сейфа
            GameObject body = CreateLowPolyCube("Body", position + Vector3.up * 0.5f, new Vector3(0.8f, 1f, 0.6f), material);
            body.transform.SetParent(safe.transform);
            
            // Дверца сейфа
            GameObject door = CreateLowPolyCube("Door", position + Vector3.up * 0.5f + Vector3.forward * 0.31f, new Vector3(0.7f, 0.9f, 0.02f), material);
            door.transform.SetParent(safe.transform);
            
            // Ручка сейфа
            GameObject handle = CreateLowPolyCube("Handle", position + Vector3.up * 0.5f + Vector3.forward * 0.32f, new Vector3(0.1f, 0.1f, 0.05f), material);
            handle.transform.SetParent(safe.transform);
            
            return safe;
        }
        
        public static GameObject CreateDoor(Vector3 position, Material material)
        {
            GameObject door = new GameObject("Door");
            door.transform.position = position;
            
            // Дверная коробка
            GameObject frame = CreateLowPolyCube("Frame", position + Vector3.up * 1.5f, new Vector3(1.2f, 3f, 0.2f), material);
            frame.transform.SetParent(door.transform);
            
            // Дверное полотно
            GameObject doorPanel = CreateLowPolyCube("DoorPanel", position + Vector3.up * 1.5f, new Vector3(1f, 2.8f, 0.05f), material);
            doorPanel.transform.SetParent(door.transform);
            
            // Ручка двери
            GameObject handle = CreateLowPolyCube("Handle", position + Vector3.up * 1.5f + Vector3.right * 0.45f, new Vector3(0.05f, 0.1f, 0.05f), material);
            handle.transform.SetParent(door.transform);
            
            return door;
        }
        
        public static GameObject CreateGarageStructure(Vector3 position, Vector3 size, Material wallMaterial, Material floorMaterial, Material ceilingMaterial)
        {
            GameObject garage = new GameObject("GarageStructure");
            garage.transform.position = position;
            
            // Пол
            GameObject floor = CreateLowPolyPlane("Floor", position, new Vector3(size.x / 10f, 1f, size.z / 10f), floorMaterial);
            floor.transform.SetParent(garage.transform);
            
            // Потолок
            GameObject ceiling = CreateLowPolyPlane("Ceiling", position + Vector3.up * size.y, new Vector3(size.x / 10f, 1f, size.z / 10f), ceilingMaterial);
            ceiling.transform.SetParent(garage.transform);
            
            // Стены
            // Задняя стена
            GameObject backWall = CreateLowPolyCube("BackWall", position + new Vector3(0, size.y / 2f, -size.z / 2f), new Vector3(size.x, size.y, 0.2f), wallMaterial);
            backWall.transform.SetParent(garage.transform);
            
            // Передняя стена (с дверью)
            GameObject frontWall = CreateLowPolyCube("FrontWall", position + new Vector3(0, size.y / 2f, size.z / 2f), new Vector3(size.x, size.y, 0.2f), wallMaterial);
            frontWall.transform.SetParent(garage.transform);
            
            // Левая стена
            GameObject leftWall = CreateLowPolyCube("LeftWall", position + new Vector3(-size.x / 2f, size.y / 2f, 0), new Vector3(0.2f, size.y, size.z), wallMaterial);
            leftWall.transform.SetParent(garage.transform);
            
            // Правая стена
            GameObject rightWall = CreateLowPolyCube("RightWall", position + new Vector3(size.x / 2f, size.y / 2f, 0), new Vector3(0.2f, size.y, size.z), wallMaterial);
            rightWall.transform.SetParent(garage.transform);
            
            return garage;
        }
    }
}
