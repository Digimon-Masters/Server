    using AutoMapper;
    using DigitalWorldOnline.Application.Admin.Commands;
    using DigitalWorldOnline.Application.Admin.Queries;
    using DigitalWorldOnline.Commons.DTOs.Config;
    using DigitalWorldOnline.Commons.ViewModel.Asset;
    using DigitalWorldOnline.Commons.ViewModel.Summons;
    using MediatR;
    using Microsoft.AspNetCore.Components;
    using MudBlazor;
    using Serilog;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    namespace DigitalWorldOnline.Admin.Pages.Summons
    {
        public partial class SummonCreation
        {
            private MudAutocomplete<MobAssetViewModel> _selectedMobAsset;
            private MudAutocomplete<ItemAssetViewModel> _selectedItemAsset;

            SummonViewModel _summon = new SummonViewModel();
            bool Loading = false;

            [Inject]
            public NavigationManager Nav { get; set; }

            [Inject]
            public ISender Sender { get; set; }

            [Inject]
            public IMapper Mapper { get; set; }

            [Inject]
            public ISnackbar Toast { get; set; }

            [Inject]
            public ILogger Logger { get; set; }

            private async Task<IEnumerable<MobAssetViewModel>> GetMobAssets(string value)
            {
                if (string.IsNullOrEmpty(value) || value.Length < 3)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        _selectedMobAsset.Clear();
                    }

                    return Array.Empty<MobAssetViewModel>();
                }

                var assets = await Sender.Send(new GetMobAssetQuery(value));

                return Mapper.Map<List<MobAssetViewModel>>(assets.Registers);
            }

            private async Task<IEnumerable<ItemAssetViewModel>> GetItemAssets(string value)
            {
                if (string.IsNullOrEmpty(value) || value.Length < 3)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        _selectedItemAsset.Clear();
                    }

                    return Array.Empty<ItemAssetViewModel>();
                }

                var assets = await Sender.Send(new GetItemAssetQuery(value));

                return Mapper.Map<List<ItemAssetViewModel>>(assets.Registers);
            }

            private void UpdateMobFields()
            {
                if (_selectedMobAsset.Value != null && _summon.SummonedMobs.Any())
                {
                    var selectedMob = Mapper.Map<SummonMobViewModel>(_selectedMobAsset.Value);
                    var backupExp = _summon.SummonedMobs.Last().ExpReward;
                    var backupLocation = _summon.SummonedMobs.Last().Location;
                    var backupDrop = _summon.SummonedMobs.Last().DropReward;
                    var backupSpawn = _summon.SummonedMobs.Last().Duration;

                    // Mantendo os valores originais antes da atualização
                    selectedMob.ExpReward = backupExp;
                    selectedMob.Location = backupLocation;
                    selectedMob.DropReward = backupDrop;
                    selectedMob.Duration = backupSpawn > 5 ? backupSpawn : 5;

                    _summon.SummonedMobs.Add(selectedMob);
                }

                StateHasChanged();
            }

            private void AddDrop()
            {
                if (_summon.SummonedMobs.Any() && _summon.SummonedMobs.LastOrDefault()?.DropReward?.Drops.Any() == true)
                {
                    long maxId = _summon.SummonedMobs.Last().DropReward.Drops.Max(x => x.Id);
                    _summon.SummonedMobs.Last().DropReward.Drops.Add(new SummonMobItemDropViewModel(maxId));
                }
                else
                {
                    _summon.SummonedMobs.Last().DropReward.Drops.Add(new SummonMobItemDropViewModel());
                }

                StateHasChanged();
            }
            private void DeleteDrop(long id)
            {
                foreach (var mob in _summon.SummonedMobs)
                {
                    mob.DropReward.Drops.RemoveAll(x => x.Id == id);
                }

                StateHasChanged();
            }

            private async Task Create()
            {
                if (_summon.SummonedMobs.Count == 0)
                    return;

                try
                {
                    Loading = true;
                    StateHasChanged();

                    Logger.Information("Creating new mobs");

                    foreach (var mob in _summon.SummonedMobs)
                    {
                        mob.DropReward.Drops.RemoveAll(x => x.ItemInfo == null);

                        mob.DropReward.Drops.ForEach(drop =>
                        {
                            drop.ItemId = drop.ItemInfo.ItemId;
                        });
                    }

                    Logger.Information("Mobs created");

                    Toast.Add("Mobs created successfully.", Severity.Success);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error creating mobs: {ex}", ex.Message);
                    Toast.Add("Unable to create mobs, try again later.", Severity.Error);
                }
                finally
                {
                    Loading = false;
                    StateHasChanged();
                    Return();
                }
            }


            private void Return()
            {              
                    Nav.NavigateTo($"/maps");
            }
      
        }
    }