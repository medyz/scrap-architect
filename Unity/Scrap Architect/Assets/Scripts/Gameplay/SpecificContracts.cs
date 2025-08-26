using UnityEngine;
using System;
using System.Collections.Generic;

namespace ScrapArchitect.Gameplay
{
    /// <summary>
    /// Конкретные контракты для игры Scrap Architect
    /// </summary>
    public static class SpecificContracts
    {
        /// <summary>
        /// Создать контракт "Доставка арбузов"
        /// </summary>
        public static Contract CreateWatermelonDeliveryContract()
        {
            Contract contract = new Contract();
            contract.title = "Доставка арбузов";
            contract.description = "Бабушка Мария просит доставить арбузы с поля в её сарай. Будьте осторожны - арбузы очень хрупкие!";
            contract.contractType = ContractType.Delivery;
            contract.difficulty = ContractDifficulty.Easy;
            contract.clientName = "Бабушка Мария";
            contract.clientDescription = "Добрая старушка, которая выращивает самые сладкие арбузы в округе";
            contract.timeLimit = 120f; // 2 минуты
            
            // Начальная и конечная позиции
            contract.startLocation = new Vector3(-20f, 0f, -20f);
            contract.finishLocation = new Vector3(20f, 0f, 20f);
            
            // Цель: доставить арбузы
            ContractObjective objective = new ContractObjective();
            objective.description = "Доставить арбузы в сарай бабушки Марии";
            objective.type = ContractObjective.ObjectiveType.DeliverItems;
            objective.targetValue = 3f; // 3 арбуза
            contract.objectives.Add(objective);
            
            // Бонусная цель: не разбить ни одного арбуза
            ContractObjective bonusObjective = new ContractObjective();
            bonusObjective.description = "Доставить все арбузы целыми (бонус)";
            bonusObjective.type = ContractObjective.ObjectiveType.DeliverItems;
            bonusObjective.targetValue = 3f;
            bonusObjective.isOptional = true;
            contract.objectives.Add(bonusObjective);
            
            // Награды
            contract.reward.scrapReward = 150;
            contract.reward.experienceReward = 75;
            contract.reward.unlockParts.Add("watermelon_crate");
            
            // Бонусная награда за быстрое выполнение
            contract.bonusReward.scrapReward = 50;
            contract.bonusReward.experienceReward = 25;
            
            contract.tags.Add("delivery");
            contract.tags.Add("fragile");
            contract.tags.Add("beginner");
            
            return contract;
        }
        
        /// <summary>
        /// Создать контракт "Покраска забора"
        /// </summary>
        public static Contract CreateFencePaintingContract()
        {
            Contract contract = new Contract();
            contract.title = "Покраска забора";
            contract.description = "Мэр города хочет, чтобы забор вокруг парка был покрашен в яркие цвета. Используйте краскопульт!";
            contract.contractType = ContractType.Construction;
            contract.difficulty = ContractDifficulty.Medium;
            contract.clientName = "Мэр Петров";
            contract.clientDescription = "Педантичный мэр, который любит порядок и чистоту";
            contract.timeLimit = 180f; // 3 минуты
            
            // Цель: покрасить забор
            ContractObjective objective = new ContractObjective();
            objective.description = "Покрасить 80% поверхности забора";
            objective.type = ContractObjective.ObjectiveType.UseTool;
            objective.targetValue = 80f; // 80% забора
            contract.objectives.Add(objective);
            
            // Бонусная цель: покрасить за 2 минуты
            ContractObjective bonusObjective = new ContractObjective();
            bonusObjective.description = "Покрасить забор за 2 минуты (бонус)";
            bonusObjective.type = ContractObjective.ObjectiveType.SurviveTime;
            bonusObjective.targetValue = 120f;
            bonusObjective.isOptional = true;
            contract.objectives.Add(bonusObjective);
            
            // Награды
            contract.reward.scrapReward = 200;
            contract.reward.experienceReward = 100;
            contract.reward.unlockParts.Add("paint_sprayer");
            contract.reward.unlockParts.Add("color_palette");
            
            // Бонусная награда
            contract.bonusReward.scrapReward = 100;
            contract.bonusReward.experienceReward = 50;
            
            contract.tags.Add("painting");
            contract.tags.Add("precision");
            contract.tags.Add("tools");
            
            return contract;
        }
        
