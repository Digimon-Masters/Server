using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Models.Character;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.Commons.Packets.Items;
using System.Text;

namespace DigitalWorldOnline.Commons.Models.Base
{
    public partial class ItemListModel
    {
        /// <summary>
        /// Returns the current itens in inventory.
        /// </summary>
        public byte Count => (byte)Items.Count(x => x.ItemId != 0);

        /// <summary>
        /// Return the current free slots amount.
        /// </summary>
        public byte TotalEmptySlots => (byte)Items.Count(x => x.ItemId == 0);

        public int RetrieveEnabled => Count > 0 || Bits > 0 ? 100 : 0;

        /// <summary>
        /// Sort itens by ItemId.
        /// </summary>
        public void Sort()
        {
            var existingItens = Items
                .Where(x => x.ItemId > 0)
                .OrderByDescending(x => x.ItemInfo.Type)
                .ThenByDescending(x => x.ItemId)
                .ThenByDescending(x => x.Amount)
                .ToList();

            var emptyItens = Items
                .Where(x => x.ItemId == 0)
                .ToList();

            existingItens.AddRange(emptyItens);

            var slot = 0;

            foreach (var existingItem in existingItens)
            {
                existingItem.Slot = slot;
                slot++;
            }

            Items = existingItens;
        }

      
        /// <summary>
        /// Increase the current inventory size/slots.
        /// </summary>
        /// <param name="amount">Slots to add.</param>
        public byte AddSlots(byte amount = 1)
        {
            for (byte i = 0; i < amount; i++)
            {
                var newItemSlot = new ItemModel(Items.Max(x => x.Slot))
                {
                    ItemListId = Id
                };

                Items.Add(newItemSlot);

                Size++;
            }

            return Size;
        }

        /// <summary>
        /// Increase the current inventory size.
        /// </summary>
        public ItemModel AddSlot()
        {
            var newItemSlot = new ItemModel(Items.Max(x => x.Slot))
            {
                ItemListId = Id
                //ItemList = this
            };

            Items.Add(newItemSlot);

            Size++;

            return newItemSlot;
        }

        public bool RemoveOrReduceItemsBySection(int itemSection, int totalAmount)
        {
            var backup = BackupOperation();

            var targetAmount = totalAmount;
            var targetItems = FindItemsBySection(itemSection);
            targetItems = targetItems.OrderBy(x => x.Slot).ToList();
            foreach (var targetItem in targetItems)
            {
                if (targetItem.Amount >= targetAmount)
                {
                    targetItem.ReduceAmount(targetAmount);
                    targetAmount = 0;
                }
                else
                {
                    targetAmount -= targetItem.Amount;
                    targetItem.SetAmount();
                }

                if (targetAmount == 0)
                    break;
            }

            if (targetAmount > 0)
            {
                RevertOperation(backup);
                return false;
            }

            return true;
        }

        public bool RemoveOrReduceItemsByItemId(int itemId, int totalAmount)
        {
            var backup = BackupOperation();

            var targetAmount = totalAmount;
            var targetItems = FindItemsById(itemId);
            targetItems = targetItems.OrderBy(x => x.Slot).ToList();
            foreach (var targetItem in targetItems)
            {
                if (targetItem.Amount >= targetAmount)
                {
                    targetItem.ReduceAmount(targetAmount);
                    targetAmount = 0;
                }
                else
                {
                    targetAmount -= targetItem.Amount;
                    targetItem.SetAmount();
                }

                if (targetAmount == 0)
                    break;
            }

            if (targetAmount > 0)
            {
                RevertOperation(backup);
                return false;
            }

            return true;
        }

        public List<ItemModel> FindItemsBySection(int itemSection)
        {
            return Items
                .Where(x => x.Amount > 0 && x.ItemInfo?.Section == itemSection)
                .ToList();
        }

        public ItemModel? FindItemBySection(int itemSection)
        {
            return Items.FirstOrDefault(x => x.Amount > 0 && x.ItemInfo?.Section == itemSection);
        }

        public ItemModel? FindItemById(int itemId, bool allowEmpty = false)
        {
            if (allowEmpty)
                return Items.FirstOrDefault(x => itemId == x.ItemId);
            else
                return Items.FirstOrDefault(x => x.Amount > 0 && itemId == x.ItemId);
        }

        public int FindAvailableSlot(ItemModel targetItem)
        {
            var slot = Items.FindIndex(x =>
                x.ItemId == targetItem.ItemId &&
                x.Amount + targetItem.Amount < targetItem.ItemInfo.Overlap);

            if (slot < 0)
                slot = GetEmptySlot;

            return slot;
        }

        public List<ItemModel> FindItemsById(int itemId, bool allowEmpty = false)
        {
            if (allowEmpty)
                return Items.Where(x => itemId == x.ItemId).ToList();
            else
                return Items.Where(x => x.Amount > 0 && itemId == x.ItemId).ToList();
        }

