using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSetting : MonoBehaviour
{
    [SerializeField] private GameObject mainInterfaceWindow;
    [SerializeField] private GameObject mainInterfaceCallButton;
    [SerializeField] private GameObject interfaceParent;
    [Space(5)]
    [SerializeField] private GameObject interfaceBlock;

    private void OnEnable()
    {
        TakeableBuilding.giveBlockToInterface += AddBlockToInterface;
        TakeableCrowd.addPeople += ActivateAddingPeopleMenu;
    }

    private void OnDisable()
    {
        TakeableBuilding.giveBlockToInterface -= AddBlockToInterface;
        TakeableCrowd.addPeople -= ActivateAddingPeopleMenu;
    } 

    private void AddBlockToInterface(TowerBlock block)
    {
        var newBlock = Instantiate(interfaceBlock, interfaceParent.transform);
        newBlock.GetComponent<Image>().sprite = block.icon;
        newBlock.GetComponent<HumanToBlock>().towerBlock = block;
        newBlock.transform.Find("Description").Find("DescriptionText").GetComponent<TextMeshProUGUI>().text = block.description;
    }

    private void ActivateAddingPeopleMenu()
    {
        mainInterfaceWindow.SetActive(true);
        mainInterfaceCallButton.SetActive(false);
    }
}