        /// <summary>
        /// Создать контракт "Уборка двора"
        /// </summary>
        public static Contract CreateYardCleaningContract()
        {
            Contract contract = new Contract();
            contract.title = "Уборка двора";
            contract.description = "Двор бабушки Марии зарос сорняками и завален мусором. Помогите ей навести порядок!";
            contract.contractType = ContractType.Collection;
            contract.difficulty = ContractDifficulty.Easy;
            contract.clientName = "Бабушка Мария";
            contract.clientDescription = "Добрая старушка, которая выращивает самые сладкие арбузы в округе";
            contract.timeLimit = 150f; // 2.5 минуты
            
            // Цель: собрать мусор
            ContractObjective objective = new ContractObjective();
            objective.description = "Собрать весь мусор со двора";
            objective.type = ContractObjective.ObjectiveType.CollectItems;
            objective.targetValue = 15f; // 15 предметов мусора
            contract.objectives.Add(objective);
            
            // Дополнительная цель: вырвать сорняки
            ContractObjective weedObjective = new ContractObjective();
            weedObjective.description = "Вырвать сорняки";
            weedObjective.type = ContractObjective.ObjectiveType.CollectItems;
            weedObjective.targetValue = 10f; // 10 сорняков
            contract.objectives.Add(weedObjective);
            
            // Награды
            contract.reward.scrapReward = 120;
            contract.reward.experienceReward = 60;
            contract.reward.unlockParts.Add("vacuum_cleaner");
            contract.reward.unlockParts.Add("weed_puller");
            
            contract.tags.Add("cleaning");
            contract.tags.Add("collection");
            contract.tags.Add("beginner");
            
            return contract;
        }
        
        /// <summary>
        /// Создать контракт "Гонка на газонокосилках"
        /// </summary>
        public static Contract CreateLawnmowerRaceContract()
        {
            Contract contract = new Contract();
            contract.title = "Гонка на газонокосилках";
            contract.description = "Сосед Иван бросил вызов! Кто быстрее проедет на газонокосилке по сложной трассе?";
            contract.contractType = ContractType.Racing;
            contract.difficulty = ContractDifficulty.Hard;
            contract.clientName = "Сосед Иван";
            contract.clientDescription = "Конкурентный сосед, который всегда хочет быть первым";
            contract.timeLimit = 90f; // 1.5 минуты
            
            // Чекпоинты для гонки
            Vector3[] checkpoints = {
                new Vector3(0f, 0f, 0f),
                new Vector3(15f, 0f, 10f),
                new Vector3(25f, 0f, -5f),
                new Vector3(10f, 0f, -15f),
                new Vector3(-10f, 0f, -10f),
                new Vector3(-15f, 0f, 5f),
                new Vector3(0f, 0f, 0f)
            };
            contract.checkpoints = checkpoints;
            
            // Цель: пройти все чекпоинты
            for (int i = 0; i < checkpoints.Length; i++)
            {
                ContractObjective objective = new ContractObjective();
                objective.description = $"Достичь чекпоинта {i + 1}";
                objective.type = ContractObjective.ObjectiveType.ReachLocation;
                objective.targetValue = 1f;
                contract.objectives.Add(objective);
            }
            
            // Бонусная цель: обогнать соседа
            ContractObjective bonusObjective = new ContractObjective();
            bonusObjective.description = "Финишировать первым (бонус)";
            bonusObjective.type = ContractObjective.ObjectiveType.ReachLocation;
            bonusObjective.targetValue = 1f;
            bonusObjective.isOptional = true;
            contract.objectives.Add(bonusObjective);
            
            // Награды
            contract.reward.scrapReward = 300;
            contract.reward.experienceReward = 150;
            contract.reward.unlockParts.Add("racing_lawnmower");
            contract.reward.unlockParts.Add("turbo_engine");
            
            // Бонусная награда за победу
            contract.bonusReward.scrapReward = 200;
            contract.bonusReward.experienceReward = 100;
            contract.bonusReward.unlockParts.Add("golden_trophy");
            
            contract.tags.Add("racing");
            contract.tags.Add("competition");
            contract.tags.Add("advanced");
            
            return contract;
        }
        