        public ItemModel FindItemBySlot(int slot)
        {
            if (slot < 0) return null;

            var ItemInfo = Items.First(x => x.Slot == slot);

            return ItemInfo;
        }
        public ItemModel FindItemByTradeSlot(int slot)
        {
            if (slot < 0) return null;

            var ItemInfo = Items.First(x => x.TradeSlot == slot);

            return ItemInfo;
        }
        public ItemModel GiftFindItemBySlot(int slot)
        {
            if (slot < 0) return null;


            var ItemInfo = Items.FirstOrDefault(x => x.Slot == slot && x.ItemId > 0);

            if (ItemInfo == null)
                return null;

            return ItemInfo;
        }

        public bool UpdateGiftSlot()
        {
            var ItemInfo = Items.Where( x => x.ItemId > 0).ToList();

            if (ItemInfo.Count <= 0)
                return false;

            var slot = -1;

            foreach (var item in ItemInfo)
            {
                slot++;

                var newItem = new ItemModel();
                newItem.SetItemId(item.ItemId);
                newItem.SetAmount(item.Amount);
                newItem.SetItemInfo(item.ItemInfo);

                RemoveItem(item, (short)item.Slot);             
                AddItem(newItem);
            }

            return true;
        }
        /// <summary>
        /// Returns the first empty slot index or -1.
        /// </summary>
        public int GetEmptySlot => Items.FindIndex(x => x.ItemId == 0);

        public int InsertItem(ItemModel newItem)
        {
            var targetSlot = GetEmptySlot;
            newItem.Id = Items[targetSlot].Id;
            newItem.Slot = targetSlot;

            Items[targetSlot] = newItem;

            return targetSlot;
        }

        public bool AddBits(long bits)
        {
            if (Bits + bits > long.MaxValue)
            {
                Bits = long.MaxValue;
                return false;
            }
            else
            {
                Bits += bits;
                return true;
            }
        }

        public bool RemoveBits(long bits)
        {
            if (Bits >= bits)
            {
                Bits -= bits;
                return true;
            }
            else
            {
                Bits = 0;
                return false;
            }
        }

        public bool AddItems(List<ItemModel> itemsToAdd)
        {
            var backup = BackupOperation();

            foreach (var itemToAdd in itemsToAdd)
            {
                if (itemToAdd.Amount == 0 || itemToAdd.ItemId == 0)
                    continue;

                FillExistentSlots(itemToAdd);
                AddNewSlots(itemToAdd);

                if (itemToAdd.Amount > 0)
                {
                    RevertOperation(backup);
                    return false;
                }
            }

            CheckEmptyItems();
            return true;
        }

        //TODO: Retornar objeto contendo slots afetados e resultado final
        public bool AddItem(ItemModel newItem)
        {
            if (newItem.Amount == 0 || newItem.ItemId == 0)
                return false;

            var backup = BackupOperation();

            var itemToAdd = (ItemModel)newItem.Clone();

            FillExistentSlots(itemToAdd);
            AddNewSlots(itemToAdd);

            if (itemToAdd.Amount > 0)
            {
                RevertOperation(backup);
                return false;
            }

            CheckEmptyItems();

            newItem.Slot = itemToAdd.Slot;

            return true;
        }

        public bool AddItemWithSlot(ItemModel itemToAdd, int slot)
        {
            if (itemToAdd.Amount == 0 || itemToAdd.ItemId == 0)
                return false;

            var tempItem = (ItemModel)itemToAdd.Clone();

            var targetSlot = FindItemBySlot(slot);
            targetSlot.ItemId = tempItem.ItemId;
            targetSlot.Amount = tempItem.Amount;
            targetSlot.Power = tempItem.Power;
            targetSlot.RerollLeft = tempItem.RerollLeft;
            targetSlot.FamilyType = tempItem.FamilyType;
            targetSlot.Duration = tempItem.Duration;
            targetSlot.EndDate = tempItem.EndDate;
            targetSlot.FirstExpired = tempItem.FirstExpired;
            targetSlot.AccessoryStatus = tempItem.AccessoryStatus;
            targetSlot.SocketStatus = tempItem.SocketStatus;
            targetSlot.ItemInfo = tempItem.ItemInfo;

            return true;
        }

        public bool SplitItem(ItemModel itemToAdd, int targetSlot)
        {
            //TODO: Backup?

            if (itemToAdd == null || itemToAdd.Amount == 0 || itemToAdd.ItemId == 0)
                return false;

            FillExistentSlot(itemToAdd, targetSlot);

            CheckEmptyItems();

            return true;
        }

        private List<ItemModel> BackupOperation()
        {
            var backup = new List<ItemModel>();
            backup.AddRange(Items);
            return backup;
        }

