using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    private const float gravityScale = 9.8f, speedScale = 5f, jumpForce = 8f, turnSpeed = 90f;
    private float verticalSpeed = 0f, mouseX = 0f, mouseY = 0f, currentCameraAngelX = 0f;
    [SerializeField] private CharacterController charecterConn;
    [SerializeField] private GameObject playerCamera;

    [SerializeField] private GameObject particleBlockObject, tool;
    private const float hitScaleSpeed = 15f;
    private float hitLastTime;


    public List<ItemData> inventoryItems, currentChestItems;
    [SerializeField]
    private Transform inventoryContent;
    private bool canMove = true;

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
    }

    private void Update()
    {
        if (canMove)
        {
            Move();
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 5f))
            {
                if (Input.GetMouseButton(0))
                {
                    ObjectINteraction(hit.transform.gameObject);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenInventory();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            CloseInventoryPanel();
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
            tool.GetComponent<Animator>().SetTrigger("attack");
            hitLastTime = Time.time;
            block.health -= tool.GetComponent<Tools>().damageToBlock;
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
                Dig(currentObj.GetComponent<Block>());
                break;
            case "Enemy":
                break;
            case "Chest":
                currentChestItems = currentObj.GetComponent<Chest>().chestItems;
                OpenChest();
                break;
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
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.StartsWith("mini"))
        {
            InvMenager.instance.CreateItem(2, inventoryItems);
            Destroy(col.gameObject);
        }
    }
}
