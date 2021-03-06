﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.SystemModule;
using Xafology.ExpressApp.Xpo;

namespace Xafology.ExpressApp.BatchDelete
{
    public class BatchDeleteListViewController : ViewController
    {
        private const string deleteCaption = "Delete";
        private const string purgeCaption = "Purge";

        private SingleChoiceAction batchDeleteAction;

        public BatchDeleteListViewController()
        {
            TargetViewType = ViewType.ListView;
            TargetObjectType = typeof(IBatchDeletable);

            batchDeleteAction = new SingleChoiceAction(this, "BatchUpdateAction", DevExpress.Persistent.Base.PredefinedCategory.Edit);
            batchDeleteAction.Caption = "Batch";
            batchDeleteAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            batchDeleteAction.Execute += batchDeleteAction_Execute;
            batchDeleteAction.ShowItemsOnClick = true;

            var deleteChoice = new ChoiceActionItem();
            deleteChoice.Caption = deleteCaption;
            batchDeleteAction.Items.Add(deleteChoice);

            var purgeChoice = new ChoiceActionItem();
            purgeChoice.Caption = purgeCaption;
            batchDeleteAction.Items.Add(purgeChoice);
        }

        private void batchDeleteAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var objSpace = (XPObjectSpace)Application.CreateObjectSpace();
            var currentTypeInfo = ((ObjectView)View).ObjectTypeInfo;
            var currentType = currentTypeInfo.Type;
            XPClassInfo classInfo = objSpace.Session.GetClassInfo(currentType);
            CriteriaOperator criteria = ActiveFilterEnabled ? ActiveFilterCriteria : null;

            switch (e.SelectedChoiceActionItem.Caption)
            {
                case deleteCaption:
                    var message = new GenericMessageBox("This will delete all objects filtered in the current view. Do you wish to continue?",
                        "Confirm",
                        (sender1, svp1) => DeleteObjects(objSpace, criteria, classInfo),
                        (sender1, svp1) => { return; });
                    break;
                case purgeCaption:
                    objSpace.Session.PurgeDeletedObjects();
                    break;
            }
        }

        private void DeleteObjects(XPObjectSpace objSpace, CriteriaOperator criteria, XPClassInfo classInfo)
        {
            var deleter = new BatchDeleter(objSpace);
            deleter.Delete(classInfo, criteria);
        }

        public virtual CriteriaOperator ActiveFilterCriteria { get { return null; } }
        public virtual bool ActiveFilterEnabled { get { return false; } }
    }
}