        /// <summary>
        /// Создать контракт "Сбор цветов"
        /// </summary>
        public static Contract CreateFlowerCollectionContract()
        {
            Contract contract = new Contract();
            contract.title = "Сбор цветов";
            contract.description = "Флорист Анна нуждается в свежих цветах для свадебного букета. Соберите самые красивые!";
            contract.contractType = ContractType.Collection;
            contract.difficulty = ContractDifficulty.Medium;
            contract.clientName = "Флорист Анна";
            contract.clientDescription = "Талантливая флористка с тонким вкусом";
            contract.timeLimit = 120f; // 2 минуты
            
            // Цель: собрать цветы
            ContractObjective objective = new ContractObjective();
            objective.description = "Собрать 20 цветов разных видов";
            objective.type = ContractObjective.ObjectiveType.CollectItems;
            objective.targetValue = 20f;
            contract.objectives.Add(objective);
            
            // Бонусная цель: собрать редкие цветы
            ContractObjective bonusObjective = new ContractObjective();
            bonusObjective.description = "Собрать 5 редких цветов (бонус)";
            bonusObjective.type = ContractObjective.ObjectiveType.CollectItems;
            bonusObjective.targetValue = 5f;
            bonusObjective.isOptional = true;
            contract.objectives.Add(bonusObjective);
            
            // Награды
            contract.reward.scrapReward = 180;
            contract.reward.experienceReward = 90;
            contract.reward.unlockParts.Add("flower_basket");
            contract.reward.unlockParts.Add("gardening_gloves");
            
            // Бонусная награда
            contract.bonusReward.scrapReward = 90;
            contract.bonusReward.experienceReward = 45;
            contract.bonusReward.unlockParts.Add("rare_seeds");
            
            contract.tags.Add("collection");
            contract.tags.Add("nature");
            contract.tags.Add("precision");
            
            return contract;
        }
        
        /// <summary>
        /// Создать контракт "Перевозка яиц"
        /// </summary>
        public static Contract CreateEggTransportContract()
        {
            Contract contract = new Contract();
            contract.title = "Перевозка яиц";
            contract.description = "Фермер Петр просит перевезти яйца из курятника в магазин. Осторожно - яйца очень хрупкие!";
            contract.contractType = ContractType.Transport;
            contract.difficulty = ContractDifficulty.Hard;
            contract.clientName = "Фермер Петр";
            contract.clientDescription = "Опытный фермер, который заботится о своих животных";
            contract.timeLimit = 180f; // 3 минуты
            
            // Начальная и конечная позиции
            contract.startLocation = new Vector3(-30f, 0f, -30f);
            contract.finishLocation = new Vector3(30f, 0f, 30f);
            
            // Цель: доставить яйца
            ContractObjective objective = new ContractObjective();
            objective.description = "Доставить 12 яиц в магазин";
            objective.type = ContractObjective.ObjectiveType.DeliverItems;
            objective.targetValue = 12f;
            contract.objectives.Add(objective);
            
            // Бонусная цель: не разбить ни одного яйца
            ContractObjective bonusObjective = new ContractObjective();
            bonusObjective.description = "Доставить все яйца целыми (бонус)";
            bonusObjective.type = ContractObjective.ObjectiveType.DeliverItems;
            bonusObjective.targetValue = 12f;
            bonusObjective.isOptional = true;
            contract.objectives.Add(bonusObjective);
            
            // Награды
            contract.reward.scrapReward = 250;
            contract.reward.experienceReward = 125;
            contract.reward.unlockParts.Add("egg_carton");
            contract.reward.unlockParts.Add("suspension_system");
            
            // Бонусная награда
            contract.bonusReward.scrapReward = 150;
            contract.bonusReward.experienceReward = 75;
            contract.bonusReward.unlockParts.Add("shock_absorber");
            
            contract.tags.Add("transport");
            contract.tags.Add("fragile");
            contract.tags.Add("advanced");
            
            return contract;
        }
        