        private void RevertOperation(List<ItemModel> backup)
        {
            Items.Clear();
            Items.AddRange(backup);
            CheckEmptyItems();
        }

        private void AddNewSlots(ItemModel itemToAdd)
        {
            while (itemToAdd.Amount > 0 && Count < Size)
            {
                itemToAdd.Slot = GetEmptySlot;

                var newItem = (ItemModel)itemToAdd.Clone();

                if (itemToAdd.Amount > itemToAdd.ItemInfo.Overlap)
                {
                    itemToAdd.ReduceAmount(itemToAdd.ItemInfo.Overlap);
                    newItem.SetAmount(itemToAdd.ItemInfo.Overlap);
                }
                else
                {
                    newItem.SetAmount(itemToAdd.Amount);
                    itemToAdd.SetAmount();
                }

                InsertItem(newItem);
            }
        }

        internal void CheckExpiredItems()
        {
            
        }

        private void FillExistentSlots(ItemModel itemToAdd)
        {
            var targetItems = FindItemsById(itemToAdd.ItemId);

            foreach (var targetItem in targetItems.Where(x => x.ItemInfo.Overlap > 1))
            {
                if (targetItem.Amount + itemToAdd.Amount > itemToAdd.ItemInfo.Overlap)
                {
                    itemToAdd.ReduceAmount(itemToAdd.ItemInfo.Overlap - targetItem.Amount);
                    targetItem.SetAmount(itemToAdd.ItemInfo.Overlap);
                }
                else
                {
                    targetItem.IncreaseAmount(itemToAdd.Amount);
                    itemToAdd.SetAmount();
                }

                itemToAdd.Slot = targetItem.Slot;
            }
        }

        private void FillExistentSlot(ItemModel itemToAdd, int targetSlot)
        {
            var targetItem = FindItemBySlot(targetSlot);

            if (targetItem.ItemId == itemToAdd.ItemId || targetItem.ItemId == 0)
            {
                if (targetItem.Amount + itemToAdd.Amount > itemToAdd.ItemInfo.Overlap)
                {
                    itemToAdd.IncreaseAmount(itemToAdd.ItemInfo.Overlap - targetItem.Amount);
                    targetItem.SetAmount(targetItem.ItemInfo.Overlap);
                }
                else
                {
                    targetItem.IncreaseAmount(itemToAdd.Amount);
                    itemToAdd.SetAmount();
                }

                targetItem.SetItemId(itemToAdd.ItemId);
                targetItem.SetRemainingTime((uint)itemToAdd.ItemInfo.UsageTimeMinutes);
            }
        }

        public bool MoveItem(short originSlot, short destinationSlot)
        {
            var originItem = FindItemBySlot(originSlot);
            var destinationItem = FindItemBySlot(destinationSlot);

            if (originItem.ItemId == 0)
                return false;

            if (originItem.ItemId == destinationItem.ItemId)
            {
                if (originItem.Amount + destinationItem.Amount > originItem.ItemInfo.Overlap)
                {
                    originItem.ReduceAmount(originItem.ItemInfo.Overlap - destinationItem.Amount);
                    destinationItem.SetAmount(originItem.ItemInfo.Overlap);
                }
                else
                {
                    destinationItem.IncreaseAmount(originItem.Amount);
                    originItem.SetAmount();
                }
            }
            else
            {
                if (destinationItem.ItemId == 0)
                {
                    var tempItem = (ItemModel)originItem.Clone(destinationItem.Id);
                    tempItem.Slot = destinationItem.Slot;

                    destinationItem = tempItem;
                    originItem.SetItemId();
                }
                else
                {
                    var tempItem = (ItemModel)destinationItem.Clone(originItem.Id);
                    tempItem.Slot = originItem.Slot;

                    var tempItem2 = (ItemModel)originItem.Clone(destinationItem.Id);
                    tempItem2.Slot = destinationItem.Slot;

                    destinationItem = tempItem2;
                    originItem = (ItemModel)tempItem.Clone(originItem.Id);
                }
            }

            Items[originSlot] = originItem;
            Items[destinationSlot] = destinationItem;

            return true;
        }

        public void Clear()
        {
            foreach (var item in Items)
            {
                item.SetItemId();
                item.SetAmount();
                item.SetRemainingTime();
                item.SetSellPrice(0);
            }
        }

