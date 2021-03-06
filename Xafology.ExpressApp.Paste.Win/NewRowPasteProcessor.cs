﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Paste.Parameters;

namespace Xafology.ExpressApp.Paste.Win
{
    public class NewRowPasteProcessor
    {
        private readonly ICopyParser copyParser;
        private readonly View view;
        private readonly PasteUtils pasteUtils;

        public NewRowPasteProcessor(ICopyParser copyParser, View view)
        {
            if (view == null)
                throw new ArgumentException("Parameter 'view' cannot be null", "view");
            if (copyParser == null)
                throw new ArgumentException("Parameter 'copyParser' cannot be null", "copyParser");

            this.copyParser = copyParser;
            this.view = view;
            this.pasteUtils = new PasteUtils();
        }

        // note that the new row must be focused for this to work
        public void Process(PasteParam pasteParam)
        {
            var listview = (ListView)view;
            GridListEditor listEditor = listview.Editor as GridListEditor;
            var gridView = listEditor.GridView;
            var copiedValues = copyParser.ToArray();
            var newRowHandle = gridView.FocusedRowHandle;

            if (!gridView.IsNewItemRow(gridView.FocusedRowHandle))
                return;

            // paste rows
            for (int r = 0; r < copiedValues.Length; r++)
            {
                // ignore row with empty string
                if (copiedValues[r].Length == 1 && string.IsNullOrWhiteSpace(copiedValues[r][0]))
                    continue;

                // add new row in gridview
                gridView.FocusedRowHandle = newRowHandle;
                gridView.AddNewRow();

                // paste cells
                pasteUtils.PasteColumnsToRow(copiedValues[r], gridView.FocusedRowHandle,
                    listview, pasteParam);

                gridView.UpdateCurrentRow();
            }
        }

    }
}
