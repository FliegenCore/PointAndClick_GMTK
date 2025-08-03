using System;
using _Game.Scripts.Dialogues;
using _Game.Scripts.HelpSystem;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.Inventory
{
    public class InventoryController : MonoBehaviour, IService
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private InventoryView _inventoryView;
        private InventoryData _inventoryData;
        
        public InventoryView View => _inventoryView;
        
        public void Initialize()
        {
            _inventoryData = new InventoryData();         
        }
#if UNITY_EDITOR
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddItem("berries");
            }
        }
#endif

        public void AddItem(string itemName, int count = 1)
        {
            if (count <= 0) return;

            if (_inventoryData.Items.Count >= 2)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                G.Get<HelpTooltipController>().SpawnHelp(pos, "No space in inventory");
                return;
            }
            
            _inventoryData.Items.Add(itemName);
            _audioSource.Play();
            RefreshUI();
        }

        public void RemoveItem(string itemName)
        {
            _inventoryData.Items.Remove(itemName);
            _audioSource.Play();
            RefreshUI();
        }

        private void RefreshUI()
        {
            foreach (var cell in _inventoryView.Cells)
            {
                if (cell.CurrentItem != null)
                {
                    Destroy(cell.CurrentItem.gameObject);
                    cell.CurrentItem = null;
                }
            }
            
            for (int i = 0; i < _inventoryData.Items.Count; i++)
            {
                if (_inventoryView.Cells[i].CurrentItem == null)
                {
                    string nme = _inventoryData.Items[i];
                    
                    UIItem item = InstintiateItem();
                    item.transform.parent = _inventoryView.Cells[i].transform;
                    item.Id = nme;
                    item.transform.position = _inventoryView.Cells[i].transform.position;
                    item.sprite = LoadSprite(nme);
                    item.SetNativeSize();
                    _inventoryView.Cells[i].CurrentItem = item;
                }
            }
        }

        private UIItem InstintiateItem()
        {
            var asset = Resources.Load<UIItem>("Prefabs/UI/Item");
            
            return Instantiate(asset);
        }

        private Sprite LoadSprite(string itemName)
        {
            return Resources.Load<Sprite>("Sprites/" + itemName);
        }
    }
}