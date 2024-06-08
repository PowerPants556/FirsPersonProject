using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    public const string EQUIPE_NOT_SELECTED_TEXT = "EquipeNotSelected";
    private const float gravityScale = 9.8f, speedScale = 5f, jumpForce = 8f, turnSpeed = 90f;
    private float verticalSpeed = 0f, mouseX = 0f, mouseY = 0f, currentCameraAngelX = 0f;
    [SerializeField] private CharacterController charecterConn;
    [SerializeField] private GameObject playerCamera;

    [SerializeField] private GameObject particleBlockObject;
    private GameObject currentEquipedItem;
    private const float hitScaleSpeed = 15f;
    private float hitLastTime;
    [HideInInspector]public string itemYouCanEquipeName = EQUIPE_NOT_SELECTED_TEXT;


    public List<ItemData> inventoryItems, currentChestItems;
    [SerializeField]
    private Transform inventoryContent;
    private bool canMove = true;
    [SerializeField] private GameObject[] equipableItems;

    private RaycastHit hit;


    public static PlayerCon instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        InvMenager.instance.CreateItem(0, inventoryItems);
        EquipItem("Pickaxe");
    }

    private void Update()
    {
        if (canMove)
        {
            Move();
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
            {
                if (Input.GetMouseButton(0))
                {
                    ObjectINteraction(hit.transform.gameObject);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    ItemAbility();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && !InvMenager.instance.GetInventoryPanel().activeSelf)
        {
            OpenInventory();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            CloseInventoryPanel();
        }
        else if (Input.GetKeyDown(KeyCode.E) &&
            InvMenager.instance.GetInventoryPanel().activeSelf &&
            itemYouCanEquipeName != EQUIPE_NOT_SELECTED_TEXT)
        {
            EquipItem(itemYouCanEquipeName);
        }
    }

    private void FixedUpdate()
    {
        if(canMove)
        RotateCharecter();
    }

    private void RotateCharecter()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        transform.Rotate(new Vector3(0f, mouseX * turnSpeed * Time.fixedDeltaTime, 0f));
        currentCameraAngelX += mouseY * Time.fixedDeltaTime * turnSpeed * -1;
        currentCameraAngelX = Mathf.Clamp(currentCameraAngelX, -60f, 60f);
        playerCamera.transform.localEulerAngles = new Vector3(currentCameraAngelX, 0f, 0f);
    }

    private void Move()
    {
        Vector3 velocity = new Vector3 (Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        velocity = transform.TransformDirection(velocity) * speedScale;
        if (charecterConn.isGrounded)
        {
            verticalSpeed = 0f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalSpeed = jumpForce;
            }
        }
        verticalSpeed -= gravityScale * Time.deltaTime;
        velocity.y = verticalSpeed;
        charecterConn.Move(velocity * Time.deltaTime);
    }

    private void Dig(Block block)
    {
        if (Time.time - hitLastTime > 1 / hitScaleSpeed)
        {
            currentEquipedItem.GetComponent<Animator>().SetTrigger("attack");
            hitLastTime = Time.time;

            Tools currentToolInfo;
            if (currentEquipedItem.TryGetComponent<Tools>(out currentToolInfo))
            {
                block.health -= currentToolInfo.damageToBlock;
            }
            else
            {
                block.health -= 1;
            }
            GameObject particleObj = Instantiate(particleBlockObject, block.transform.position, Quaternion.identity);
            particleObj.GetComponent<ParticleSystemRenderer>().material = block.GetComponent<MeshRenderer>().material;

            if(block.health <= 0)
            {
                block.DestroyBehavior();
            }
        }
    }

    private void ObjectINteraction (GameObject currentObj)
    {
        switch (currentObj.tag)
        {
            case "Block":
                if(Vector3.Distance(transform.position, currentObj.transform.position) < 5f)
                {
                    Dig(currentObj.GetComponent<Block>());
                }
                break;
            case "Enemy":
                Attack(currentObj);
                break;
            case "Chest":
                if(Vector3.Distance(transform.position, currentObj.transform.position) < 5f)
                {
                    currentChestItems = currentObj.GetComponent<Chest>().chestItems;
                    OpenChest();
                }
                break;
        }
    }

    private void Attack(GameObject currentObj)
    {
        if (Time.time - hitLastTime > 1 / hitScaleSpeed)
        {
            hitLastTime = Time.time;
            var enemy = currentObj.GetComponent<Enemy>();
            enemy.TakeDamage(currentEquipedItem.GetComponent<Tools>().damageToEnemy);
        }
    }

    private void OpenInventory()
    {
        if (!InvMenager.instance.GetInventoryPanel().activeSelf)
        {
            SwitchCursor(true, CursorLockMode.Confined);

            InvMenager.instance.GetInventoryPanel().SetActive(true);
            if (inventoryItems.Count > 0)
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    InvMenager.instance.InstantiatingItem(inventoryItems[i],
                        InvMenager.instance.GetInventoryContent().transform, InvMenager.instance.invSlots);
                }
            }
        }
    }

    private void OpenChest()
    {
        SwitchCursor(true, CursorLockMode.Confined);

        if (!InvMenager.instance.GetChestPanel().activeSelf);
        {
            SwitchCursor(true, CursorLockMode.Confined);
            InvMenager.instance.GetChestPanel().SetActive(true);
            for (int i = 0;i < currentChestItems.Count; i++)
            {
                InvMenager.instance.InstantiatingItem(currentChestItems[i], InvMenager.instance.GetChestContent().transform, InvMenager.instance.currentChestSlots);
            }
        }
    }
    private void SwitchCursor(bool active, CursorLockMode lockMode)
    {
        Cursor.visible = active;
        Cursor.lockState = lockMode;
        canMove = !active;
    }
    private void CloseInventoryPanel()
    {
        SwitchCursor(false, CursorLockMode.Locked);
        foreach (GameObject slot in InvMenager.instance.currentChestSlots)
        {
            Destroy(slot);
        }
        foreach(GameObject slot in InvMenager.instance.invSlots)
        {
            Destroy(slot);
        }
        InvMenager.instance.currentChestSlots.Clear();
        InvMenager.instance.invSlots.Clear();
        InvMenager.instance.GetChestPanel().SetActive(false);
        InvMenager.instance.GetInventoryPanel().SetActive(false);
    }

    private void EquipItem(string toolName)
    {
        foreach(GameObject tool in equipableItems)
        {
            if(toolName == tool.name)
            {
                tool.SetActive(true);
                currentEquipedItem = tool;
                toolName = EQUIPE_NOT_SELECTED_TEXT;
            }
            else
            {
                tool.SetActive(false);
            }
        }
    }

    private void ItemAbility()
    {
        switch (currentEquipedItem.name)
        {
            case "Ground":
                CreateBlock();
                break;
            case "Meat":
                break;
            default:
                break;
        }

    }

    private void CreateBlock()
    {
        GameObject blockPref = Resources.Load<GameObject>("Ground");
        Vector3 tempPos = hit.transform.gameObject.transform.position;
        Vector3 newBlockPos = Vector3.zero;
        if(hit.transform.gameObject.tag == "Block")
        {
            GameObject currentBlock = Instantiate(blockPref);
            if (hit.point.y == tempPos.y + 0.5f)
            {
                newBlockPos = new Vector3(tempPos.x, tempPos.y + 1, tempPos.z);
            }
            else if (hit.point.y == tempPos.y - 0.5f)
            {
                newBlockPos = new Vector3(tempPos.x, tempPos.y - 1, tempPos.z);
            }

            if (hit.point.x == tempPos.x + 0.5f)
            {
                newBlockPos = new Vector3(tempPos.x + 1, tempPos.y, tempPos.z);
            }
            else if (hit.point.x == tempPos.x - 0.5f)
            {
                newBlockPos = new Vector3(tempPos.x - 1 , tempPos.y, tempPos.z);
            }

            if (hit.point.z == tempPos.z + 0.5f)
            {
                newBlockPos = new Vector3(tempPos.x, tempPos.y, tempPos.z + 1);
            }
            else if (hit.point.z == tempPos.z - 0.5f)
            {
                newBlockPos = new Vector3(tempPos.x, tempPos.y, tempPos.z - 1);
            }

            currentBlock.transform.position = newBlockPos;
            currentBlock.transform.SetParent(hit.transform.parent);
            ModifyItemCount("Ground");
        }
    }

    private void ModifyItemCount(string itemName)
    {
        foreach(ItemData item in inventoryItems)
        {
            if(item.name == itemName)
            {
                item.count--;
                if(item.count <= 0)
                {
                    inventoryItems.Remove(item);
                    if(inventoryItems.Count > 0)
                    {
                        EquipItem(inventoryItems[0].name);
                    }
                }
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.StartsWith("mini"))
        {
            InvMenager.instance.CreateItem(2, inventoryItems);
            Destroy(col.gameObject);
        }
    }
}