        public bool RemoveOrReduceItems(List<ItemModel> itemsToRemoveOrReduce)
        {
            var backup = BackupOperation();

            //TODO: teste com 2 slots do mesmo itemId
            foreach (var itemToRemove in itemsToRemoveOrReduce)
            {
                if (itemToRemove.Amount == 0 || itemToRemove.ItemId == 0)
                    continue;

                var targetItems = FindItemsById(itemToRemove.ItemId);

                foreach (var targetItem in targetItems)
                {
                    if (targetItem.Amount >= itemToRemove.Amount)
                    {
                        targetItem.ReduceAmount(itemToRemove.Amount);
                        itemToRemove.SetAmount();
                        break;
                    }
                    else
                    {
                        itemToRemove.ReduceAmount(targetItem.Amount);
                        targetItem.SetAmount();
                    }
                }

                if (itemToRemove.Amount > 0)
                {
                    RevertOperation(backup);
                    return false;
                }
            }

            CheckEmptyItems();
            return true;
        }

        public bool RemoveOrReduceItem(ItemModel? itemToRemove, int amount, int slot = -1)
        {
            if (itemToRemove == null || amount == 0) return false;

            var tempItem = (ItemModel?)itemToRemove.Clone();
            tempItem?.SetAmount(amount);

            return slot > -1 ?
                RemoveOrReduceItemWithSlot(tempItem, slot) :
                RemoveOrReduceItemWithoutSlot(tempItem);
        }

        public bool RemoveOrReduceItemWithSlot(ItemModel? itemToRemove, int slot)
        {
            if (itemToRemove == null || itemToRemove.Amount == 0 || itemToRemove.ItemId == 0)
                return false;

            var backup = BackupOperation();

            var targetItem = FindItemBySlot(slot);
            targetItem?.ReduceAmount(itemToRemove.Amount);
            itemToRemove.SetAmount();

            if (itemToRemove.Amount > 0)
            {
                RevertOperation(backup);
                return false;
            }

            CheckEmptyItems();
            return true;
        }

        public bool RemoveOrReduceItemWithoutSlot(ItemModel? itemToRemove)
        {
            if (itemToRemove == null || itemToRemove.Amount == 0 || itemToRemove.ItemId == 0)
                return false;

            var backup = BackupOperation();

            var targetItems = FindItemsById(itemToRemove.ItemId);

            foreach (var targetItem in targetItems)
            {
                if (targetItem.Amount >= itemToRemove.Amount)
                {
                    targetItem.ReduceAmount(itemToRemove.Amount);
                    itemToRemove.SetAmount();
                    break;
                }
                else
                {
                    itemToRemove.ReduceAmount(targetItem.Amount);
                    targetItem.SetAmount();
                }
            }

            if (itemToRemove.Amount > 0)
            {
                RevertOperation(backup);
                return false;
            }

            CheckEmptyItems();
            return true;
        }

        public bool RemoveItem(ItemModel itemToRemove, short slot)
        {
            if (itemToRemove == null || itemToRemove.Amount == 0 || itemToRemove.ItemId == 0)
                return false;

            var backup = BackupOperation();

            var targetItem = FindItemBySlot(slot);

            if (targetItem == null)
                return false;

            if (targetItem.Amount >= itemToRemove.Amount)
            {
                targetItem.ReduceAmount(itemToRemove.Amount);
                itemToRemove.SetAmount();
                CheckEmptyItems();
                return true;
            }
            else
            {
                RevertOperation(backup);
                CheckEmptyItems();
                return false;
            }
        }

        public void CheckEmptyItems()
        {
            Items.ForEach(item =>
            {
                if (item.ItemId == 0 || item.Amount <= 0)
                {
                    item.SetItemId();
                    item.SetAmount();
                    item.SetRemainingTime();
                    item.SetSellPrice(0);
                }
            });
        }

        /// <summary>
        /// Serializes the current instance to byte array.
        /// </summary>
        /// <returns>The byte array result.</returns>
        public byte[] ToArray()
        {
            byte[] buffer;

            using (MemoryStream m = new())
            {
                var sortedItems = Items.OrderBy(x => x.Slot);

                foreach (var item in sortedItems)
                    m.Write(item.ToArray(), 0, 68);

                buffer = m.ToArray();
            }

            return buffer;
        }

        /// <summary>
        /// Serializes the current instance to byte array.
        /// </summary>
        /// <returns>The byte array result.</returns>
        public byte[] GiftToArray()
        {
            byte[] buffer;

            using (MemoryStream m = new())
            {
                var filteredItems = Items.Where(x => x.ItemId > 0).OrderBy(x => x.Slot);

                if (filteredItems.Any())
                {
                    foreach (var item in filteredItems)
                    {
                        m.Write(item.GiftToArray(), 0, 68);
                    }
                    buffer = m.ToArray();
                }
                else
                {
                    buffer = new byte[0]; // Nenhum item com ItemId > 0 encontrado, retornar um array vazio.
                }
            }

            return buffer;
        }
    

        public string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Inventory{Id}");
            foreach (var item in Items.OrderBy(x => x.Slot))
            {
                sb.AppendLine($"Item[{item.Slot}] - {item.ItemId}");
                sb.AppendLine(item.ToString());
            }

            return sb.ToString();
        }
    }
}