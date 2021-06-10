using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataControl : MonoBehaviour
{
    public static DataControl control = null;
    public event Action OnLoad;
    public event Action OnSafe;
    public event Action OnInventoryLoad;
    public event Action OnInventorySave;

    [field: HideInInspector] public int Health;
    [field: HideInInspector] public int Mana;
    [field: HideInInspector] public Vector3 PlayerPosition;
    [field: HideInInspector] public int MaxHealth;
    [field: HideInInspector] public int MaxMana;
    [field: HideInInspector] public int Level;
    [field: HideInInspector] public InventoryObject[] inventories;
    [field: HideInInspector] public int CurrentEXP;
    [field: HideInInspector] public int StatPoints;
    [field: HideInInspector] public int Vitality;
    [field: HideInInspector] public int Wisdom;
    [field: HideInInspector] public int Strength;
    [field: HideInInspector] public int Dexterity;
    [field: HideInInspector] public int Intelligence;
    [field: HideInInspector] public int Gold;
    [field: HideInInspector] public List<GroundItem> groundItems;
    [field: HideInInspector] public List<Spawner> Spawners;
    [field: HideInInspector] public List<Quest> Quests;
    [field: SerializeField] public GroundItem groundItem;

    private void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(this);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }

        inventories = new InventoryObject[20];
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnNextScene;
        Directory.CreateDirectory(Application.persistentDataPath + "/saves");
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnNextScene;
    }

    private void OnNextScene(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (CheckForSave())
        {
            Load();
        }
    }

    public void LoadNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public bool CheckForSave()
    {
        if(Directory.GetFiles(Application.persistentDataPath + "/saves").Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckForWorldSave()
    {
        if (File.Exists(Application.persistentDataPath + "/saves/worldData" + SceneManager.GetActiveScene().buildIndex + ".ss"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckForInventorySave(int id)
    {
        if (File.Exists(Application.persistentDataPath + "/saves/inventory" + id + "Data.ss"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ContinueGame()
    {
        Quests = new List<Quest>();
        SceneManager.LoadScene(LoadCurrentSceneNumber());
    }

    public void NewGame(int buildIndex)
    {
        foreach (string file in Directory.GetFiles(Application.persistentDataPath + "/saves"))
        {
            File.Delete(file);
        }

        SceneManager.LoadScene(buildIndex);
    }

    public void Save()
    {
        SaveWorld();
        SaveCurrentSceneNumber();
        SaveQuests();
        SavePlayer();
    }

    public void SaveCurrentSceneNumber()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saves/sceneNumber.ss");

        int data = SceneManager.GetActiveScene().buildIndex;
        bf.Serialize(file, data);
        file.Close();
    }

    private int LoadCurrentSceneNumber()
    {
        if(File.Exists(Application.persistentDataPath + "/saves/sceneNumber.ss"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saves/sceneNumber.ss", FileMode.Open);
            int data = (int)bf.Deserialize(file);
            return data;
        }
        else
        {
            return 0;
        }
    }

    private void SavePlayer()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saves/playerData.ss");

        PlayerData data = new PlayerData();

        //Player data
        data.health = Health;
        data.mana = Mana;
        data.maxHealth = MaxHealth;
        data.maxMana = MaxMana;
        data.level = Level;
        data.currentEXP = CurrentEXP;
        data.statPoints = StatPoints;
        data.vitality = Vitality;
        data.wisdom = Wisdom;
        data.strength = Strength;
        data.dexterity = Dexterity;
        data.intelligence = Intelligence;
        data.gold = Gold;

        bf.Serialize(file, data);
        file.Close();
    }

    private void SaveQuests()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saves/questData.ss");

        List<QuestData> data = new List<QuestData>();

        foreach (Quest quest in Quests)
        {
            data.Add(new QuestData
            {
                Name = quest.Name,
                accepted = quest.Accepted,
                completed = quest.Completed,
                IsHandedIn = quest.IsHandedIN,
                questStep = (int)quest.QuestStep,
                objectName = quest.gameObject.name,
                className = quest.gameObject.GetComponent<Quest>().GetType().Name
            });
        }

        bf.Serialize(file, data);
        file.Close();
    }

    private void SaveWorld()
    {
        groundItems = new List<GroundItem>();
        OnSafe?.Invoke();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saves/worldData" + SceneManager.GetActiveScene().buildIndex + ".ss");

        //Dropped items data
        List<GroundItemData> data = new List<GroundItemData>();
        WorldData worldData = new WorldData();

        worldData.positionX = PlayerPosition.x;
        worldData.positionY = PlayerPosition.y;

        if (groundItems.Count > 0)
        {
            foreach (GroundItem ditem in groundItems)
            {
                if (ditem.Item.buffs != null)
                {
                    BuffData[] buffs = new BuffData[ditem.Item.buffs.Length];

                    for (int i = 0; i < ditem.Item.buffs.Length; i++)
                    {
                        buffs[i] = new BuffData()
                        {
                            stat = (int)ditem.Item.buffs[i].stat,
                            value = ditem.Item.buffs[i].Value,
                            max = ditem.Item.buffs[i].max,
                            min = ditem.Item.buffs[i].min,
                        };
                    }
                }

                string tempName = null;
                int _minDamage = 0;
                int _maxDamage = 0;
                if (ditem.Item is CombatItem)
                {
                    tempName = ((CombatItem)ditem.Item).combatItemObject.gameObject.name.ToString();
                    _minDamage = ((CombatItem)ditem.Item).MinDamage;
                    _maxDamage = ((CombatItem)ditem.Item).MaxDamage;
                }

                int _armorSubType = 0;
                if (ditem.Item is EquipmentObject)
                {
                    _armorSubType = (int)((EquipmentObject)ditem.Item).armorSubType;
                }

                int _replenishAmount = 0;
                string _drinkSound = "";
                if (ditem.Item is HealthPotion)
                {
                    _replenishAmount = ((HealthPotion)ditem.Item).ReplenishAmount;
                    _drinkSound = ((HealthPotion)ditem.Item).drinkSound.name;
                }

                if (ditem.Item is ManaPotion)
                {
                    _replenishAmount = ((ManaPotion)ditem.Item).ReplenishAmount;
                    _drinkSound = ((ManaPotion)ditem.Item).drinkSound.name;
                }

                data.Add(new GroundItemData
                {
                    item = new ItemObjectData()
                    {
                        name = ditem.Item.name,
                        id = ditem.Item.Id,
                        spriteLink = ditem.Item.uiDisplay.name,
                        className = ditem.Item.GetType().ToString(),
                        itemType = (int)ditem.Item.type,
                        decription = ditem.Item.description,
                        stackSize = ditem.Item.stackSize,
                        weight = ditem.Item.weight,
                        value = ditem.Item.baseValue,
                        combatItemClassName = tempName,
                        rarity = (int)ditem.Item.rarity,
                        minDamage = _minDamage,
                        maxDamage = _maxDamage,
                        armorSubType = _armorSubType,
                        regenAmount = _replenishAmount,
                        drinkSound = _drinkSound
                    },
                    amount = ditem.Amount,
                    positionX = ditem.gameObject.transform.position.x,
                    positionY = ditem.gameObject.transform.position.y
                });
            }
        }

        List<SpawnerData> data2 = new List<SpawnerData>();

        if(Spawners.Count > 0)
        foreach (Spawner spawner in Spawners)
        {
            data2.Add(new SpawnerData
            {
                isDead = spawner.IsDead
            });
        }

        worldData.groundItemDatas = data;
        worldData.spawnerDatas = data2;

        bf.Serialize(file, worldData);
        file.Close();
    }

    public void SaveInventory(int inventoryId)
    {
        OnInventorySave?.Invoke();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saves/inventory" + inventoryId + "Data.ss");
        InventoryData tempObj = new InventoryData();
        InventoryContainerData tempContainer = new InventoryContainerData();
        tempContainer.items = new InventorySlotData[inventories[inventoryId].Container.items.Length];

        //Make inventory
        for (int i = 0; i < inventories[inventoryId].Container.items.Length; i++)
        {
            if (inventories[inventoryId].Container.items[i] != null)
            {
                tempContainer.items[i] = new InventorySlotData();
                tempContainer.items[i].id = inventories[inventoryId].Container.items[i].ID;
                tempContainer.items[i].amount = inventories[inventoryId].Container.items[i].amount;
                tempContainer.items[i].weight = inventories[inventoryId].Container.items[i].weight;
                tempContainer.items[i].value = inventories[inventoryId].Container.items[i].value;

                if (inventories[inventoryId].Container.items[i].item != null)
                {
                    tempContainer.items[i].item = new ItemObjectData();

                    string tempName = null;
                    int _minDamage = 0;
                    int _maxDamage = 0;

                    if (inventories[inventoryId].Container.items[i].item is CombatItem)
                    {
                        tempName = ((CombatItem)inventories[inventoryId].Container.items[i].item).combatItemObject.gameObject.name.ToString();
                        _minDamage = ((CombatItem)inventories[inventoryId].Container.items[i].item).MinDamage;
                        _maxDamage = ((CombatItem)inventories[inventoryId].Container.items[i].item).MaxDamage;
                    }

                    if (inventories[inventoryId].Container.items[i].item is EquipmentObject)
                    {
                        tempContainer.items[i].item.armorSubType = (int)((EquipmentObject)inventories[inventoryId].Container.items[i].item).armorSubType;
                    }

                    if (inventories[inventoryId].Container.items[i].item is HealthPotion)
                    {
                        tempContainer.items[i].item.regenAmount = ((HealthPotion)inventories[inventoryId].Container.items[i].item).ReplenishAmount;
                        tempContainer.items[i].item.drinkSound = ((HealthPotion)inventories[inventoryId].Container.items[i].item).drinkSound.name;
                    }
                    
                    if (inventories[inventoryId].Container.items[i].item is ManaPotion)
                    {
                        tempContainer.items[i].item.regenAmount = ((ManaPotion)inventories[inventoryId].Container.items[i].item).ReplenishAmount;
                        tempContainer.items[i].item.drinkSound = ((ManaPotion)inventories[inventoryId].Container.items[i].item).drinkSound.name;
                    }

                    tempContainer.items[i].item.name = inventories[inventoryId].Container.items[i].item.name;
                    tempContainer.items[i].item.id = inventories[inventoryId].Container.items[i].item.Id;
                    tempContainer.items[i].item.className = inventories[inventoryId].Container.items[i].item.GetType().ToString();
                    tempContainer.items[i].item.spriteLink = inventories[inventoryId].Container.items[i].item.uiDisplay.name;
                    tempContainer.items[i].item.itemType = (int)inventories[inventoryId].Container.items[i].item.type;
                    tempContainer.items[i].item.decription = inventories[inventoryId].Container.items[i].item.description;
                    tempContainer.items[i].item.stackSize = inventories[inventoryId].Container.items[i].item.stackSize;
                    tempContainer.items[i].item.weight = inventories[inventoryId].Container.items[i].item.weight;
                    tempContainer.items[i].item.value = inventories[inventoryId].Container.items[i].item.baseValue;
                    tempContainer.items[i].item.rarity = (int)inventories[inventoryId].Container.items[i].item.rarity;
                    tempContainer.items[i].item.combatItemClassName = tempName;
                    tempContainer.items[i].item.minDamage = _minDamage;
                    tempContainer.items[i].item.maxDamage = _maxDamage;

                    if (inventories[inventoryId].Container.items[i].item.buffs != null)
                    {
                        tempContainer.items[i].item.buffs = new BuffData[inventories[inventoryId].Container.items[i].item.buffs.Length];
                        for (int b = 0; b < inventories[inventoryId].Container.items[i].item.buffs.Length; b++)
                        {
                            tempContainer.items[i].item.buffs[b] = new BuffData();
                            tempContainer.items[i].item.buffs[b].stat = (int)inventories[inventoryId].Container.items[i].item.buffs[b].stat;
                            tempContainer.items[i].item.buffs[b].value = (int)inventories[inventoryId].Container.items[i].item.buffs[b].Value;
                            tempContainer.items[i].item.buffs[b].min = (int)inventories[inventoryId].Container.items[i].item.buffs[b].min;
                            tempContainer.items[i].item.buffs[b].max = (int)inventories[inventoryId].Container.items[i].item.buffs[b].max;
                        }
                    }
                }
            }
        }

        tempObj.container = tempContainer;
        tempObj.inventoryId = inventories[inventoryId].InventoryID;
        tempObj.weight = inventories[inventoryId].weight;

        bf.Serialize(file, tempObj);
        file.Close();
    }

    public void Load()
    {
        if (!SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            LoadPlayer();
            LoadWorld();
            LoadQuests();

            OnLoad?.Invoke();
        }
    }

    private void LoadPlayer()
    {
        if (File.Exists(Application.persistentDataPath + "/saves/playerData.ss"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saves/playerData.ss", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            //Player data
            Health = data.health;
            Mana = data.mana;
            MaxHealth = data.maxHealth;
            MaxMana = data.maxMana;
            Level = data.level;
            CurrentEXP = data.currentEXP;
            StatPoints = data.statPoints;
            Vitality = data.vitality;
            Wisdom = data.wisdom;
            Strength = data.strength;
            Dexterity = data.dexterity;
            Intelligence = data.intelligence;
            Gold = data.gold;
        }
    }

    private void LoadQuests()
    {
        if (File.Exists(Application.persistentDataPath + "/saves/questData.ss"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saves/questData.ss", FileMode.Open);
            List<QuestData> data = (List<QuestData>)bf.Deserialize(file);
            file.Close();

            for (int i = 0; i <= data.Count-1; i++)
            {
                if (data.Count == 0)
                {
                    foreach (Quest quest in Quests)
                    {
                        quest.ResetQuest();
                    }
                    Quests.Clear();
                }

                if (Quests.Where(w => w.Name == data[i].Name).ToList().Count == 0)
                {
                    var tempQuest = Resources.Load(data[i].objectName);
                    Quest quest = (Quest)((GameObject)tempQuest).GetComponent(data[i].className);
                    quest.Init();
                    quest.Accepted = data[i].accepted;
                    quest.Completed = data[i].completed;
                    quest.IsHandedIN = data[i].IsHandedIn;
                    quest.QuestStep = (Quest.QuestSteps)data[i].questStep;
                    Quests.Add(quest);
                }
                else if(Quests.Count > data.Count && Quests.Count - 1 > i)
                {
                    Quests.RemoveAt(i);
                }
                else if (Quests[i].Name == data[i].Name)
                {
                    Quests[i].Accepted = data[i].accepted;
                    Quests[i].Completed = data[i].completed;
                    Quests[i].IsHandedIN = data[i].IsHandedIn;
                    Quests[i].QuestStep = (Quest.QuestSteps)data[i].questStep;
                }
            }
        }
    }

    private void LoadWorld()
    {
        if (File.Exists(Application.persistentDataPath + "/saves/worldData" + SceneManager.GetActiveScene().buildIndex + ".ss"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saves/worldData" + SceneManager.GetActiveScene().buildIndex + ".ss", FileMode.Open);
            WorldData data = (WorldData)bf.Deserialize(file);
            file.Close();

            //Dropped items data
            groundItems = new List<GroundItem>();
            foreach (GroundItemData ddata in data.groundItemDatas)
            {
                GroundItem obj = Instantiate(groundItem);
                if (ddata.item.buffs != null)
                {
                    ItemBuff[] buffs = new ItemBuff[ddata.item.buffs.Length];

                    for (int i = 0; i < ddata.item.buffs.Length; i++)
                    {
                        buffs[i] = new ItemBuff(ddata.item.buffs[i].min, ddata.item.buffs[i].max)
                        {
                            stat = (Stats)ddata.item.buffs[i].stat,
                            Value = ddata.item.buffs[i].value,
                            max = ddata.item.buffs[i].max,
                            min = ddata.item.buffs[i].min,
                        };
                    }
                    obj.Item.buffs = buffs;
                }

                obj.Item = (ItemObject)ScriptableObject.CreateInstance(ddata.item.className);
                if ((ItemType)ddata.item.itemType == ItemType.CombatItem)
                {
                    var temp = Resources.Load(ddata.item.combatItemClassName);
                    ((CombatItem)obj.Item).combatItemObject = ((GameObject)temp).GetComponent<CombatItemObject>();
                    ((CombatItem)obj.Item).MaxDamage = ddata.item.maxDamage;
                    ((CombatItem)obj.Item).MinDamage = ddata.item.minDamage;
                }

                if ((ItemType)ddata.item.itemType == ItemType.Armor)
                {
                    ((EquipmentObject)obj.Item).armorSubType = (ItemArmorSubType)ddata.item.armorSubType;
                }

                if (ddata.item.className.Contains("Health") || ddata.item.className.Contains("health"))
                {
                    ((HealthPotion)obj.Item).ReplenishAmount = ddata.item.regenAmount;
                    ((HealthPotion)obj.Item).drinkSound = Resources.Load<AudioClip>(ddata.item.drinkSound);
                }

                if (ddata.item.className.Contains("Mana") || ddata.item.className.Contains("mana"))
                {
                    ((ManaPotion)obj.Item).ReplenishAmount = ddata.item.regenAmount;
                    ((ManaPotion)obj.Item).drinkSound = Resources.Load<AudioClip>(ddata.item.drinkSound);
                }

                obj.Item.name = ddata.item.name;
                obj.Item.Id = ddata.item.id;
                obj.Item.uiDisplay = Resources.Load<Sprite>(ddata.item.spriteLink);
                obj.Item.type = (ItemType)ddata.item.itemType;
                obj.Item.description = ddata.item.decription;
                obj.Item.stackSize = ddata.item.stackSize;
                obj.Item.weight = ddata.item.weight;
                obj.Item.baseValue = ddata.item.value;
                obj.Item.rarity = (ItemRarity)ddata.item.rarity;
                obj.Amount = ddata.amount;
                obj.transform.position = new Vector3(ddata.positionX, ddata.positionY);
                groundItems.Add(obj);
            }

            for (int i = 0; i < Spawners.Count; i++)
            {
                Spawners[i].IsDead = data.spawnerDatas[i].isDead;
            }

            PlayerPosition = new Vector3(data.positionX, data.positionY);
        }      
    }

    public void LoadInventory(int inventoryId)
    {
        if(File.Exists(Application.persistentDataPath + "/saves/inventory" + inventoryId + "Data.ss"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saves/inventory" + inventoryId + "Data.ss", FileMode.Open);
            InventoryData data = (InventoryData)bf.Deserialize(file);
            file.Close();

            inventories[inventoryId].weight = data.weight;
            inventories[inventoryId].InventoryID = data.inventoryId;

            foreach (InventorySlot slot in inventories[inventoryId].Container.items)
            {
                slot.item = null;
            }

            for (int i = 0; i < data.container.items.Length; i++)
            {
                if (data.container.items[i] != null)
                {
                    inventories[inventoryId].Container.items[i].ID = data.container.items[i].id;
                    inventories[inventoryId].Container.items[i].amount = data.container.items[i].amount;
                    inventories[inventoryId].Container.items[i].weight = data.container.items[i].weight;
                    inventories[inventoryId].Container.items[i].value = data.container.items[i].value;

                    if (data.container.items[i].item != null)
                    {
                        inventories[inventoryId].Container.items[i].item = (ItemObject)ScriptableObject.CreateInstance(data.container.items[i].item.className);

                        if ((ItemType)data.container.items[i].item.itemType == ItemType.CombatItem)
                        {
                            var obj = Resources.Load(data.container.items[i].item.combatItemClassName);
                            ((CombatItem)inventories[inventoryId].Container.items[i].item).combatItemObject = ((GameObject)obj).GetComponent<CombatItemObject>();
                            ((CombatItem)inventories[inventoryId].Container.items[i].item).MinDamage = data.container.items[i].item.minDamage;
                            ((CombatItem)inventories[inventoryId].Container.items[i].item).MaxDamage = data.container.items[i].item.maxDamage;
                        }

                        if ((ItemType)data.container.items[i].item.itemType == ItemType.Armor)
                        {
                            ((EquipmentObject)inventories[inventoryId].Container.items[i].item).armorSubType = (ItemArmorSubType)data.container.items[i].item.armorSubType;
                        }

                        if (data.container.items[i].item.name.Contains("Health") || data.container.items[i].item.name.Contains("health"))
                        {
                            ((HealthPotion)inventories[inventoryId].Container.items[i].item).drinkSound = Resources.Load<AudioClip>(data.container.items[i].item.drinkSound);
                            ((HealthPotion)inventories[inventoryId].Container.items[i].item).ReplenishAmount = data.container.items[i].item.regenAmount;
                        }

                        if (data.container.items[i].item.name.Contains("Mana") || data.container.items[i].item.name.Contains("mana"))
                        {
                            ((ManaPotion)inventories[inventoryId].Container.items[i].item).drinkSound = Resources.Load<AudioClip>(data.container.items[i].item.drinkSound);
                            ((ManaPotion)inventories[inventoryId].Container.items[i].item).ReplenishAmount = data.container.items[i].item.regenAmount;
                        }

                        inventories[inventoryId].Container.items[i].item.name = data.container.items[i].item.name;
                        inventories[inventoryId].Container.items[i].item.Id = data.container.items[i].item.id;
                        inventories[inventoryId].Container.items[i].item.uiDisplay = Resources.Load<Sprite>(data.container.items[i].item.spriteLink);
                        inventories[inventoryId].Container.items[i].item.type = (ItemType)data.container.items[i].item.itemType;
                        inventories[inventoryId].Container.items[i].item.description = data.container.items[i].item.decription;
                        inventories[inventoryId].Container.items[i].item.stackSize = data.container.items[i].item.stackSize;
                        inventories[inventoryId].Container.items[i].item.weight = data.container.items[i].item.weight;
                        inventories[inventoryId].Container.items[i].item.baseValue = data.container.items[i].item.value;
                        inventories[inventoryId].Container.items[i].item.rarity = (ItemRarity)data.container.items[i].item.rarity;

                        if (data.container.items[i].item.buffs != null)
                        {
                            inventories[inventoryId].Container.items[i].item.buffs = new ItemBuff[data.container.items[i].item.buffs.Length];

                            for (int b = 0; b < data.container.items[i].item.buffs.Length; b++)
                            {
                                inventories[inventoryId].Container.items[i].item.buffs[b] = new ItemBuff(data.container.items[i].item.buffs[b].min, data.container.items[i].item.buffs[b].max);
                                inventories[inventoryId].Container.items[i].item.buffs[b].stat = (Stats)data.container.items[i].item.buffs[b].stat;
                                inventories[inventoryId].Container.items[i].item.buffs[b].Value = data.container.items[i].item.buffs[b].value;
                            }
                        }
                    }
                }
            }
        }

        OnInventoryLoad?.Invoke();
    }

    [Serializable]
    class WorldData
    {
        public List<GroundItemData> groundItemDatas;
        public List<SpawnerData> spawnerDatas;
        public float positionX;
        public float positionY;
    }

    [Serializable]
    class PlayerData
    {
        public int health;
        public int mana;
        public int maxHealth;
        public int maxMana;
        public int level;
        public int currentEXP;
        public int statPoints;
        public int vitality;
        public int wisdom;
        public int strength;
        public int dexterity;
        public int intelligence;
        public int gold;
    }

    [Serializable]
    class QuestData
    {
        public string Name;
        public bool accepted;
        public bool completed;
        public bool IsHandedIn;
        public int questStep;
        public string className;
        public string objectName;
    }

    [Serializable]
    class InventoryData
    {
        public int weight;
        public InventoryContainerData container;
        public int inventoryId;
    }

    [Serializable]
    class InventoryContainerData
    {
        public InventorySlotData[] items;
    }

    [Serializable]
    class InventorySlotData
    {
        public int id;
        public ItemObjectData item;
        public int amount;
        public int weight;
        public int value;
    }

    [Serializable]
    class ItemObjectData
    {
        public string name;
        public int id;
        public string className;
        public string spriteLink;
        public int itemType;
        public string decription;
        public int stackSize;
        public int weight;
        public int value;
        public BuffData[] buffs;
        public int rarity;
        public string combatItemClassName;
        public int minDamage;
        public int maxDamage;
        public int armorSubType;
        public int regenAmount;
        public string drinkSound;
    }

    [Serializable]
    class BuffData
    {
        public int stat;
        public int value;
        public int min;
        public int max;
    }

    [Serializable]
    class GroundItemData
    {
        public ItemObjectData item;
        public int amount;
        public float positionX;
        public float positionY;
    }

    [Serializable]
    class SpawnerData
    {
        public bool isDead;
    }
}