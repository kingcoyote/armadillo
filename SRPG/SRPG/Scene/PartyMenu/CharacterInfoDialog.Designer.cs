using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;

namespace SRPG.Scene.PartyMenu
{
    partial class CharacterInfoDialog
    {

        private CharacterOverviewDialog _overviewDialog;
        private CharacterStatsDialog _statsDialog;
        private CharacterInventoryDialog _inventoryDialog;
        private CharacterAbilityDialog _abilityDialog;

        private void InitializeComponent()
        {
            EnableDragging = false;

            Bounds = new UniRectangle(
                new UniScalar(0.2f, 15.0f), new UniScalar(0.0f, 105.0f),
                new UniScalar(0.8f, -15.0f), new UniScalar(1.0f, -105.0f)
            );

            //
            // overview dialog
            //
            _overviewDialog = new CharacterOverviewDialog();
            _overviewDialog.Bounds =  new UniRectangle(
                new UniScalar(20), new UniScalar(0, 20.0F),
                new UniScalar(0.6F, -40.0F), new UniScalar(0.0f, 200.0f)
            );
            Children.Add(_overviewDialog);

            //
            // stats dialog
            //
            _statsDialog = new CharacterStatsDialog();
            _statsDialog.Bounds = new UniRectangle(
                new UniScalar(0.6F, 0F), new UniScalar(0.0F, 20.0F),
                new UniScalar(0.4F, -20.0F), new UniScalar(0.0f, 200.0f)
            );
            Children.Add(_statsDialog);

            //
            // inventory dialog
            //
            _inventoryDialog = new CharacterInventoryDialog();
            _inventoryDialog.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 233.0F),
                new UniScalar(0.6F, -40.0F), new UniScalar(0.0f, 152.0f)
            );
            Children.Add(_inventoryDialog);

            //
            // ability dialog
            //
            _abilityDialog = new CharacterAbilityDialog();
            _abilityDialog.Bounds = new UniRectangle(
                new UniScalar(20), new UniScalar(400),
                new UniScalar(1.0f, -40), new UniScalar(1.0f, -420.0f)
            );
            Children.Add(_abilityDialog);
        }
    }
}
