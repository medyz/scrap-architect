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
        
        // Новые методы для деталей интерьера
        
        public static GameObject CreateToolShelf(Vector3 position, Material material)
        {
            GameObject shelf = new GameObject("ToolShelf");
            shelf.transform.position = position;
            
            // Основная полка
            GameObject mainShelf = CreateLowPolyCube("MainShelf", position + Vector3.up * 1.8f, new Vector3(2f, 0.1f, 0.6f), material);
            mainShelf.transform.SetParent(shelf.transform);
            
            // Подставки для полки
            GameObject support1 = CreateLowPolyCube("Support1", position + new Vector3(-0.8f, 0.9f, 0), new Vector3(0.1f, 1.8f, 0.1f), material);
            support1.transform.SetParent(shelf.transform);
            
            GameObject support2 = CreateLowPolyCube("Support2", position + new Vector3(0.8f, 0.9f, 0), new Vector3(0.1f, 1.8f, 0.1f), material);
            support2.transform.SetParent(shelf.transform);
            
            // Инструменты на полке
            GameObject hammer = CreateLowPolyCube("Hammer", position + Vector3.up * 1.85f + Vector3.right * 0.3f, new Vector3(0.3f, 0.05f, 0.1f), material);
            hammer.transform.SetParent(shelf.transform);
            
            GameObject wrench = CreateLowPolyCube("Wrench", position + Vector3.up * 1.85f + Vector3.left * 0.3f, new Vector3(0.4f, 0.05f, 0.1f), material);
            wrench.transform.SetParent(shelf.transform);
            
            GameObject screwdriver = CreateLowPolyCube("Screwdriver", position + Vector3.up * 1.85f + Vector3.right * 0.8f, new Vector3(0.2f, 0.05f, 0.05f), material);
            screwdriver.transform.SetParent(shelf.transform);
            
            return shelf;
        }
        
        public static GameObject CreateBlueprintWall(Vector3 position, Material material)
        {
            GameObject blueprintWall = new GameObject("BlueprintWall");
            blueprintWall.transform.position = position;
            
            // Чертежи на стене
            GameObject blueprint1 = CreateLowPolyCube("Blueprint1", position + Vector3.up * 2f + Vector3.right * 0.5f, new Vector3(0.8f, 0.6f, 0.02f), material);
            blueprint1.transform.SetParent(blueprintWall.transform);
            
            GameObject blueprint2 = CreateLowPolyCube("Blueprint2", position + Vector3.up * 2f + Vector3.left * 0.5f, new Vector3(0.6f, 0.8f, 0.02f), material);
            blueprint2.transform.SetParent(blueprintWall.transform);
            
            // Булавки для чертежей
            GameObject pin1 = CreateLowPolyCube("Pin1", position + Vector3.up * 2.3f + Vector3.right * 0.5f + Vector3.forward * 0.01f, new Vector3(0.02f, 0.02f, 0.02f), material);
            pin1.transform.SetParent(blueprintWall.transform);
            
            GameObject pin2 = CreateLowPolyCube("Pin2", position + Vector3.up * 2.4f + Vector3.left * 0.5f + Vector3.forward * 0.01f, new Vector3(0.02f, 0.02f, 0.02f), material);
            pin2.transform.SetParent(blueprintWall.transform);
            
            return blueprintWall;
        }
        
        public static GameObject CreateWorkbenchTools(Vector3 position, Material material)
        {
            GameObject tools = new GameObject("WorkbenchTools");
            tools.transform.position = position;
            
            // Молоток на верстаке
            GameObject hammer = CreateLowPolyCube("Hammer", position + Vector3.up * 0.95f + Vector3.right * 0.3f, new Vector3(0.2f, 0.05f, 0.05f), material);
            hammer.transform.SetParent(tools.transform);
            
            // Отвертка
            GameObject screwdriver = CreateLowPolyCube("Screwdriver", position + Vector3.up * 0.95f + Vector3.left * 0.2f, new Vector3(0.15f, 0.03f, 0.03f), material);
            screwdriver.transform.SetParent(tools.transform);
            
            // Гаечный ключ
            GameObject wrench = CreateLowPolyCube("Wrench", position + Vector3.up * 0.95f + Vector3.right * 0.6f, new Vector3(0.25f, 0.04f, 0.04f), material);
            wrench.transform.SetParent(tools.transform);
            
            // Болты и гайки
            GameObject bolts = CreateLowPolyCube("Bolts", position + Vector3.up * 0.95f + Vector3.left * 0.5f, new Vector3(0.1f, 0.02f, 0.1f), material);
            bolts.transform.SetParent(tools.transform);
            
            return tools;
        }
        
        public static GameObject CreateFloorDetails(Vector3 position, Material material)
        {
            GameObject floorDetails = new GameObject("FloorDetails");
            floorDetails.transform.position = position;
            
            // Пятна масла
            GameObject oilStain1 = CreateLowPolyCube("OilStain1", position + Vector3.up * 0.01f + Vector3.right * 2f, new Vector3(0.5f, 0.01f, 0.3f), material);
            oilStain1.transform.SetParent(floorDetails.transform);
            
            GameObject oilStain2 = CreateLowPolyCube("OilStain2", position + Vector3.up * 0.01f + Vector3.left * 1.5f, new Vector3(0.3f, 0.01f, 0.4f), material);
            oilStain2.transform.SetParent(floorDetails.transform);
            
            // Металлические обрезки
            GameObject scrap1 = CreateLowPolyCube("Scrap1", position + Vector3.up * 0.02f + Vector3.right * 3f, new Vector3(0.2f, 0.02f, 0.1f), material);
            scrap1.transform.SetParent(floorDetails.transform);
            
            GameObject scrap2 = CreateLowPolyCube("Scrap2", position + Vector3.up * 0.02f + Vector3.left * 2.5f, new Vector3(0.15f, 0.02f, 0.15f), material);
            scrap2.transform.SetParent(floorDetails.transform);
            
            return floorDetails;
        }
        
        public static GameObject CreateLightFixture(Vector3 position, Material material)
        {
            GameObject lightFixture = new GameObject("LightFixture");
            lightFixture.transform.position = position;
            
            // Основание светильника
            GameObject baseFixture = CreateLowPolyCube("Base", position + Vector3.up * 3.5f, new Vector3(0.3f, 0.1f, 0.3f), material);
            baseFixture.transform.SetParent(lightFixture.transform);
            
            // Цепь для подвешивания
            GameObject chain = CreateLowPolyCube("Chain", position + Vector3.up * 3.8f, new Vector3(0.05f, 0.3f, 0.05f), material);
            chain.transform.SetParent(lightFixture.transform);
            
            // Абажур
            GameObject lampshade = CreateLowPolyCube("Lampshade", position + Vector3.up * 4.1f, new Vector3(0.4f, 0.2f, 0.4f), material);
            lampshade.transform.SetParent(lightFixture.transform);
            
            return lightFixture;
        }
    }
}