        /// <summary>
        /// Создать контракт "Победа над соседом"
        /// </summary>
        public static Contract CreateNeighborVictoryContract()
        {
            Contract contract = new Contract();
            contract.title = "Победа над соседом";
            contract.description = "Сосед Иван снова хвастается своим новым изобретением. Покажите ему, кто здесь настоящий мастер!";
            contract.contractType = ContractType.Racing;
            contract.difficulty = ContractDifficulty.Expert;
            contract.clientName = "Сосед Иван";
            contract.clientDescription = "Конкурентный сосед, который всегда хочет быть первым";
            contract.timeLimit = 120f; // 2 минуты
            
            // Сложная трасса с препятствиями
            Vector3[] checkpoints = {
                new Vector3(0f, 0f, 0f),
                new Vector3(20f, 0f, 15f),
                new Vector3(35f, 0f, -10f),
                new Vector3(15f, 0f, -25f),
                new Vector3(-15f, 0f, -20f),
                new Vector3(-25f, 0f, 10f),
                new Vector3(-10f, 0f, 25f),
                new Vector3(0f, 0f, 0f)
            };
            contract.checkpoints = checkpoints;
            
            // Цель: пройти трассу
            for (int i = 0; i < checkpoints.Length; i++)
            {
                ContractObjective objective = new ContractObjective();
                objective.description = $"Достичь чекпоинта {i + 1}";
                objective.type = ContractObjective.ObjectiveType.ReachLocation;
                objective.targetValue = 1f;
                contract.objectives.Add(objective);
            }
            
            // Бонусная цель: установить рекорд
            ContractObjective bonusObjective = new ContractObjective();
            bonusObjective.description = "Установить новый рекорд трассы (бонус)";
            bonusObjective.type = ContractObjective.ObjectiveType.SurviveTime;
            bonusObjective.targetValue = 90f; // За 90 секунд
            bonusObjective.isOptional = true;
            contract.objectives.Add(bonusObjective);
            
            // Награды
            contract.reward.scrapReward = 500;
            contract.reward.experienceReward = 250;
            contract.reward.unlockParts.Add("champion_trophy");
            contract.reward.unlockParts.Add("experimental_engine");
            contract.reward.unlockParts.Add("neighbor_respect");
            
            // Бонусная награда
            contract.bonusReward.scrapReward = 300;
            contract.bonusReward.experienceReward = 150;
            contract.bonusReward.unlockParts.Add("legendary_parts");
            
            contract.tags.Add("racing");
            contract.tags.Add("competition");
            contract.tags.Add("expert");
            contract.tags.Add("story");
            
            return contract;
        }
        
        /// <summary>
        /// Получить все конкретные контракты
        /// </summary>
        public static List<Contract> GetAllSpecificContracts()
        {
            List<Contract> contracts = new List<Contract>
            {
                CreateWatermelonDeliveryContract(),
                CreateFencePaintingContract(),
                CreateYardCleaningContract(),
                CreateLawnmowerRaceContract(),
                CreateFlowerCollectionContract(),
                CreateEggTransportContract(),
                CreateNeighborVictoryContract()
            };
            
            return contracts;
        }
        
        /// <summary>
        /// Получить контракты по сложности
        /// </summary>
        public static List<Contract> GetContractsByDifficulty(ContractDifficulty difficulty)
        {
            List<Contract> allContracts = GetAllSpecificContracts();
            List<Contract> filteredContracts = new List<Contract>();
            
            foreach (var contract in allContracts)
            {
                if (contract.difficulty == difficulty)
                {
                    filteredContracts.Add(contract);
                }
            }
            
            return filteredContracts;
        }
        
        /// <summary>
        /// Получить контракты по типу
        /// </summary>
        public static List<Contract> GetContractsByType(ContractType type)
        {
            List<Contract> allContracts = GetAllSpecificContracts();
            List<Contract> filteredContracts = new List<Contract>();
            
            foreach (var contract in allContracts)
            {
                if (contract.contractType == type)
                {
                    filteredContracts.Add(contract);
                }
            }
            
            return filteredContracts;
        }
        
        /// <summary>
        /// Получить контракты по тегу
        /// </summary>
        public static List<Contract> GetContractsByTag(string tag)
        {
            List<Contract> allContracts = GetAllSpecificContracts();
            List<Contract> filteredContracts = new List<Contract>();
            
            foreach (var contract in allContracts)
            {
                if (contract.tags.Contains(tag))
                {
                    filteredContracts.Add(contract);
                }
            }
            
            return filteredContracts;
        }
    }
}
