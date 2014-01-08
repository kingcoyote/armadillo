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

        private void InitializeComponent()
        {
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
                new UniScalar(0.6F, -20.0F), new UniScalar(0.0f, 220.0f)
            );
            Children.Add(_overviewDialog);

            //
            // stats dialog
            //
            _statsDialog = new CharacterStatsDialog();
            _statsDialog.Bounds = new UniRectangle(
                new UniScalar(0.6F, 0F), new UniScalar(0.0F, 20.0F),
                new UniScalar(0.4F, -20.0F), new UniScalar(0.0f, 220.0f)
            );
            Children.Add(_statsDialog);

            //
            // inventory dialog
            //
            _inventoryDialog = new CharacterInventoryDialog();
            _inventoryDialog.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 240.0F),
                new UniScalar(0.6F, -20.0F), new UniScalar(0.0f, 115.0f)
            );
            Children.Add(_inventoryDialog);

            // todo add abilities dialog
        }

        public void Hide()
        {
            if (Bounds.Location.X.Offset < -50000) return;

            Bounds.Location.X.Offset -= 50000;
        }

        public void Show()
        {
            if (Bounds.Location.X.Offset > -40000) return;

            Bounds.Location.X.Offset += 50000;
        }
    }
}
